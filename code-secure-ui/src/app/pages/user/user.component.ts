import {Component, OnDestroy, OnInit} from '@angular/core';
import {LoadingTableComponent} from "../../shared/ui/loading-table/loading-table.component";
import {NgIcon, provideIcons} from "@ng-icons/core";
import {PaginationComponent} from "../../shared/ui/pagination/pagination.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {TimeagoModule} from "ngx-timeago";
import {DropdownItem} from '../../shared/ui/dropdown/dropdown.model';
import {ActivatedRoute, Router} from '@angular/router';
import {finalize, Subject, switchMap, takeUntil} from 'rxjs';
import {bindQueryParams, updateQueryParams} from '../../core/router';
import {UserInfo, UserSortField, UserStatus} from '../../api/models';
import {UserService} from '../../api/services/user.service';
import {UserStore} from './user.store';
import {AvatarComponent} from '../../shared/ui/avatar/avatar.component';
import {UserInfoComponent} from '../../shared/components/user-info/user-info.component';
import {DropdownComponent} from '../../shared/ui/dropdown/dropdown.component';
import {ButtonDirective} from '../../shared/ui/button/button.directive';
import {AddUserPopupComponent} from './add-user-popup/add-user-popup.component';
import {UpdateUserPopupComponent} from './update-user-popup/update-user-popup.component';
import {RoleService} from '../../api/services/role.service';
import {ConfirmPopupComponent} from '../../shared/ui/confirm-popup/confirm-popup.component';
import {ToastrService} from '../../shared/components/toastr/toastr.service';
import {TooltipDirective} from '../../shared/ui/tooltip/tooltip.directive';
import {heroPaperAirplane} from '@ng-icons/heroicons/outline';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [
    LoadingTableComponent,
    NgIcon,
    PaginationComponent,
    ReactiveFormsModule,
    TimeagoModule,
    FormsModule,
    AvatarComponent,
    UserInfoComponent,
    DropdownComponent,
    ButtonDirective,
    AddUserPopupComponent,
    UpdateUserPopupComponent,
    ConfirmPopupComponent,
    TooltipDirective
  ],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss',
  viewProviders: [provideIcons({heroPaperAirplane})]
})
export class UserComponent implements OnInit, OnDestroy {
  loading = false;
  sortOptions: UserSortField[] = [UserSortField.Status, UserSortField.CreatedAt, UserSortField.UpdatedAt];
  sorts: DropdownItem[] = [
    {
      value: UserSortField.CreatedAt,
      label: 'Created'
    },
    {
      value: UserSortField.UpdatedAt,
      label: 'Updated'
    },
    {
      value: UserSortField.Status,
      label: 'Status'
    }
  ];
  user: UserInfo | undefined = undefined;

  constructor(
    private userService: UserService,
    public store: UserStore,
    private router: Router,
    private route: ActivatedRoute,
    private roleService: RoleService,
    private toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.route.queryParams.pipe(
      switchMap(params => {
        this.loading = true;
        bindQueryParams(params, this.store.filter);
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
      this.store.currentPage.set(response.currentPage!);
      this.store.totalPage.set(response.pageCount!);
      this.store.count.set(response.count!);
    })
    this.roleService.getRoles().subscribe(roles => {
      const options = roles.map(value => <DropdownItem>{
        value: value.name,
        label: value.name
      });
      this.store.roleOptions.set(options);
    })
  }

  onSearchChange() {
    updateQueryParams(this.router, this.store.filter);
  }

  onOrderChange() {
    this.store.filter.desc = !this.store.filter.desc;
    updateQueryParams(this.router, this.store.filter);
  }

  onSortChange(value: any) {
    this.store.filter.sortBy = value;
    updateQueryParams(this.router, this.store.filter);
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  onChangePage($event: number) {
    this.store.filter.page = $event;
    updateQueryParams(this.router, this.store.filter);
  }

  private destroy$ = new Subject();

  addUser() {
    this.store.showAddUserPopup.set(true);
  }

  showConfirmDisableUserPopup(user: UserInfo) {
    this.user = user;
    this.store.showDisableUserPopup.set(true);
  }

  showUpdateUserPopup(user: UserInfo) {
    this.user = user;
    this.store.showUpdateUserPopup.set(true);
  }

  disableUser() {
    if (this.user) {
      this.userService.updateUserByAdmin({
        userId: this.user.id!,
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
        this.store.showDisableUserPopup.set(false);
        this.toastr.success('Disabled user success!');
      })
    }
  }

  showSendEmailConfirmPopup(user: UserInfo) {
    this.user = user;
    this.store.showSendEmailConfirmPopup.set(true);
  }

  sendEmailConfirmUser() {
    if (this.user) {
      this.userService.sendConfirmEmail({
        userId: this.user.id!,
      }).pipe(
        finalize(() => this.store.showSendEmailConfirmPopup.set(false))
      ).subscribe(() => {
        this.toastr.success('Send confirm email success!');
      })
    }
  }
}
