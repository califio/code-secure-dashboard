import {Injectable, signal} from '@angular/core';
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
  currentPage = signal(1);
  totalPage = signal(1);
  count = signal(0);
  //
  showUpdateMemberPopup = signal(false);
  showAddMemberPopup = signal(false);
  showConfirmDeleteMemberPopup = signal(false);
  constructor() { }
}
