import {Component, OnDestroy, OnInit, signal} from '@angular/core';
import {UserSummary} from '../../../../../../api/models/user-summary';
import {ProjectRole} from '../../../../../../api/models/project-role';
import {MemberStore} from '../member.store';
import {ProjectService} from '../../../../../../api/services/project.service';
import {ProjectStore} from '../../../project.store';
import {ToastrService} from '../../../../../../shared/services/toastr.service';
import {BehaviorSubject, debounceTime, finalize, Subject, takeUntil} from 'rxjs';
import {Button} from 'primeng/button';
import {Dialog} from 'primeng/dialog';
import {SelectButton} from 'primeng/selectbutton';
import {UserInfoComponent} from '../../../../../../shared/components/user-info/user-info.component';
import {FormsModule} from '@angular/forms';
import {Select, SelectFilterEvent} from 'primeng/select';
import {UserService} from '../../../../../../api/services/user.service';
import {switchMap} from 'rxjs/operators';

@Component({
  selector: 'app-add-member-dialog',
  standalone: true,
  imports: [
    Button,
    Dialog,
    SelectButton,
    UserInfoComponent,
    FormsModule,
    Select,
  ],
  templateUrl: './add-member-dialog.component.html',
})
export class AddMemberDialogComponent implements OnInit, OnDestroy {
  userId: string = '';
  loading = false;
  // role
  role: ProjectRole = ProjectRole.Developer;
  roleOptions = [
    {
      label: 'Developer',
      value: ProjectRole.Developer
    },
    {
      label: 'Validator',
      value: ProjectRole.Validator
    },
    {
      label: 'Manager',
      value: ProjectRole.Manager
    }
  ];
  // users
  search$ = new BehaviorSubject<string>('');
  loadingUsers = true;
  users = signal<UserSummary[]>([]);
  private destroy$ = new Subject();

  constructor(
    public store: MemberStore,
    private projectStore: ProjectStore,
    private projectService: ProjectService,
    private userService: UserService,
    private toastr: ToastrService
  ) {
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.search$.pipe(
      debounceTime(100),
      takeUntil(this.destroy$),
      switchMap(search => {
        return this.getUsers(search);
      })
    ).subscribe(response => {
      this.users.set(response.items!);
    });
    this.getUsers(null).subscribe(response => {
      this.users.set(response.items!);
    })
  }

  onCancel() {
    this.store.showAddMemberDialog.set(false);
  }

  onAddUser() {
    this.loading = true;
    this.projectService.addMember({
      projectId: this.projectStore.projectId(),
      body: {
        userId: this.userId,
        role: this.role
      }
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(user => {
      const members = this.store.members()
      members.push(user);
      this.store.members.set(members);
      this.toastr.success({message: 'Add member success!'});
      this.store.showAddMemberDialog.set(false);
    })
  }

  getUsers(search: string | null | undefined) {
    this.loadingUsers = true;
    return this.userService.queryUserSummary({
      body: {
        name: search
      }
    }).pipe(
      finalize(() => this.loadingUsers = false)
    );
  }

  onSearchUsers($event: SelectFilterEvent) {
    this.search$.next($event.filter);
  }
}
