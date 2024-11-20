import {Component, Input} from '@angular/core';
import {AvatarComponent} from "../../../../../../shared/components/ui/avatar/avatar.component";
import {ButtonDirective} from "../../../../../../shared/directives/button.directive";
import {DropdownComponent} from "../../../../../../shared/components/ui/dropdown/dropdown.component";
import {UserDropdownComponent} from '../../../../../../shared/components/user-dropdown/user-dropdown.component';
import {UserSummary} from '../../../../../../api/models/user-summary';
import {DropdownItem} from '../../../../../../shared/components/ui/dropdown/dropdown.model';
import {ProjectRole} from '../../../../../../api/models/project-role';
import {MemberStore} from '../member.store';
import {ProjectService} from '../../../../../../api/services/project.service';
import {ProjectStore} from '../../../project-store';
import {ToastrService} from '../../../../../../shared/components/toastr/toastr.service';
import {finalize} from 'rxjs';

@Component({
  selector: 'app-add-member-popup',
  standalone: true,
  imports: [
    AvatarComponent,
    ButtonDirective,
    DropdownComponent,
    UserDropdownComponent
  ],
  templateUrl: './add-member-popup.component.html',
  styleUrl: './add-member-popup.component.scss'
})
export class AddMemberPopupComponent {
  @Input()
  user: UserSummary | undefined = undefined;
  loading = false;
  role: ProjectRole = ProjectRole.Developer;
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
  constructor(
    public memberStore: MemberStore,
    private projectStore: ProjectStore,
    private projectService: ProjectService,
    private toastr: ToastrService
  ) {
  }
  onSelectUser(user: UserSummary) {
    this.user = user;
  }

  onSelectRole(role: any) {
    this.role = role;
  }

  onCancel() {
    this.memberStore.showAddMemberPopup.set(false);
  }

  onAddUser() {
    this.loading = true;
    this.projectService.addMember({
      slug: this.projectStore.slug(),
      body: {
        userId: this.user?.id!,
        role: this.role
      }
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(user => {
      const members = this.memberStore.members()
      members.push(user);
      this.memberStore.members.set(members);
      this.toastr.success('Add member success!');
      this.memberStore.showAddMemberPopup.set(false);
      this.user = undefined;
    })
  }
}
