import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {TimeagoModule} from "ngx-timeago";
import {ActivatedRoute, Router} from '@angular/router';
import {finalize, Subject, switchMap, takeUntil} from 'rxjs';
import {bindQueryParams, updateQueryParams} from '../../core/router';
import {UserInfo, UserSortField, UserStatus} from '../../api/models';
import {UserService} from '../../api/services/user.service';
import {UserStore} from './user.store';
import {UserInfoComponent} from '../../shared/components/user-info/user-info.component';
import {AddUserPopupComponent} from './add-user-popup/add-user-popup.component';
import {UpdateUserPopupComponent} from './update-user-popup/update-user-popup.component';
import {RoleService} from '../../api/services/role.service';
import {ToastrService} from '../../shared/services/toastr.service';
import {ButtonDirective} from 'primeng/button';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {ConfirmationService} from 'primeng/api';
import {Checkbox} from 'primeng/checkbox';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {Panel} from 'primeng/panel';
import {TableModule} from 'primeng/table';
import {LayoutService} from '../../layout/layout.service';
import {Chip} from 'primeng/chip';
import {Tooltip} from 'primeng/tooltip';
import {SortByComponent} from '../../shared/ui/sort-by/sort-by.component';
import {SortByState} from '../../shared/ui/sort-by/sort-by-state';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TimeagoModule,
    FormsModule,
    UserInfoComponent,
    AddUserPopupComponent,
    UpdateUserPopupComponent,
    ButtonDirective,
    ConfirmDialog,
    Checkbox,
    IconField,
    InputIcon,
    InputText,
    Paginator,
    Panel,
    TableModule,
    Chip,
    Tooltip,
    SortByComponent,
  ],
  templateUrl: './user.component.html',
  providers: [ConfirmationService]
})
export class UserComponent implements OnInit, OnDestroy {
  loading = false;
  sortOptions = [
    {
      value: UserSortField.CreatedAt,
      label: 'created'
    },
    {
      value: UserSortField.UpdatedAt,
      label: 'updated'
    },
    {
      value: UserSortField.Status,
      label: 'status'
    }
  ];
  user: UserInfo = {};
  isDesktop = true;
  private destroy$ = new Subject();

  constructor(
    private userService: UserService,
    public store: UserStore,
    private router: Router,
    private route: ActivatedRoute,
    private roleService: RoleService,
    private toastr: ToastrService,
    private layoutService: LayoutService,
    private confirmationService: ConfirmationService
  ) {
    this.isDesktop = this.layoutService.isDesktop();
  }

  ngOnInit(): void {
    this.route.queryParams.pipe(
      switchMap(params => {
        this.loading = true;
        bindQueryParams(params, this.store.filter);
        this.store.currentPage.set(this.store.filter.page!);
        this.store.pageSize.set(this.store.filter.size!);
        return this.userService.getUsersByAdmin({
          body: this.store.filter
        }).pipe(
          finalize(() => {
            this.loading = false;
          }),
        );
      }),
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.store.users.set(response.items!);
      this.store.totalRecords.set(response.count!);
    });
    this.roleService.getRoles().subscribe(roles => {
      this.store.roles.set(roles);
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  onSearchChange() {
    updateQueryParams(this.router, this.store.filter);
  }

  onSortChange($event: SortByState) {
    this.store.filter.sortBy = $event.sortBy;
    this.store.filter.desc = $event.desc;
    updateQueryParams(this.router, this.store.filter);
  }

  onPageChange($event: PaginatorState) {
    this.store.filter.size = $event.rows;
    this.store.filter.page = $event.page! + 1;
    updateQueryParams(this.router, this.store.filter);
  }

  onShowAddUserDialog() {
    this.store.showAddUserDialog = true;
  }

  onShowUpdateUserDialog(user: UserInfo) {
    this.store.selectedUser.set(user);
    this.store.showUpdateUserDialog = true;
  }

  onConfirmDisableUser(user: UserInfo) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to disable this user?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      rejectButtonProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptButtonProps: {
        label: 'Disable',
        severity: 'danger',
      },
      accept: () => {
        this.userService.updateUserByAdmin({
          userId: user.id!,
          body: {
            status: UserStatus.Disabled
          }
        }).subscribe(user => {
          const users = this.store.users().map(value => {
            if (value.id == user.id) {
              return user;
            }
            return value;
          });
          this.store.users.set(users);
          this.toastr.success({
            message: `Disabled user ${user.userName}!`
          });
        })
      }
    });
  }

  onSendEmailConfirmDialog(user: UserInfo) {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to send confirm email to this user?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      rejectButtonProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptButtonProps: {
        label: 'Send',
        severity: 'primary',
      },
      accept: () => {
        this.userService.sendConfirmEmail({
          userId: user.id!,
        }).subscribe(() => {
          this.toastr.success({message: 'Send confirm email success!'});
        })
      }
    });
  }

  protected readonly UserStatus = UserStatus;
}
