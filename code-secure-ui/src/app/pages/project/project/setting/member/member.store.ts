import {computed, Injectable, signal} from '@angular/core';
import {ProjectUserFilter} from '../../../../../api/models/project-user-filter';
import {ProjectUser} from '../../../../../api/models/project-user';

@Injectable({
  providedIn: 'root'
})
export class MemberStore {
  filter: ProjectUserFilter = {
    desc: true,
    name: '',
    page: 1,
    role: undefined,
    size: 20
  };
  loading = signal(false);
  members = signal<ProjectUser[]>([]);
  // update member
  member = signal<ProjectUser>({});
  showUpdateMemberDialog = signal(false);
  showAddMemberDialog = signal(false);
  //paginator
  currentPage = signal(1);
  pageSize = signal(20);
  totalRecords = signal(0);
  firstRecord = computed(() => {
    return (this.currentPage() - 1) * this.pageSize();
  });

  constructor() {
  }
}
