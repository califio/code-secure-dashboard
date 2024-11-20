import {Component, Input} from '@angular/core';
import {AvatarComponent} from '../../../../../../shared/components/ui/avatar/avatar.component';
import {ProjectUser} from '../../../../../../api/models/project-user';
import {DropdownComponent} from '../../../../../../shared/components/ui/dropdown/dropdown.component';
import {DropdownItem} from '../../../../../../shared/components/ui/dropdown/dropdown.model';
import {ProjectRole} from '../../../../../../api/models';
import {NgButtonComponent} from '../../../../../../shared/components/ui/ng-button/ng-button.component';
import {ButtonDirective} from '../../../../../../shared/directives/button.directive';
import {ProjectService} from '../../../../../../api/services/project.service';
import {ProjectStore} from '../../../project-store';
import {MemberStore} from '../member.store';
import {ToastrService} from '../../../../../../shared/components/toastr/toastr.service';
import {finalize} from 'rxjs';
import {UserInfoComponent} from "../../../../../../shared/components/user-info/user-info.component";

@Component({
  selector: 'app-update-member-popup',
  standalone: true,
    imports: [
        AvatarComponent,
        DropdownComponent,
        NgButtonComponent,
        ButtonDirective,
        UserInfoComponent
    ],
  templateUrl: './update-member-popup.component.html',
  styleUrl: './update-member-popup.component.scss'
})
export class UpdateMemberPopupComponent {
  @Input()
  user: ProjectUser = {};
  roles: DropdownItem[] = [
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
    public memberStore: MemberStore,
    private toastr: ToastrService
  ) {
  }

  changeRoleUser(role: any) {
    this.user.role = role;
  }

  onUpdateRole() {
    this.loading = true;
    this.projectService.updateProjectMember({
      slug: this.projectStore.slug(),
      userId: this.user.userId ?? '',
      body: {
        role: this.user.role
      }
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(response => {
      const members = this.memberStore.members().map(value => {
        if (value.userId != response.userId) {
          return value;
        }
        return response;
      });
      this.memberStore.members.set(members);
      this.toastr.success('Update member success!');
      this.memberStore.showUpdateMemberPopup.set(false);
    })
  }

  onCancel() {
    this.memberStore.showUpdateMemberPopup.set(false);
  }
}
