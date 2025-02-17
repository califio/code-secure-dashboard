import {Component, HostListener, OnDestroy, OnInit} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {FindingDetailComponent} from '../../../../shared/components/finding/finding-detail/finding-detail.component';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectService} from '../../../../api/services/project.service';
import {finalize, forkJoin, Observable, Subject, switchMap, takeUntil, tap} from 'rxjs';
import {bindQueryParams, updateQueryParams} from '../../../../core/router';
import {ProjectFindingPage} from '../../../../api/models/project-finding-page';
import {FindingService} from '../../../../api/services/finding.service';
import {ToastrService} from '../../../../shared/services/toastr.service';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {FindingStatus} from '../../../../api/models/finding-status';
import {FindingStore} from './finding.store';
import {ProjectStore} from '../project.store';
import {ProjectFindingSortField} from '../../../../api/models';
import {ScannerLabelComponent} from "../../../../shared/components/scanner-label/scanner-label.component";
import {
  FindingStatusLabelComponent
} from '../../../../shared/components/finding/finding-status-label/finding-status-label.component';
import {
  ScanBranchLabelComponent
} from '../../../../shared/components/scan/scan-branch-label/scan-branch-label.component';
import {IconField} from "primeng/iconfield";
import {InputIcon} from "primeng/inputicon";
import {InputText} from "primeng/inputtext";
import {FloatLabel} from 'primeng/floatlabel';
import {Select} from 'primeng/select';
import {MultiSelect, MultiSelectChangeEvent} from 'primeng/multiselect';
import {Button} from 'primeng/button';
import {getFindingStatusOptions} from '../../../../shared/components/finding/finding-status';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {Checkbox, CheckboxChangeEvent} from 'primeng/checkbox';
import {
  FindingSeverityComponent
} from '../../../../shared/components/finding/finding-severity/finding-severity.component';
import {TableModule} from 'primeng/table';
import {LayoutService} from '../../../../layout/layout.service';
import {Menu} from 'primeng/menu';
import {MenuItem} from 'primeng/api';
import {OverlayBadge} from 'primeng/overlaybadge';
import {Tooltip} from 'primeng/tooltip';

@Component({
  selector: 'app-finding',
  standalone: true,
  imports: [
    NgIcon,
    FindingDetailComponent,
    ReactiveFormsModule,
    FormsModule,
    ScannerLabelComponent,
    FindingStatusLabelComponent,
    ScanBranchLabelComponent,
    IconField,
    InputIcon,
    InputText,
    FloatLabel,
    Select,
    MultiSelect,
    Paginator,
    Checkbox,
    FindingSeverityComponent,
    TableModule,
    Menu,
    Button,
    OverlayBadge,
    Tooltip,
  ],
  templateUrl: './finding.component.html',
})
export class FindingComponent implements OnInit, OnDestroy {
  showDetailFinding = false;
  statusOptions = getFindingStatusOptions();
  selectedFindings = new Set<string>();
  isDesktop = true;
  menuItems: MenuItem[] = [];
  private destroy$ = new Subject();

  constructor(
    private projectService: ProjectService,
    private projectStore: ProjectStore,
    private route: ActivatedRoute,
    private router: Router,
    private findingService: FindingService,
    public store: FindingStore,
    private toastrService: ToastrService,
    private layoutService: LayoutService
  ) {
    this.isDesktop = this.layoutService.isDesktop();
    this.menuItems = getFindingStatusOptions().map(item => {
      return {
        status: item.status,
        label: item.label
      }
    })
  }

  ngOnInit(): void {
    this.projectService.getProjectCommits({
      projectId: this.projectStore.projectId()
    }).subscribe(commits => {
      this.store.commits.set(commits);
    });

    this.projectService.getProjectScanners({
      projectId: this.projectStore.projectId()
    }).subscribe(scanners => {
      this.store.scanners.set(scanners);
    });

    this.route.queryParams.pipe(
      switchMap(params => {
        bindQueryParams(params, this.store.filter);
        this.store.pageSize.set(this.store.filter.size!);
        this.store.currentPage.set(this.store.filter.page!);
        return this.getProjectFindings();
      }),
      takeUntil(this.destroy$)
    ).subscribe()
  }

