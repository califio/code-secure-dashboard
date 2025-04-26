import {Component, computed, OnDestroy, OnInit, signal} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {FormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {ProjectService} from '../../../api/services/project.service';
import {bindQueryParams, updateQueryParams} from '../../../core/router';
import {finalize, Subject, switchMap, takeUntil} from 'rxjs';
import {ProjectSortField} from '../../../api/models/project-sort-field';
import {SourceType} from '../../../api/models/source-type';
import {Panel} from 'primeng/panel';
import {TableModule} from 'primeng/table';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {Tooltip} from 'primeng/tooltip';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {Checkbox} from 'primeng/checkbox';
import {LayoutService} from '../../../layout/layout.service';
import {ProjectSummary} from '../../../api/models/project-summary';
import {SortByComponent} from '../../../shared/ui/sort-by/sort-by.component';
import {SortByState} from '../../../shared/ui/sort-by/sort-by-state';
import {FloatLabel} from "primeng/floatlabel";
import {Select} from "primeng/select";
import {UserSummary} from '../../../api/models/user-summary';
import {ProjectFilter} from '../../../api/models/project-filter';
import {UserService} from '../../../api/services/user.service';
import {
  SourceControlSelectComponent
} from '../../../shared/components/source-control-select/source-control-select.component';
import {SourceControlService} from '../../../api/services/source-control.service';
import {SourceControlSummary} from '../../../api/models/source-control-summary';
import {FindingStatus} from '../../../api/models/finding-status';
import {cast} from '../../../core/transform';

@Component({
  selector: 'page-list-project',
  standalone: true,
  imports: [
    NgIcon,
    FormsModule,
    TimeagoModule,
    RouterLink,
    Panel,
    TableModule,
    Paginator,
    Tooltip,
    IconField,
    InputIcon,
    InputText,
    Checkbox,
    SortByComponent,
    FloatLabel,
    Select,
    SourceControlSelectComponent
  ],
  templateUrl: './list.component.html',
})
export class ListComponent implements OnInit, OnDestroy {
  loading = false;
  sorts = [
    {
      value: ProjectSortField.Name,
      label: 'name'
    },
    {
      value: ProjectSortField.CreatedAt,
      label: 'created'
    },
    {
      value: ProjectSortField.UpdatedAt,
      label: 'updated'
    }
  ];
  projects = signal<ProjectSummary[]>([]);
  filter: ProjectFilter = {
    name: '',
    sortBy: ProjectSortField.CreatedAt,
    memberUserId: undefined,
    size: 20,
    page: 1,
    desc: true,
    sourceControlId: undefined
  }
  sourceControls = signal<SourceControlSummary[]>([]);
  isDesktop = true;
  // paginator
  currentPage = signal(1);
  pageSize = signal(20);
  totalRecords = signal(0);
  firstRecord = computed(() => {
    return (this.currentPage() - 1) * this.pageSize();
  });
  users = signal<UserSummary[]>([]);
  private destroy$ = new Subject();

  constructor(
    private projectService: ProjectService,
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
    private sourceControlService: SourceControlService,
    private layoutService: LayoutService
  ) {
    this.isDesktop = this.layoutService.isDesktop();
  }

  ngOnInit(): void {
    this.sourceControlService.getSourceControlSystem().subscribe(sourceControls => {
      this.sourceControls.set(sourceControls);
    });
    this.userService.listProjectManagerUser().subscribe(users => {
      this.users.set(users);
    })
    this.route.queryParams.pipe(
      switchMap(params => {
        this.loading = true;
        bindQueryParams(params, this.filter);
        this.currentPage.set(this.filter.page!);
        this.pageSize.set(this.filter.size!);
        return this.projectService.getProjectByFilter({
          body: this.filter
        }).pipe(
          finalize(() => {
            this.loading = false;
          }),
        );
      }),
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.projects.set(response.items!);
      this.totalRecords.set(response.count!);
    })
  }

  onSearchChange() {
    updateQueryParams(this.router, this.filter);
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  sourceIcon(source?: SourceType | undefined | null): string {
    if (source) {
      return source.toString().toLowerCase()
    }
    return ""
  }

  onPageChange($event: PaginatorState) {
    this.filter.size = $event.rows;
    if ($event.page) {
      this.filter.page = $event.page + 1;
    }

    updateQueryParams(this.router, this.filter);
  }

  onSortChange($event: SortByState) {
    this.filter.sortBy = $event.sortBy;
    this.filter.desc = $event.desc;
    updateQueryParams(this.router, this.filter);
  }

  onChangeUser($event: any) {
    this.filter.memberUserId = $event;
    updateQueryParams(this.router, this.filter);
  }

  onChangeSourceControl($event: string) {
    this.filter.sourceControlId = $event;
    updateQueryParams(this.router, this.filter);
  }

  protected readonly FindingStatus = FindingStatus;
  castAsProject = cast<ProjectSummary>;
}
