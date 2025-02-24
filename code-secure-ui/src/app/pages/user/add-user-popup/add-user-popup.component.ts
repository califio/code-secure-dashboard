import {Component} from '@angular/core';
import {UserStore} from '../user.store';
import {UserService} from '../../../api/services/user.service';
import {CreateUserRequest} from '../../../api/models/create-user-request';
import {FormsModule} from '@angular/forms';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../shared/services/toastr.service';
import {Select} from 'primeng/select';
import {Button} from 'primeng/button';
import {Dialog} from "primeng/dialog";
import {InputText} from 'primeng/inputtext';
import {ToggleSwitch} from 'primeng/toggleswitch';

@Component({
  selector: 'app-add-user-popup',
  standalone: true,
  imports: [
    FormsModule,
    Select,
    Button,
    Dialog,
    InputText,
    ToggleSwitch,
  ],
  templateUrl: './add-user-popup.component.html',
})
export class AddUserPopupComponent {
  loading = false;
  request: CreateUserRequest = {
    role: 'User', email: ''
  }

  constructor(
    public store: UserStore,
    private userService: UserService,
    private toastr: ToastrService
  ) {
  }


  closeDialog() {
    this.store.showAddUserDialog = false;
    this.resetBody();
  }

  onAddUser() {
    this.loading = true;
    this.userService.createUserByAdmin({
      body: this.request
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(user => {
      const users = this.store.users();
      users.push(user);
      this.store.users.set(users);
      this.toastr.success({
        message: 'Add user success!'
      });
      this.closeDialog();
    })
  }

  private resetBody() {
    this.request = {
      role: 'User', email: ''
    };
  }
}
