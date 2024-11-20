import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {ClickOutsideDirective} from '../../directives/click-outside.directive';
import {UserSummary} from '../../../api/models/user-summary';
import {UserInfoComponent} from '../user-info/user-info.component';
import {NgClass} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {UserService} from '../../../api/services/user.service';
import {finalize} from 'rxjs';

@Component({
  selector: 'user-dropdown',
  standalone: true,
  imports: [
    NgIcon,
    ClickOutsideDirective,
    UserInfoComponent,
    NgClass,
    FormsModule
  ],
  templateUrl: './user-dropdown.component.html',
  styleUrl: './user-dropdown.component.scss'
})
export class UserDropdownComponent {
  name = '';
  hidden = true;
  options: UserSummary[] = [];
  selectedOption: UserSummary | undefined = undefined;
  loading = false;
  constructor(
    private userService: UserService
  ) {
    this.loading = true;
    this.userService.getUsers({
      body: {}
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(response => {
      this.options = response.items!;
    })
  }
  @Input()
  set selected(userId: string | null | undefined) {
    this.selectedOption = this.options.find(option => option.id == userId);
  }

  @Output()
  selectChange = new EventEmitter<UserSummary>();

  onClick(option: UserSummary) {
    this.selectedOption = option;
    this.selectChange.emit(option);
    this.hidden = true;
  }

  onSearch() {
    this.loading = true;
    this.userService.getUsers({
      body: {
        name: this.name
      }
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(response => {
      this.options = response.items!;
    })
  }
}
