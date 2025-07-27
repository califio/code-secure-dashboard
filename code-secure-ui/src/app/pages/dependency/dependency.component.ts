import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgIcon} from '@ng-icons/core';
import {RiskLevelIconComponent} from '../../shared/components/risk-level-icon/risk-level-icon.component';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {TableModule} from 'primeng/table';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {Button} from 'primeng/button';
import {SortByComponent} from '../../shared/ui/sort-by/sort-by.component';
import {PackageTypeComponent} from '../../shared/components/package/package-type/package-type.component';
import {
  PackageStatusFilterComponent
} from '../../shared/components/package/package-status-filter/package-status-filter.component';
import {PackageDetailComponent} from '../../shared/components/package/package-detail/package-detail.component';
import {InputText} from 'primeng/inputtext';
import {
  PackageSeverityFilterComponent
} from '../../shared/components/package/package-severity-filter/package-severity-filter.component';
import {finalize, Observable, Subject, switchMap, takeUntil, tap} from 'rxjs';
import {PackageService} from '../../api/services/package.service';
import {DependencyStore} from './dependency.store';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {LayoutService} from '../../layout/layout.service';
import {ProjectService} from '../../api/services/project.service';
import {PackageStatus, ProjectPackage, ProjectPackagePage, ProjectPackageSortField, RiskLevel} from '../../api/models';
import {bindQueryParams, updateQueryParams} from '../../core/router';
import {toArray} from '../../core/transform';
import {SortByState} from '../../shared/ui/sort-by/sort-by-state';

@Component({
  selector: 'app-dependency',
  standalone: true,
  imports: [
    NgIcon,
    ReactiveFormsModule,
    FormsModule,
    RiskLevelIconComponent,
    IconField,
    InputIcon,
    InputText,
    TableModule,
    Paginator,
    SortByComponent,
    Button,
    PackageTypeComponent,
    PackageDetailComponent,
    PackageSeverityFilterComponent,
    PackageStatusFilterComponent,
    RouterLink,
  ],
  templateUrl: './dependency.component.html',
})
export class DependencyComponent implements OnInit, OnDestroy {

  isDesktop = true;
  private destroy$ = new Subject();

  constructor(
    private packageService: PackageService,
    private projectService: ProjectService,
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
    this.store.filter = {
      desc: true,
      name: '',
      page: 1,
      size: 20,
      sortBy: ProjectPackageSortField.RiskLevel
    }
    this.route.queryParams.pipe(
      switchMap(params => {
        bindQueryParams(params, this.store.filter);
        this.store.pageSize.set(this.store.filter.size!);
        this.store.currentPage.set(this.store.filter.page!);
        this.store.filter.severity = toArray(this.store.filter.severity);
        return this.getDependencies()
      }),
      takeUntil(this.destroy$)
    ).subscribe();
  }

  onSearchChange() {
    updateQueryParams(this.router, this.store.filter);
  }

  onReload() {
    this.getDependencies().subscribe();
  }

  private getDependencies(): Observable<ProjectPackagePage> {
    this.store.loading.set(true);
    return this.packageService.getPackagesByFilter({
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

  onOpenDependency(pkg: ProjectPackage) {
    this.store.loadingDependency.set(true);
    this.projectService.getProjectPackageDetail({
      projectId: pkg.projectId!,
      packageId: pkg.packageId!
    }).pipe(
      finalize(() => this.store.loadingDependency.set(false))
    ).subscribe(result => {
      this.store.packageDetail.set(result);
    });
  }

  getNamePackage(pkg: ProjectPackage) {
    if (pkg.group) {
      return `${pkg.group}/${pkg.name}@${pkg.version}`;
    }
    return `${pkg.name}@${pkg.version}`;
  }

  projectPackage(input: ProjectPackage): ProjectPackage {
    return input;
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


  onSortChange($event: SortByState) {
    this.store.filter.sortBy = $event.sortBy;
    this.store.filter.desc = $event.desc;
    updateQueryParams(this.router, this.store.filter);
  }

  onChangeSeverity($event: RiskLevel[]) {
    this.store.filter.severity = $event;
    updateQueryParams(this.router, this.store.filter);
  }

  onChangeStatus($event: PackageStatus) {
    this.store.filter.status = $event;
    updateQueryParams(this.router, this.store.filter);
  }
}
