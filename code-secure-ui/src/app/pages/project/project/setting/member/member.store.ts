import {computed, Injectable, signal} from '@angular/core';
import {ProjectMember} from '../../../../../api/models/project-member';

@Injectable({
  providedIn: 'root'
})
export class MemberStore {
  loading = signal(false);
  members = signal<ProjectMember[]>([]);
  // update member
  member = signal<ProjectMember>({});
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
