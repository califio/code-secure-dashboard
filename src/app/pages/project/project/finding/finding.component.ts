import {Component, HostListener, OnDestroy, OnInit} from '@angular/core';
import {ListFindingComponent} from '../../../../shared/components/finding/list-finding/list-finding.component';
import {NgIcon} from '@ng-icons/core';
import {DropdownComponent} from '../../../../shared/components/ui/dropdown/dropdown.component';
import {FindingStatusComponent} from '../../../../shared/components/finding/finding-status/finding-status.component';
import {FindingDetailComponent} from '../../../../shared/components/finding/finding-detail/finding-detail.component';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectService} from '../../../../api/services/project.service';
import {delay, finalize, forkJoin, Observable, Subject, switchMap, takeUntil, tap} from 'rxjs';
import {bindQueryParams, updateQueryParams} from '../../../../core/router';
import {ProjectFindingPage} from '../../../../api/models/project-finding-page';
import {FindingService} from '../../../../api/services/finding.service';
import {FindingDetail} from '../../../../api/models/finding-detail';
import {ToastrService} from '../../../../shared/components/toastr/toastr.service';
import {
  ScanBranchDropdownComponent
} from '../../../../shared/components/scan-branch-dropdown/scan-branch-dropdown.component';
import {LoadingTableComponent} from '../../../../shared/components/ui/loading-table/loading-table.component';
import {PaginationComponent} from '../../../../shared/components/ui/pagination/pagination.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {FindingStatus} from '../../../../api/models/finding-status';
import {FindingStore} from './finding.store';
import {
  FindingStatusFilterComponent
} from '../../../../shared/components/finding/finding-status-filter/finding-status-filter.component';
import {ProjectStore} from '../project-store';
import {ScannerDropdownComponent} from '../../../../shared/components/scanner-dropdown/scanner-dropdown.component';
import {ProjectScanner} from '../../../../api/models/project-scanner';
import {DropdownItem} from '../../../../shared/components/ui/dropdown/dropdown.model';
import {ProjectFindingSortField} from '../../../../api/models';

@Component({
  selector: 'app-finding',
  standalone: true,
  imports: [
    ListFindingComponent,
    NgIcon,
    DropdownComponent,
    FindingStatusComponent,
    FindingDetailComponent,
    ScanBranchDropdownComponent,
    LoadingTableComponent,
    PaginationComponent,
    ReactiveFormsModule,
    FormsModule,
    FindingStatusFilterComponent,
    ScannerDropdownComponent,
  ],
  templateUrl: './finding.component.html',
  styleUrl: './finding.component.scss'
})
export class FindingComponent implements OnInit, OnDestroy {
  slug = '';
  finding: FindingDetail | null = null;
  showDetailFinding = false;
  loadingFinding = false;
  selectedFindings: string[] = [];
  commitId: string | null | undefined = null;

  constructor(
    private projectService: ProjectService,
    private projectStore: ProjectStore,
    private route: ActivatedRoute,
    private router: Router,
    private findingService: FindingService,
    public store: FindingStore,
    private toastrService: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.slug = this.projectStore.slug();
    this.projectService.getProjectCommits({
      slug: this.slug
    }).subscribe(branches => {
      this.store.commits.set(branches);
      this.commitId = this.store.filter.commitId;
    });
    this.projectService.getProjectScanners({
      slug: this.slug
    }).subscribe(scanners => {
      this.store.scanners.set(scanners);
      this.scanner = scanners.find(scanner => scanner.name == this.store.filter.scanner
        && this.store.filter.type == scanner.type);
    })
    this.route.queryParams.pipe(
      switchMap(params => {
        bindQueryParams(params, this.store.filter);
        return this.getProjectFindings();
      }),
      takeUntil(this.destroy$)
    ).subscribe()
  }

  onOpenFinding(findingId: string) {
    if (this.showDetailFinding) {
      this.router.navigate(['/finding', findingId]).then();
    } else {
      this.finding = null;
      this.loadingFinding = true;
      this.findingService.getFinding({
        id: findingId
      }).pipe(
        delay(200),
        finalize(() => {
          this.loadingFinding = false
        })
      ).subscribe(finding => {
        this.finding = finding;
      })
    }
  }

  closeFinding() {
    this.finding = null;
  }

  @HostListener('window:resize', ['$event'])
  getScreenSize() {
    this.showDetailFinding = window.innerWidth < 1024;
  }

  private destroy$ = new Subject();
  search = '';
  scanner: ProjectScanner | null | undefined;
  sortOptions: DropdownItem[] = [
    {
      value: ProjectFindingSortField.Name,
      label: 'Name'
    },
    {
      value: ProjectFindingSortField.UpdatedAt,
      label: 'Updated'
    },
    {
      value: ProjectFindingSortField.CreatedAt,
      label: 'Created'
    },
    {
      value: ProjectFindingSortField.Status,
      label: 'Status'
    },
    {
      value: ProjectFindingSortField.Severity,
      label: 'Severity'
    }
  ];

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  onReload() {
    this.getProjectFindings().subscribe();
  }

  private getProjectFindings(): Observable<ProjectFindingPage> {
    this.store.loading.set(true);
    return this.projectService.getProjectFindings({
      slug: this.slug,
      body: this.store.filter
    }).pipe(
      delay(300),
      finalize(() => {
        this.store.loading.set(false);
      }),
      tap(response => {
        this.store.findings.set(response.items!);
        this.store.currentPage.set(response.currentPage!);
        this.store.totalPage.set(response.pageCount!);
      })
    );
  }

  onSearchChange() {
    updateQueryParams(this.router, this.store.filter);
  }

  onStatusChange(status: FindingStatus | undefined) {
    this.store.filter.status = status;
    updateQueryParams(this.router, this.store.filter);
  }

  onSelectScan(scanId: string) {
    this.store.filter.commitId = scanId;
    updateQueryParams(this.router, this.store.filter);
  }

  onPageChange(page: number) {
    this.store.filter.page = page;
    updateQueryParams(this.router, this.store.filter);
  }

  selectFindingsChange(findingIds: string[]) {
    this.selectedFindings = findingIds;
  }

  onMarkAs(status: FindingStatus) {
    if (this.selectedFindings.length > 0) {
      var requests = this.selectedFindings.map(findingId => this.findingService.updateFinding({
        id: findingId,
        body: {
          status: status
        }
      }));
      forkJoin(requests).subscribe((results) => {
        this.toastrService.success(`change status of ${results.length} findings success`);
        this.selectedFindings = [];
        this.onReload();
      });
    } else {
      this.toastrService.warning('select at least one finding to change status', 5000);
    }
  }

  onChangeScanner(scanner: ProjectScanner | null) {
    if (scanner != null) {
      this.store.filter.scanner = scanner.name;
      this.store.filter.type = scanner.type;
    } else {
      this.store.filter.scanner = undefined;
      this.store.filter.type = undefined;
    }
    updateQueryParams(this.router, this.store.filter);
  }

  onOrderChange() {
    this.store.filter.desc = !this.store.filter.desc;
    updateQueryParams(this.router, this.store.filter);
  }

  onSortChange(sortBy: any) {
    this.store.filter.sortBy = sortBy;
    updateQueryParams(this.router, this.store.filter);
  }
}
