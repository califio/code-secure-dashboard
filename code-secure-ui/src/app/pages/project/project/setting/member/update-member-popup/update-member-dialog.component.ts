import {Component} from '@angular/core';
import {ProjectRole} from '../../../../../../api/models';
import {ProjectService} from '../../../../../../api/services/project.service';
import {MemberStore} from '../member.store';
import {ToastrService} from '../../../../../../shared/services/toastr.service';
import {finalize} from 'rxjs';
import {UserInfoComponent} from "../../../../../../shared/components/user-info/user-info.component";
import {ProjectStore} from '../../../project.store';
import {Button} from 'primeng/button';
import {Dialog} from 'primeng/dialog';
import {FormsModule} from '@angular/forms';
import {SelectButton} from 'primeng/selectbutton';

@Component({
  selector: 'app-update-member-dialog',
  standalone: true,
  imports: [
    UserInfoComponent,
    Button,
    Dialog,
    FormsModule,
    SelectButton,
  ],
  templateUrl: './update-member-dialog.component.html',
})
export class UpdateMemberDialogComponent {
  role = ProjectRole.Developer;
  roles = [
    {
      label: 'Developer',
      value: ProjectRole.Developer
    },
    {
      label: 'Validator',
      value: ProjectRole.Validator
    },
    {
      label: 'Manager',
      value: ProjectRole.Manager
    }
  ];
  loading = false;

  constructor(
    private projectStore: ProjectStore,
    private projectService: ProjectService,
    public store: MemberStore,
    private toastr: ToastrService
  ) {
  }

  changeRoleUser(role: any) {
    this.role = role;
  }

  onUpdateRole() {
    this.loading = true;
    this.projectService.updateProjectMember({
      projectId: this.projectStore.projectId(),
      userId: this.store.member().userId ?? '',
      body: {
        role: this.role
      }
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(response => {
      const members = this.store.members().map(value => {
        if (value.userId != response.userId) {
          return value;
        }
        return response;
      });
      this.store.members.set(members);
      this.toastr.success({message: 'Update member success!'});
      this.store.showUpdateMemberDialog.set(false);
    })
  }

  onCancel() {
    this.store.showUpdateMemberDialog.set(false);
  }
}
