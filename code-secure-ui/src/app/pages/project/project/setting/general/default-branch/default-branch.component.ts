import {Component, OnInit} from '@angular/core';
import {Button, ButtonDirective} from "primeng/button";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ProjectService} from '../../../../../../api/services/project.service';
import {ProjectStore} from '../../../project.store';
import {InputText} from 'primeng/inputtext';
import {NgIcon} from '@ng-icons/core';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../../../../shared/services/toastr.service';

@Component({
  selector: 'app-default-branch',
  imports: [
    ButtonDirective,
    FormsModule,
    ReactiveFormsModule,
    Button,
    InputText,
    NgIcon
  ],
  templateUrl: './default-branch.component.html',
  styleUrl: './default-branch.component.scss'
})
export class DefaultBranchComponent implements OnInit {
  loading = false;
  branches: string[] = [];

  constructor(
    private projectService: ProjectService,
    private store: ProjectStore,
    private toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.projectService.getDefaultBranchesProject({
      projectId: this.store.projectId()
    }).subscribe(branches => {
      this.branches = branches;
    });
  }

  addBranch() {
    this.branches.push('');
  }

  save() {
    this.loading = true;
    this.branches = this.branches.filter(item => item.length > 0).map(item => item.trim());
    this.projectService.updateDefaultBranchesProject({
      projectId: this.store.projectId(),
      body: this.branches
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    })
  }

  deleteBranch(branch: string) {
    this.branches = this.branches.filter(value => value != branch);
  }
}
