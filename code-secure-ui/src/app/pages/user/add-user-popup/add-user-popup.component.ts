import {Component, OnInit} from '@angular/core';
import {UserStore} from '../user.store';
import {ButtonDirective} from '../../../shared/ui/button/button.directive';
import {DropdownComponent} from '../../../shared/ui/dropdown/dropdown.component';
import {DropdownItem} from '../../../shared/ui/dropdown/dropdown.model';
import {UserService} from '../../../api/services/user.service';
import {CreateUserRequest} from '../../../api/models/create-user-request';
import {RoleService} from '../../../api/services/role.service';
import {FormsModule} from '@angular/forms';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../shared/components/toastr/toastr.service';

@Component({
  selector: 'app-add-user-popup',
  standalone: true,
  imports: [
    ButtonDirective,
    DropdownComponent,
    FormsModule
  ],
  templateUrl: './add-user-popup.component.html',
  styleUrl: './add-user-popup.component.scss'
})
export class AddUserPopupComponent implements OnInit {
  loading = false;
  roleOptions: DropdownItem[] = [];
  request: CreateUserRequest = {
    role: '', email: ''
  }
  constructor(
    public userStore: UserStore,
    private userService: UserService,
    private roleService: RoleService,
    private toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.roleService.getRoles().subscribe(roles => {
      this.roleOptions = this.roleOptions = roles.map(role => <DropdownItem>{
        value: role.name,
        label: role.name
      });
      if (roles.length > 0) {
        this.request.role = roles[0].name!;
      }
    })
  }

  onCancel() {
    this.userStore.showAddUserPopup.set(false);
    this.resetBody();
  }

  onAddUser() {
    this.loading = true;
    this.userService.createUserByAdmin({
      body: this.request
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(user => {
      const  users = this.userStore.users();
      users.push(user);
      this.userStore.users.set(users);
      this.toastr.success('Add user success!');
      this.userStore.showAddUserPopup.set(false);
      this.resetBody();
    })
  }

  onRoleChange(role: any) {
    this.request.role = role;
  }

  private resetBody() {
    this.request = {
      role: '', email: ''
    };
  }
}
