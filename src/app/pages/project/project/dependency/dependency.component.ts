import {Component, OnDestroy, OnInit} from '@angular/core';
import {ComingSoonComponent} from '../../../../shared/ui/coming-soon/coming-soon.component';
import {ProjectService} from '../../../../api/services/project.service';
import {ProjectStore} from '../project.store';
import {FindingDetailComponent} from '../../../../shared/components/finding/finding-detail/finding-detail.component';
import {FindingStatusComponent} from '../../../../shared/components/finding/finding-status/finding-status.component';
import {
  FindingStatusFilterComponent
} from '../../../../shared/components/finding/finding-status-filter/finding-status-filter.component';
import {ListFindingComponent} from '../../../../shared/components/finding/list-finding/list-finding.component';
import {NgIcon} from '@ng-icons/core';
import {PaginationComponent} from '../../../../shared/ui/pagination/pagination.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {
  ScanBranchDropdownComponent
} from '../../../../shared/components/scan-branch-dropdown/scan-branch-dropdown.component';
import {ScannerDropdownComponent} from '../../../../shared/components/scanner-dropdown/scanner-dropdown.component';
import {DependencyStore} from './dependency.store';
import {bindQueryParams, updateQueryParams} from '../../../../core/router';
import {ActivatedRoute, Router} from '@angular/router';
import {finalize, Observable, Subject, switchMap, takeUntil, tap} from 'rxjs';
import {ProjectPackagePage} from '../../../../api/models/project-package-page';
import {
  FindingStatusLabelComponent
} from '../../../../shared/components/finding/finding-status-label/finding-status-label.component';
import {LoadingTableComponent} from '../../../../shared/ui/loading-table/loading-table.component';
import {ProjectPackage} from '../../../../api/models/project-package';
import {RiskLevelIconComponent} from '../../../../shared/components/risk-level-icon/risk-level-icon.component';
import {RiskLevel} from '../../../../api/models/risk-level';
import {TooltipDirective} from '../../../../shared/ui/tooltip/tooltip.directive';

@Component({
  selector: 'app-dependency',
  standalone: true,
  imports: [
    ComingSoonComponent,
    FindingDetailComponent,
    FindingStatusComponent,
    FindingStatusFilterComponent,
    ListFindingComponent,
    NgIcon,
    PaginationComponent,
    ReactiveFormsModule,
    ScanBranchDropdownComponent,
    ScannerDropdownComponent,
    FormsModule,
    FindingStatusLabelComponent,
    LoadingTableComponent,
    RiskLevelIconComponent,
    TooltipDirective
  ],
  templateUrl: './dependency.component.html',
  styleUrl: './dependency.component.scss'
})
export class DependencyComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject();
  loadingDependency = false;
  dependency = false;

  constructor(
    private projectService: ProjectService,
    private projectStore: ProjectStore,
    public store: DependencyStore,
    private router: Router,
    private route: ActivatedRoute
  ) {
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
        this.store.totalPage.set(response.pageCount!);
      })
    )
  }

  onPageChange(page: number) {
    this.store.filter.page = page;
    updateQueryParams(this.router, this.store.filter);
  }

  onOpenDependency(id: string | undefined) {
    this.dependency = true;
  }

  getNameDependency(dependency: ProjectPackage) {
    if (dependency.group) {
      return `${dependency.group}.${dependency.name}@${dependency.version}`;
    }
    return `${dependency.name}@${dependency.version}`;
  }

  protected readonly RiskLevel = RiskLevel;
}