  onOpenFinding(findingId: string) {
    if (this.showDetailFinding) {
      this.router.navigate(['/finding', findingId]).then();
    } else {
      this.store.loadingFinding.set(true);
      this.findingService.getFinding({
        id: findingId
      }).pipe(
        finalize(() => {
          this.store.loadingFinding.set(false);
        })
      ).subscribe(finding => {
        this.store.finding.set(finding);
      })
    }
  }

  closeFinding() {
    this.store.finding.set(null);
  }

  @HostListener('window:resize', ['$event'])
  getScreenSize() {
    this.showDetailFinding = window.innerWidth < 1024;
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  onReload() {
    this.getProjectFindings().subscribe();
  }

  private getProjectFindings(): Observable<ProjectFindingPage> {
    this.store.loadingFindings.set(true);
    if (this.store.filter.scanner) {
      if (!Array.isArray(this.store.filter.scanner)) {
        this.store.filter.scanner = [this.store.filter.scanner];
      }
    }
    return this.projectService.getProjectFindings({
      projectId: this.projectStore.projectId(),
      body: this.store.filter
    }).pipe(
      finalize(() => {
        this.store.loadingFindings.set(false);
      }),
      tap(response => {
        this.store.findings.set(response.items!);
        this.store.currentPage.set(response.currentPage!);
        this.store.totalRecords.set(response.count!);
      })
    );
  }

  onSearchChange() {
    updateQueryParams(this.router, this.store.filter);
  }

  onSelectScan(scanId: string) {
    this.store.filter.commitId = scanId;
    updateQueryParams(this.router, this.store.filter);
  }

  onPageChange($event: PaginatorState) {
    this.store.filter.page = $event.page! + 1;
    this.store.filter.size = $event.rows;
    updateQueryParams(this.router, this.store.filter);
  }

  onMarkAs(status: FindingStatus) {
    if (this.selectedFindings.size > 0) {
      const requests = Array.from(this.selectedFindings.values()).map(findingId => this.findingService.updateFinding({
        id: findingId,
        body: {
          status: status
        }
      }));
      forkJoin(requests).subscribe((results) => {
        this.toastrService.success({
          message: `Change status of ${results.length} findings success`
        });
        this.selectedFindings.clear();
        this.onReload();
      });
    } else {
      this.toastrService.warning({
        message: 'Select at least one finding to change status'
      });
    }
  }

  onOrderChange() {
    this.store.filter.desc = !this.store.filter.desc;
    updateQueryParams(this.router, this.store.filter);
  }

  onSortChange(sortBy: any) {
    this.store.filter.sortBy = sortBy;
    updateQueryParams(this.router, this.store.filter);
  }

  onChangeStatus($event: MultiSelectChangeEvent) {
    this.store.filter.status = $event.value;
    updateQueryParams(this.router, this.store.filter);
  }

  onChangeScanners($event: MultiSelectChangeEvent) {
    this.store.filter.scanner = $event.value;
    updateQueryParams(this.router, this.store.filter);
  }

  onSelectFinding(findingId: string, $event: CheckboxChangeEvent) {
    if ($event.checked) {
      this.selectedFindings.add(findingId);
    } else {
      this.selectedFindings.delete(findingId);
    }
  }

  sortOptions = [
    {
      value: ProjectFindingSortField.Name,
      label: 'name'
    },
    {
      value: ProjectFindingSortField.UpdatedAt,
      label: 'updated'
    },
    {
      value: ProjectFindingSortField.CreatedAt,
      label: 'created'
    },
    {
      value: ProjectFindingSortField.Status,
      label: 'status'
    },
    {
      value: ProjectFindingSortField.Severity,
      label: 'severity'
    }
  ];
  items: MenuItem[] = [
    {
      label: 'Refresh',
      icon: 'pi pi-refresh'
    },
    {
      label: 'Export',
      icon: 'pi pi-upload'
    }
  ];

  onSelectAllChange($event: CheckboxChangeEvent) {
    if ($event.checked) {
      this.selectedFindings.clear();
      this.store.findings().forEach(finding => {
        this.selectedFindings.add(finding.id!);
      })
    } else {
      this.selectedFindings.clear();
    }
  }
}
