import {Component, OnDestroy, OnInit} from '@angular/core';
import {PaginationComponent} from '../../../shared/components/pagination/pagination.component';
import {NgIcon} from '@ng-icons/core';
import {FormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {LoadingTableComponent} from '../../../shared/components/loading-table/loading-table.component';
import {ProjectService} from '../../../api/services/project.service';
import {GetProjects$Params} from '../../../api/fn/project/get-projects';
import {bindQueryParams, updateQueryParams} from '../../../core/router';
import {delay, finalize, of, Subject, switchMap, takeUntil} from 'rxjs';
import {ProjectSummaryPage} from '../../../api/models/project-summary-page';
import {ProjectSortField} from '../../../api/models/project-sort-field';
import {DropdownComponent} from '../../../shared/components/dropdown/dropdown.component';
import {DropdownItem} from '../../../shared/components/dropdown/dropdown.model';

@Component({
  selector: 'app-list',
  standalone: true,
  imports: [
    PaginationComponent,
    NgIcon,
    FormsModule,
    TimeagoModule,
    RouterLink,
    LoadingTableComponent,
    DropdownComponent
  ],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class ListComponent implements OnInit, OnDestroy{
  loading = false;
  sortOptions: ProjectSortField[] = [ProjectSortField.Name, ProjectSortField.CreatedAt, ProjectSortField.UpdatedAt];
  sorts: DropdownItem[] = [
    {
      value: ProjectSortField.Name,
      label: 'Name'
    },
    {
      value: ProjectSortField.CreatedAt,
      label: 'Created'
    },
    {
      value: ProjectSortField.UpdatedAt,
      label: 'Updated'
    }
  ];
  response: ProjectSummaryPage = {
    count: 0,
    currentPage: 1,
    pageCount: 0,
    items: [],
    size: 0,
  };
  filter: GetProjects$Params = {
    Name: '',
    SortBy: ProjectSortField.CreatedAt,
    Size: 20,
    Page: 1,
    Desc: true
  }
  constructor(
    private projectService: ProjectService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParams.pipe(
      switchMap(params => {
        this.loading = true;
        bindQueryParams(params, this.filter);
        return this.projectService.getProjects(this.filter).pipe(
          delay(300),
          finalize(() => {
            this.loading = false;
          }),
        );
      }),
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.response = response;
    })
  }

  onSearchChange() {
    updateQueryParams(this.router, this.filter);
  }

  onOrderChange() {
    this.filter.Desc = !this.filter.Desc;
    updateQueryParams(this.router, this.filter);
  }

  onSortChange(sortBy: ProjectSortField) {
    this.filter.SortBy = sortBy;
    updateQueryParams(this.router, this.filter);
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  onChangePage($event: number) {
    this.filter.Page = $event;
    updateQueryParams(this.router, this.filter);
  }

  private destroy$ = new Subject();

}
