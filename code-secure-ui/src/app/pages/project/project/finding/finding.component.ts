import {Component, HostListener, Inject, LOCALE_ID, OnDestroy, OnInit} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {FindingDetailComponent} from '../../../../shared/components/finding/finding-detail/finding-detail.component';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectService} from '../../../../api/services/project.service';
import {finalize, forkJoin, Subject, switchMap, takeUntil} from 'rxjs';
import {bindQueryParams, updateQueryParams} from '../../../../core/router';
import {FindingService} from '../../../../api/services/finding.service';
import {ToastrService} from '../../../../shared/services/toastr.service';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {FindingStatus} from '../../../../api/models/finding-status';
import {FindingStore} from './finding.store';
import {ProjectStore} from '../project.store';
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
import {Button} from 'primeng/button';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {Checkbox, CheckboxChangeEvent} from 'primeng/checkbox';
import {
  FindingSeverityComponent
} from '../../../../shared/components/finding/finding-severity/finding-severity.component';
import {TableModule} from 'primeng/table';
import {LayoutService} from '../../../../layout/layout.service';
import {MenuItem} from 'primeng/api';
import {Tooltip} from 'primeng/tooltip';
import {
  FindingStatusMarkAsComponent
} from '../../../../shared/components/finding/finding-status-mark-as/finding-status-mark-as.component';
import {
  FindingStatusFilterComponent
} from '../../../../shared/components/finding/finding-status-filter/finding-status-filter.component';
import {
  FindingScannerFilterComponent
} from '../../../../shared/components/finding/finding-scanner-filter/finding-scanner-filter.component';
import {SortByComponent} from '../../../../shared/ui/sort-by/sort-by.component';
import {SortByState} from '../../../../shared/ui/sort-by/sort-by-state';
import {
  FindingRuleFilterComponent
} from '../../../../shared/components/finding/finding-rule-filter/finding-rule-filter.component';
import {ScannerService} from '../../../../api/services/scanner.service';
import {
  FindingSeverityFilterComponent
} from '../../../../shared/components/finding/finding-severity-filter/finding-severity-filter.component';
import {FindingSeverity} from '../../../../api/models/finding-severity';
import {
  FindingExportMenuComponent
} from '../../../../shared/components/finding/finding-export-menu/finding-export-menu.component';
import {ExportType} from '../../../../api/models/export-type';
import {FindingSortField} from '../../../../api/models/finding-sort-field';
import {formatDate} from '@angular/common';
import {toArray} from '../../../../core/transform';
import {BranchFilterComponent, BranchOption} from '../../../../shared/components/branch-filter/branch-filter.component';

@Component({
  selector: 'app-finding',
  standalone: true,
  imports: [
    NgIcon,
    FindingDetailComponent,
    ReactiveFormsModule,
    FormsModule,
    FindingStatusLabelComponent,
    ScanBranchLabelComponent,
    IconField,
    InputIcon,
    InputText,
    FloatLabel,
    Select,
    Paginator,
    Checkbox,
    FindingSeverityComponent,
    TableModule,
    Button,
    Tooltip,
    FindingStatusMarkAsComponent,
    FindingStatusFilterComponent,
    FindingScannerFilterComponent,
    SortByComponent,
    FindingRuleFilterComponent,
    FindingSeverityFilterComponent,
    FindingExportMenuComponent,
    BranchFilterComponent,
  ],
  templateUrl: './finding.component.html',
})
export class FindingComponent implements OnInit, OnDestroy {
  showDetailFinding = false;
  selectedFindings = new Set<string>();
  isDesktop = true;
  private destroy$ = new Subject();

  constructor(
    private projectService: ProjectService,
    private scannerService: ScannerService,
    private findingService: FindingService,
    private projectStore: ProjectStore,
    private route: ActivatedRoute,
    private router: Router,
    public store: FindingStore,
    private toastrService: ToastrService,
    private layoutService: LayoutService,
    @Inject(LOCALE_ID) private locale: string
  ) {
    this.isDesktop = this.layoutService.isDesktop();
  }

