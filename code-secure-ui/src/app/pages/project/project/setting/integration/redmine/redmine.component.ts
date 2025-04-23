import {Component, inject, OnInit} from '@angular/core';
import {Button, ButtonDirective} from "primeng/button";
import {RouterLink} from "@angular/router";
import {Select} from "primeng/select";
import {RedmineMetadata} from '../../../../../../api/models/redmine-metadata';
import {RedmineProjectSetting} from '../../../../../../api/models/redmine-project-setting';
import {ProjectService} from '../../../../../../api/services';
import {ProjectStore} from '../../../project.store';
import {FormsModule} from '@angular/forms';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../../../../shared/services/toastr.service';
import {ToggleSwitch} from 'primeng/toggleswitch';

@Component({
  selector: 'redmine-integration-project',
  imports: [
    Button,
    ButtonDirective,
    RouterLink,
    Select,
    FormsModule,
    ToggleSwitch
  ],
  templateUrl: './redmine.component.html',
  styleUrl: './redmine.component.scss'
})
export class RedmineComponent implements OnInit {
  setting: RedmineProjectSetting = {};
  metadata: RedmineMetadata = {};
  loading = false;
  loadingMetadata = false;
  private store = inject(ProjectStore);
  private projectService = inject(ProjectService);
  private toastr = inject(ToastrService);

  ngOnInit(): void {
    this.projectService.getRedmineIntegrationProject({
      projectId: this.store.projectId()
    }).subscribe(setting => {
      this.setting = setting;
    });
    this.loadRedmineMetadata(false);
  }

  saveSetting() {
    this.loading = true;
    this.projectService.updateRedmineIntegrationProject({
      projectId: this.store.projectId(),
      body: this.setting
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(success => {
      this.toastr.success({
        message: 'Success!'
      });
    })
  }

  loadRedmineMetadata(reload: boolean) {
    this.loadingMetadata = true;
    this.projectService.getRedmineMetadata({
      projectId: this.store.projectId(),
      reload: reload
    }).pipe(
      finalize(() => this.loadingMetadata = false)
    ).subscribe(metadata => {
      this.metadata = metadata;
    })
  }
}
