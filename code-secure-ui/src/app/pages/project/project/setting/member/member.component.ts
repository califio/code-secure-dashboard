import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {NgIcon} from '@ng-icons/core';
import {TimeagoModule} from 'ngx-timeago';
import {MemberStore} from './member.store';
import {bindQueryParams, updateQueryParams} from '../../../../../core/router';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectStore} from '../../project.store';
import {ProjectService} from '../../../../../api/services/project.service';
import {finalize, Subject, switchMap, takeUntil, tap} from 'rxjs';
import {ToastrService} from '../../../../../shared/services/toastr.service';
import {UpdateMemberDialogComponent} from './update-member-popup/update-member-dialog.component';
import {AddMemberDialogComponent} from './add-member-popup/add-member-dialog.component';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {Button} from 'primeng/button';
import {TableModule} from 'primeng/table';
import {Checkbox} from 'primeng/checkbox';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {Tooltip} from 'primeng/tooltip';
import {LayoutService} from '../../../../../layout/layout.service';
import {UserInfoComponent} from '../../../../../shared/components/user-info/user-info.component';
import {ConfirmationService} from 'primeng/api';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {Chip} from 'primeng/chip';
import {Panel} from 'primeng/panel';
import {ProjectMemberFilter} from '../../../../../api/models/project-member-filter';
import {ProjectMember} from '../../../../../api/models/project-member';

@Component({
  selector: 'app-member',
  standalone: true,
  imports: [
    FormsModule,
    NgIcon,
    TimeagoModule,
    UpdateMemberDialogComponent,
    AddMemberDialogComponent,
    IconField,
    InputIcon,
    InputText,
    Button,
    TableModule,
    Checkbox,
    Paginator,
    Tooltip,
    UserInfoComponent,
    ConfirmDialog,
    Chip,
    Panel
  ],
  templateUrl: './member.component.html',
  providers: [ConfirmationService]
})
export class MemberComponent implements OnInit, OnDestroy {
  isDesktop = true;
  filter: ProjectMemberFilter = {
    desc: true,
    name: '',
    page: 1,
    role: undefined,
    size: 20
  };
  private destroy$ = new Subject();

  constructor(
    public store: MemberStore,
    private projectStore: ProjectStore,
    private projectService: ProjectService,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private confirmationService: ConfirmationService,
    private layoutService: LayoutService
  ) {
    this.isDesktop = this.layoutService.isDesktop();
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.route.queryParams.pipe(
      switchMap(params => {
        bindQueryParams(params, this.filter);
        return this.getProjectUsers();
      }),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  onSearchChange() {
    updateQueryParams(this.router, this.filter);
  }

  private getProjectUsers() {
    this.store.loading.set(true)
    return this.projectService.getProjectUsers({
      projectId: this.projectStore.projectId(),
      body: this.filter
    }).pipe(
      finalize(() => this.store.loading.set(false)),
      tap(response => {
        this.store.members.set(response.items!);
        this.store.totalRecords.set(response.count!);
        this.store.currentPage.set(response.currentPage!);
      })
    )
  }

  showConfirmDeleteMemberDialog(member: ProjectMember) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      rejectButtonProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptButtonProps: {
        label: 'Delete',
        severity: 'danger',
      },
      accept: () => {
        this.projectService.deleteProjectMember({
          projectId: this.projectStore.projectId(),
          userId: member.userId!
        }).subscribe(() => {
          const members = this.store.members().filter(value => value.userId != member.userId);
          this.store.members.set(members);
          this.toastr.success({
            message: `Delete ${member.userName} success!`
          });
        })
      }
    });
  }

  showUpdateMemberPopup(member: ProjectMember) {
    this.store.member.set(member);
    this.store.showUpdateMemberDialog.set(true);
  }

  showAddMemberPopup() {
    this.store.showAddMemberDialog.set(true);
  }

  onPageChange($event: PaginatorState) {
    this.filter.page = $event.page! + 1;
    this.filter.size = $event.rows;
    updateQueryParams(this.router, this.filter);
  }
}