  ngOnInit(): void {
    this.store.filter = {
      desc: true,
      name: '',
      page: 1,
      scanner: [],
      severity: undefined,
      sortBy: FindingSortField.CreatedAt,
      ruleId: undefined,
      status: [
        FindingStatus.Open,
        FindingStatus.Confirmed,
      ],
      commitId: undefined,
      size: 20,
    };
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
    this.scannerService.getSastScanners({
      projectId: this.projectStore.projectId()
    }).subscribe(scanners => {
      this.store.scannerOptions.set(scanners);
    });
    this.route.queryParams.pipe(
      switchMap(params => {
        bindQueryParams(params, this.store.filter);
        this.store.filter.status = toArray(this.store.filter.status);
        this.store.filter.severity = toArray(this.store.filter.severity);
        this.store.filter.scanner = toArray(this.store.filter.scanner);
        this.store.pageSize.set(this.store.filter.size!);
        this.store.currentPage.set(this.store.filter.page!);
        return forkJoin({
          findingPage: this.getFindings(),
          rules: this.getRules()
        })
      }),
      takeUntil(this.destroy$)
    ).subscribe(result => {
      this.store.findings.set(result.findingPage.items!);
      this.store.currentPage.set(result.findingPage.currentPage!);
      this.store.totalRecords.set(result.findingPage.count!);
      this.store.ruleOptions.set(result.rules);
    })
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
    this.store.finding.set(null);
  }

  onReload() {
    this.getFindings().subscribe();
  }

  private getRules() {
    return this.findingService.getFindingRules({
      body: {
        ...this.store.filter,
        projectId: this.projectStore.projectId()
      }
    });
  }

  private getFindings() {
    this.store.loading.set(true);
    return this.projectService.getProjectFindings({
      projectId: this.projectStore.projectId(),
      body: this.store.filter
    }).pipe(
      finalize(() => {
        this.store.loading.set(false);
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

  onSortChange($event: SortByState) {
    this.store.filter.sortBy = $event.sortBy;
    this.store.filter.desc = $event.desc;
    updateQueryParams(this.router, this.store.filter);
  }

  onChangeStatus($event: FindingStatus[]) {
    this.store.filter.status = $event;
    updateQueryParams(this.router, this.store.filter);
  }

  onChangeScanners($event: string[]) {
    this.store.filter.scanner = $event;
    updateQueryParams(this.router, this.store.filter);
  }

  onChangeRule($event: string) {
    this.store.filter.ruleId = $event;
    updateQueryParams(this.router, this.store.filter);
  }

  onChangeSeverity($event: FindingSeverity[]) {
    this.store.filter.severity = $event;
    updateQueryParams(this.router, this.store.filter);
  }

  onSelectFinding(findingId: string, $event: CheckboxChangeEvent) {
    if ($event.checked) {
      this.selectedFindings.add(findingId);
    } else {
      this.selectedFindings.delete(findingId);
    }
  }

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

  onExport($event: ExportType) {
    if (!this.store.filter.commitId) {
      this.toastrService.warning({
        message: 'Require branch to export report'
      });
      return;
    }
    this.store.loadingExport.set(true);

    this.projectService.export$Any({
      projectId: this.projectStore.projectId(),
      format: $event,
      body: this.store.filter
    }).pipe(
      finalize(() => {
        this.store.loadingExport.set(false);
      })
    ).subscribe(data => {
      let ext = '';
      if ($event == ExportType.Pdf) {
        ext = '.pdf';
      } else if ($event == ExportType.Excel) {
        ext = '.xlsx';
      } else {
        ext = '.json'
      }
      const fileName = `${formatDate(Date.now(), 'yyyyMMddhhmmss', this.locale)}_${this.projectStore.project().name}${ext}`;
      const a = document.createElement('a');
      const objectUrl = URL.createObjectURL(data);
      a.href = objectUrl;
      a.download = fileName;
      a.click();
      URL.revokeObjectURL(objectUrl);
    });
  }
}
