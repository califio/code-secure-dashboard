import {computed, Injectable, signal} from '@angular/core';
import {UserFilter} from '../../api/models/user-filter';
import {UserSortField} from '../../api/models/user-sort-field';
import {RoleSummary} from '../../api/models/role-summary';
import {UserDetail} from '../../api/models/user-detail';

@Injectable({
  providedIn: 'root'
})
export class UserStore {
  loading = false;
  filter: UserFilter = {
    desc: true,
    name: '',
    page: 1,
    size: 20,
    roleId: null,
    sortBy: UserSortField.CreatedAt,
    status: undefined,
  };
  users = signal<UserDetail[]>([]);
  roles = signal<RoleSummary[]>([]);
  // update user
  selectedUser = signal<UserDetail | undefined>(undefined);
  showUpdateUserDialog = false;
  // add user
  showAddUserDialog = false;
  // paginator
  currentPage = signal(1);
  pageSize = signal(20);
  totalRecords = signal(0);
  firstRecord = computed(() => {
    return (this.currentPage() - 1) * this.pageSize();
  });

  constructor() {
  }
}
