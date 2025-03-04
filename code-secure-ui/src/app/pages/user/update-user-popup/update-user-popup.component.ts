import {Component, Input} from '@angular/core';
import {UserInfo} from '../../../api/models/user-info';
import {UserStore} from '../user.store';
import {UpdateUserRequest} from '../../../api/models/update-user-request';
import {UserService} from '../../../api/services/user.service';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../shared/services/toastr.service';
import {UserStatus} from '../../../api/models';
import {FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../core/forms';
import {Select} from 'primeng/select';
import {Button} from 'primeng/button';
import {Dialog} from 'primeng/dialog';
import {InputText} from 'primeng/inputtext';
import {SelectButton} from 'primeng/selectbutton';
import {ToggleSwitch} from 'primeng/toggleswitch';

@Component({
  selector: 'app-update-user-popup',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    Select,
    Button,
    Dialog,
    InputText,
    SelectButton,
    ToggleSwitch,
  ],
  templateUrl: './update-user-popup.component.html',
  styleUrl: './update-user-popup.component.scss'
})
export class UpdateUserPopupComponent {
  @Input() set user(value: UserInfo | undefined) {
    if (value) {
      this._user = value;
      this.form.patchValue(value);
    }
  }

  _user: UserInfo = {};

  formConfig = new FormSection<ConfigOf<UpdateUserRequest>>({
    status: new FormField(UserStatus.Active, Validators.required),
    email: new FormField('', Validators.required),
    role: new FormField('', Validators.required),
    fullName: new FormField('', Validators.required),
    verified: new FormField(true)
  });
  form: FormGroup<ControlsOf<UpdateUserRequest>>;
  statusOptions = [
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

  onUpdateUser() {
    this.form.disable()
    this.userService.updateUserByAdmin({
      userId: this._user.id!,
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
      this.toastr.success({
        message: 'Update user success!'
      });
      this.closeDialog();
    })
  }

  closeDialog() {
    this.store.showUpdateUserDialog = false;
  }
}
