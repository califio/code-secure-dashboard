import {Component, effect, input} from '@angular/core';
import {UserStore} from '../user.store';
import {UpdateUserRequest} from '../../../api/models/update-user-request';
import {UserService} from '../../../api/services/user.service';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../shared/services/toastr.service';
import {UserDetail, UserStatus} from '../../../api/models';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
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
    Button,
    Dialog,
    InputText,
    SelectButton,
    ToggleSwitch,
  ],
  templateUrl: './update-user-popup.component.html',
})
export class UpdateUserPopupComponent {
  user = input<UserDetail>();
  userId: string = '';
  body: UpdateUserRequest = {
    email: undefined, fullName: undefined, role: undefined, status: undefined, verified: undefined
  };
  loading = false;
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
  ) {
    effect(() => {
      const user = this.user();
      if (user) {
        this.userId = user.id!;
        this.body = {
          email: user.email, fullName: user.fullName, role: user.role, status: user.status, verified: user.verified
        }
      }
    });
  }

  onUpdateUser() {
    this.loading = true;
    this.userService.updateUser({
      userId: this.userId,
      body: this.body
    }).pipe(
      finalize(() => this.loading = false)
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
