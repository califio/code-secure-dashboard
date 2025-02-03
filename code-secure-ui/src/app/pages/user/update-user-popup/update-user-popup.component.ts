import {Component, Input} from '@angular/core';
import {UserInfo} from '../../../api/models/user-info';
import {AvatarComponent} from '../../../shared/ui/avatar/avatar.component';
import {UserStore} from '../user.store';
import {UserInfoComponent} from '../../../shared/components/user-info/user-info.component';
import {DropdownComponent} from '../../../shared/ui/dropdown/dropdown.component';
import {DropdownItem} from '../../../shared/ui/dropdown/dropdown.model';
import {UpdateUserRequest} from '../../../api/models/update-user-request';
import {ButtonDirective} from '../../../shared/ui/button/button.directive';
import {UserService} from '../../../api/services/user.service';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../shared/components/toastr/toastr.service';
import {UserStatus} from '../../../api/models';
import {FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../core/forms';

@Component({
  selector: 'app-update-user-popup',
  standalone: true,
  imports: [
    AvatarComponent,
    UserInfoComponent,
    DropdownComponent,
    ButtonDirective,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './update-user-popup.component.html',
  styleUrl: './update-user-popup.component.scss'
})
export class UpdateUserPopupComponent {
  _user: UserInfo | undefined;
  @Input() set user(value: UserInfo | undefined) {
    if (value) {
      this._user = value;
      this.form.patchValue(value);
    }
  }
  formConfig = new FormSection<ConfigOf<UpdateUserRequest>>({
    status: new FormField(UserStatus.Active, Validators.required),
    email: new FormField('', Validators.required),
    role: new FormField('', Validators.required),
    fullName: new FormField('', Validators.required),
  });
  form: FormGroup<ControlsOf<UpdateUserRequest>>;
  statusOptions: DropdownItem[] = [
    {
      value: UserStatus.Active,
      label: 'Active'
    },
    {
      value: UserStatus.Disabled,
      label: 'Disabled'
    },
  ];

  constructor(
    public store: UserStore,
    private userService: UserService,
    private toastr: ToastrService,
    private formService: FormService,
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  changeRoleUser($event: any) {
    this.form.controls.role!.setValue($event)
  }

  onUpdateUser() {
    this.form.disable()
    this.userService.updateUserByAdmin({
      userId: this._user!.id!,
      body: this.form.value
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(user => {
      const users = this.store.users().map(value => {
        if (value.id != user.id) {
          return value;
        }
        return user;
      });
      this.store.users.set(users);
      this.toastr.success('Update user success!');
      this.store.showUpdateUserPopup.set(false);
    })
  }

  onCancel() {
    this.store.showUpdateUserPopup.set(false);
  }

  changeStatusUser($event: any) {
    this.form.controls.status!.setValue($event)
  }
}
