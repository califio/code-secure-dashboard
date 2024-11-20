import {Injectable, signal} from '@angular/core';
import {UserFilter} from '../../api/models/user-filter';
import {UserSortField} from '../../api/models/user-sort-field';
import {UserInfo} from '../../api/models/user-info';
import {RoleSummary} from '../../api/models/role-summary';
import {DropdownItem} from '../../shared/components/ui/dropdown/dropdown.model';

@Injectable({
  providedIn: 'root'
})
export class UserStore {
  loading = false;
  filter: UserFilter = {
    desc: true,
    name: '',
    page: 1,
    roleId: null,
    sortBy: UserSortField.CreatedAt,
    status: undefined,
  };
  users = signal<UserInfo[]>([]);
  currentPage = signal(1);
  totalPage = signal(1);
  count = signal(0);
  showAddUserPopup = signal(false);
  showUpdateUserPopup = signal(false);
  showDisableUserPopup = signal(false);
  roleOptions = signal<DropdownItem[]>([]);
  constructor() { }
}
