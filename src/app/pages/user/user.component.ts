import {Component, OnDestroy, OnInit} from '@angular/core';
import {LoadingTableComponent} from "../../shared/components/ui/loading-table/loading-table.component";
import {NgIcon} from "@ng-icons/core";
import {PaginationComponent} from "../../shared/components/ui/pagination/pagination.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {TimeagoModule} from "ngx-timeago";
import {ProjectSortField} from '../../api/models/project-sort-field';
import {DropdownItem} from '../../shared/components/ui/dropdown/dropdown.model';
import {ProjectSummaryPage} from '../../api/models/project-summary-page';
import {GetProjects$Params} from '../../api/fn/project/get-projects';
import {ProjectService} from '../../api/services/project.service';
import {ActivatedRoute, Router} from '@angular/router';
import {delay, finalize, Subject, switchMap, takeUntil} from 'rxjs';
import {bindQueryParams, updateQueryParams} from '../../core/router';
import {UserSortField} from '../../api/models';
import {UserService} from '../../api/services/user.service';
import {UserStore} from './user.store';
import {AvatarComponent} from '../../shared/components/ui/avatar/avatar.component';

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
    AvatarComponent
  ],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent implements OnInit, OnDestroy{
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

  constructor(
    private userService: UserService,
    public store: UserStore,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParams.pipe(
      switchMap(params => {
        this.loading = true;
        bindQueryParams(params, this.store.filter);
        return this.userService.getUsersByAdmin({
          body: this.store.filter
        }).pipe(
          delay(300),
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
  }

  onSearchChange() {
    updateQueryParams(this.router, this.store.filter);
  }

  onOrderChange() {
    this.store.filter.desc = !this.store.filter.desc;
    updateQueryParams(this.router, this.store.filter);
  }

  onSortChange(sortBy: UserSortField) {
    this.store.filter.sortBy = sortBy;
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

}
