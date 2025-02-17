import {Component, OnDestroy, OnInit} from '@angular/core';
import {ComingSoonComponent} from '../../../../shared/ui/coming-soon/coming-soon.component';
import {ProjectService} from '../../../../api/services/project.service';
import {ProjectStore} from '../project.store';
import {NgIcon} from '@ng-icons/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {DependencyStore} from './dependency.store';
import {bindQueryParams, updateQueryParams} from '../../../../core/router';
import {ActivatedRoute, Router} from '@angular/router';
import {finalize, Observable, Subject, switchMap, takeUntil, tap} from 'rxjs';
import {ProjectPackagePage} from '../../../../api/models/project-package-page';
import {ProjectPackage} from '../../../../api/models/project-package';
import {RiskLevelIconComponent} from '../../../../shared/components/risk-level-icon/risk-level-icon.component';
import {RiskLevel} from '../../../../api/models/risk-level';
import {ProjectPackageSortField} from '../../../../api/models';
import {IconField} from "primeng/iconfield";
import {InputIcon} from "primeng/inputicon";
import {InputText} from "primeng/inputtext";
import {FloatLabel} from 'primeng/floatlabel';
import {Select} from 'primeng/select';
import {NgClass} from '@angular/common';
import {TableModule} from 'primeng/table';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {LayoutService} from '../../../../layout/layout.service';
import {Tooltip} from 'primeng/tooltip';

@Component({
  selector: 'app-dependency',
  standalone: true,
  imports: [
    ComingSoonComponent,
    NgIcon,
    ReactiveFormsModule,
    FormsModule,
    RiskLevelIconComponent,
    IconField,
    InputIcon,
    InputText,
    FloatLabel,
    Select,
    NgClass,
    TableModule,
    Paginator,
    Tooltip,
  ],
  templateUrl: './dependency.component.html',
})
export class DependencyComponent implements OnInit, OnDestroy {
  isDesktop = true;
  private destroy$ = new Subject();

  constructor(
    private projectService: ProjectService,
    private projectStore: ProjectStore,
    public store: DependencyStore,
    private router: Router,
    private route: ActivatedRoute,
    private layoutService: LayoutService
  ) {
    this.isDesktop = this.layoutService.isDesktop();
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.projectService.getProjectPackages({
      projectId: this.projectStore.projectId(),
      body: this.store.filter
    })
    this.route.queryParams.pipe(
      switchMap(params => {
        bindQueryParams(params, this.store.filter);
        this.store.pageSize.set(this.store.filter.size!);
        this.store.currentPage.set(this.store.filter.page!);
        return this.getProjectDependencies()
      }),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  onSearchChange() {
    updateQueryParams(this.router, this.store.filter);
  }

  onReload() {
    this.getProjectDependencies().subscribe();
  }

  private getProjectDependencies(): Observable<ProjectPackagePage> {
    this.store.loading.set(true);
    return this.projectService.getProjectPackages({
      projectId: this.projectStore.projectId(),
      body: this.store.filter
    }).pipe(
      finalize(() => this.store.loading.set(false)),
      tap(response => {
        this.store.dependencies.set(response.items!);
        this.store.currentPage.set(response.currentPage!);
        this.store.totalRecords.set(response.count!);
      })
    )
  }

  onPageChange($event: PaginatorState) {
    this.store.filter.page = $event.page! + 1;
    this.store.filter.size = $event.rows;
    updateQueryParams(this.router, this.store.filter);
  }

  onOpenDependency(dependency: ProjectPackage) {
    this.store.dependency.set(dependency);
  }

  getNameDependency(dependency: ProjectPackage) {
    if (dependency.group) {
      return `${dependency.group}.${dependency.name}@${dependency.version}`;
    }
    return `${dependency.name}@${dependency.version}`;
  }

  protected readonly RiskLevel = RiskLevel;
  sortOptions = [
    {
      label: 'name',
      value: ProjectPackageSortField.Name
    },
    {
      label: 'impact',
      value: ProjectPackageSortField.RiskLevel
    }
  ];


  onSortChange(sortBy: any) {
    this.store.filter.sortBy = sortBy;
    updateQueryParams(this.router, this.store.filter);
  }

  onOrderChange() {
    this.store.filter.desc = !this.store.filter.desc;
    updateQueryParams(this.router, this.store.filter);
  }
}
