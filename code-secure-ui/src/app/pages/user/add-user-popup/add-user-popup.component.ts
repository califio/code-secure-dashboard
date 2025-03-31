import {Component} from '@angular/core';
import {UserStore} from '../user.store';
import {UserService} from '../../../api/services/user.service';
import {CreateUserRequest} from '../../../api/models/create-user-request';
import {FormsModule} from '@angular/forms';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../shared/services/toastr.service';
import {Button} from 'primeng/button';
import {Dialog} from "primeng/dialog";
import {InputText} from 'primeng/inputtext';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {SelectButton} from "primeng/selectbutton";

@Component({
  selector: 'app-add-user-popup',
  standalone: true,
  imports: [
    FormsModule,
    Button,
    Dialog,
    InputText,
    ToggleSwitch,
    SelectButton,
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
    this.userService.createUser({
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
