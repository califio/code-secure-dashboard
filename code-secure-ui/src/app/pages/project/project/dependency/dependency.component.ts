import {Component, OnDestroy, OnInit} from '@angular/core';
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
import {PackageStatus, ProjectPackageSortField} from '../../../../api/models';
import {IconField} from "primeng/iconfield";
import {InputIcon} from "primeng/inputicon";
import {InputText} from "primeng/inputtext";
import {TableModule} from 'primeng/table';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {LayoutService} from '../../../../layout/layout.service';
import {SortByComponent} from '../../../../shared/ui/sort-by/sort-by.component';
import {SortByState} from '../../../../shared/ui/sort-by/sort-by-state';
import {Button} from 'primeng/button';
import {PackageTypeComponent} from '../../../../shared/components/package/package-type/package-type.component';
import {PackageDetailComponent} from '../../../../shared/components/package/package-detail/package-detail.component';
import {BranchFilterComponent, BranchOption} from '../../../../shared/components/branch-filter/branch-filter.component';
import {
  PackageSeverityFilterComponent
} from '../../../../shared/components/package/package-severity-filter/package-severity-filter.component';
import {toArray} from '../../../../core/transform';
import {
  PackageStatusFilterComponent
} from '../../../../shared/components/package/package-status-filter/package-status-filter.component';

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
    BranchFilterComponent,
    PackageSeverityFilterComponent,
    PackageStatusFilterComponent,
  ],
  templateUrl: './dependency.component.html',
})
export class DependencyComponent implements OnInit, OnDestroy {

  isDesktop = true;
  private destroy$ = new Subject();

  constructor(
    private projectService: ProjectService,
    public projectStore: ProjectStore,
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
    this.projectService.getProjectPackages({
      projectId: this.projectStore.projectId(),
      body: this.store.filter
    })
    this.route.queryParams.pipe(
      switchMap(params => {
        bindQueryParams(params, this.store.filter);
        this.store.pageSize.set(this.store.filter.size!);
        this.store.currentPage.set(this.store.filter.page!);
        this.store.filter.severity = toArray(this.store.filter.severity);
        return this.getProjectDependencies()
      }),
      takeUntil(this.destroy$)
    ).subscribe();

    this.projectService.getProjectCommits({
      projectId: this.projectStore.projectId()
    }).subscribe(commits => {
      const options = commits.map(item => {
        return <BranchOption>{
          id: item.commitId,
          commitBranch: item.branch,
          commitType: item.action,
          targetBranch: item.targetBranch
        }
      });
      this.store.branchOptions.set(options);
    });
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

  onOpenDependency(pkg: ProjectPackage) {
    this.store.loadingDependency.set(true);
    this.projectService.getProjectPackageDetail({
      projectId: this.projectStore.projectId(),
      packageId: pkg.packageId!
    }).pipe(
      finalize(() => this.store.loadingDependency.set(false))
    ).subscribe(result => {
      this.store.packageDetail.set(result);
    });
  }

  getNamePackage(pkg: ProjectPackage) {
    if (pkg.group) {
      return `${pkg.group}.${pkg.name}@${pkg.version}`;
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

  onChangeBranch($event: string) {
    this.store.filter.commitId = $event;
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
