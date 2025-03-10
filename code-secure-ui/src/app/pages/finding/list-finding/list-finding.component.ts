import {Component, HostListener, Inject, LOCALE_ID, OnInit} from '@angular/core';
import {Button} from 'primeng/button';
import {Checkbox, CheckboxChangeEvent} from 'primeng/checkbox';
import {FindingDetailComponent} from '../../../shared/components/finding/finding-detail/finding-detail.component';
import {FindingSeverityComponent} from '../../../shared/components/finding/finding-severity/finding-severity.component';
import {
  FindingStatusLabelComponent
} from '../../../shared/components/finding/finding-status-label/finding-status-label.component';
import {FormsModule} from '@angular/forms';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {NgIcon} from '@ng-icons/core';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {TableModule} from 'primeng/table';
import {Tooltip} from 'primeng/tooltip';
import {ListFindingStore} from './list-finding.store';
import {finalize, forkJoin, Subject, switchMap, takeUntil, tap} from 'rxjs';
import {FindingStatus} from '../../../api/models/finding-status';
import {
  FindingStatusMarkAsComponent
} from '../../../shared/components/finding/finding-status-mark-as/finding-status-mark-as.component';
import {FindingService} from '../../../api/services/finding.service';
import {bindQueryParams, updateQueryParams} from '../../../core/router';
import {ActivatedRoute, Router} from '@angular/router';
import {ToastrService} from '../../../shared/services/toastr.service';
import {Panel} from 'primeng/panel';
import {
  FindingStatusFilterComponent
} from '../../../shared/components/finding/finding-status-filter/finding-status-filter.component';
import {
  FindingScannerFilterComponent
} from '../../../shared/components/finding/finding-scanner-filter/finding-scanner-filter.component';
import {SortByComponent} from '../../../shared/ui/sort-by/sort-by.component';
import {SortByState} from '../../../shared/ui/sort-by/sort-by-state';
import {ScannerService} from '../../../api/services/scanner.service';
import {RuleService} from '../../../api/services/rule.service';
import {
  FindingRuleFilterComponent
} from '../../../shared/components/finding/finding-rule-filter/finding-rule-filter.component';
import {
  FindingSeverityFilterComponent
} from '../../../shared/components/finding/finding-severity-filter/finding-severity-filter.component';
import {FindingSeverity} from '../../../api/models/finding-severity';
import {UserService} from '../../../api/services/user.service';
import {FloatLabel} from 'primeng/floatlabel';
import {Select} from 'primeng/select';
import {
  FindingExportMenuComponent
} from '../../../shared/components/finding/finding-export-menu/finding-export-menu.component';
import {ExportType, FindingFilter, FindingSortField} from '../../../api/models';
import {formatDate} from '@angular/common';
import {toArray} from '../../../core/transform';
import {SourceControlService} from '../../../api/services/source-control.service';
import {
  SourceControlSelectComponent
} from '../../../shared/components/source-control-select/source-control-select.component';
import {
  FindingCategoryFilterComponent
} from '../../../shared/components/finding/finding-category-filter/finding-category-filter.component';
import {RangeDateComponent} from '../../../shared/ui/range-date/range-date.component';
import {RangeDateState} from '../../../shared/ui/range-date/range-date.model';

@Component({
  selector: 'page-list-finding',
  imports: [
    Button,
    Checkbox,
    FindingDetailComponent,
    FindingSeverityComponent,
    FindingStatusLabelComponent,
    FormsModule,
    IconField,
    InputIcon,
    InputText,
    NgIcon,
    Paginator,
    TableModule,
    Tooltip,
    FindingStatusMarkAsComponent,
    Panel,
    FindingStatusFilterComponent,
    FindingScannerFilterComponent,
    SortByComponent,
    FindingRuleFilterComponent,
    FindingSeverityFilterComponent,
    FloatLabel,
    Select,
    FindingExportMenuComponent,
    SourceControlSelectComponent,
    FindingCategoryFilterComponent,
    RangeDateComponent
  ],
  templateUrl: './list-finding.component.html',
  standalone: true
})
export class ListFindingComponent implements OnInit {

  exportTypes = [ExportType.Excel]
  showDetailFinding = false;
  selectedFindings = new Set<string>();
  isDesktop = true;
  // filter
  filter: FindingFilter & {
    fixedAtRange?: string
    createdAtRange?: string
  } = {
    desc: true,
    name: '',
    page: 1,
    scanner: [],
    severity: [],
    sortBy: FindingSortField.CreatedAt,
    ruleId: undefined,
    status: [
      FindingStatus.Open,
      FindingStatus.Confirmed,
    ],
    commitId: undefined,
    sourceControlId: undefined,
    size: 20,
    fixedAtRange: undefined,
    createdAtRange: undefined,
    category: undefined
  };
  private destroy$ = new Subject();
  loadingExport = false;

  constructor(
    public store: ListFindingStore,
    private findingService: FindingService,
    private scannerService: ScannerService,
    private userService: UserService,
    private sourceControlService: SourceControlService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    @Inject(LOCALE_ID) private locale: string
  ) {
  }

  ngOnInit(): void {
    this.sourceControlService.getSourceControlSystem().subscribe(sourceControl => {
      this.store.sourceControls.set(sourceControl);
    });
    this.userService.getProjectManagerUsers().subscribe(users => {
      this.store.users.set(users);
    });
    this.scannerService.getSastScanners().subscribe(scanners => {
      this.store.scanners.set(scanners);
    });
    this.route.queryParams.pipe(
      switchMap(params => {
        bindQueryParams(params, this.filter);
        this.filter.scanner = toArray(this.filter.scanner);
        this.filter.status = toArray(this.filter.status);
        this.filter.severity = toArray(this.filter.severity);
        if (this.filter.fixedAtRange) {
          const fixedAtRange = JSON.parse(this.filter.fixedAtRange) as RangeDateState;
          console.log(fixedAtRange);
          if (fixedAtRange && fixedAtRange.type != null) {
            if (fixedAtRange.startDate) {
              fixedAtRange.startDate = new Date(fixedAtRange.startDate);
            }
            if (fixedAtRange.endDate) {
              fixedAtRange.endDate = new Date(fixedAtRange.endDate);
            }
            this.store.fixedAtRangeDate.set(fixedAtRange);
          } else {
            this.store.fixedAtRangeDate.set({});
          }
        }
        if (this.filter.createdAtRange) {
          const createdAtRange = JSON.parse(this.filter.createdAtRange) as RangeDateState;;
          if (createdAtRange && createdAtRange.type != null) {
            if (createdAtRange.startDate) {
              createdAtRange.startDate = new Date(createdAtRange.startDate);
            }
            if (createdAtRange.endDate) {
              createdAtRange.endDate = new Date(createdAtRange.endDate);
            }
            this.store.createdAtRangeDate.set(createdAtRange);
          } else {
            this.store.createdAtRangeDate.set({});
          }
        }
        this.store.pageSize.set(this.filter.size!);
        this.store.currentPage.set(this.filter.page!);
        return forkJoin({
          findingPage: this.getFindings(),
          rules: this.getRules(),
          categories: this.getCategories()
        })
      }),
      takeUntil(this.destroy$)
    ).subscribe(result => {
      this.store.findings.set(result.findingPage.items!);
      this.store.currentPage.set(result.findingPage.currentPage!);
      this.store.totalRecords.set(result.findingPage.count!);
      this.store.rules.set(result.rules);
      this.store.categories.set(result.categories);
    })
  }

  private getRules() {
    return this.findingService.getFindingRules({
      body: this.filter
    });
  }

  private getCategories() {
    return this.findingService.getFindingCategories({
      body: this.filter
    });
  }
  private getFindings() {
    this.store.loading.set(true);
    console.log(this.store.createdAtRangeDate());
    if (this.store.createdAtRangeDate().startDate) {
      this.filter.startCreatedAt = this.store.createdAtRangeDate().startDate!.toISOString();
    } else {
      this.filter.startCreatedAt = undefined;
    }
    if (this.store.createdAtRangeDate().endDate) {
      this.filter.endCreatedAt = this.store.createdAtRangeDate().endDate!.toISOString();
    } else {
      this.filter.endCreatedAt = undefined;
    }
    //fixed
    if (this.store.fixedAtRangeDate().startDate) {
      this.filter.startFixedAt = this.store.fixedAtRangeDate().startDate!.toISOString();
    } else {
      this.filter.startFixedAt = undefined;
    }
    if (this.store.fixedAtRangeDate().endDate) {
      this.filter.endFixedAt = this.store.fixedAtRangeDate().endDate!.toISOString();
    } else {
      this.filter.endFixedAt = undefined;
    }
    this.filter.fixedAtRange = undefined;
    this.filter.createdAtRange = undefined;
    return this.findingService.getFindings({
      body: this.filter
    }).pipe(
      finalize(() => {
        this.store.loading.set(false);
      })
    );
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
    this.getFindings().subscribe();
  }

  onSearchChange() {
    updateQueryParams(this.router, this.filter);
  }

  onPageChange($event: PaginatorState) {
    this.filter.page = $event.page! + 1;
    this.filter.size = $event.rows;
    updateQueryParams(this.router, this.filter);
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
        this.toastr.success({
          message: `Change status of ${results.length} findings success`
        });
        this.selectedFindings.clear();
        this.onReload();
      });
    } else {
      this.toastr.warning({
        message: 'Select at least one finding to change status'
      });
    }
  }

  onChangeStatus($event: FindingStatus[]) {
    this.filter.status = $event;
    updateQueryParams(this.router, this.filter);
  }

  onChangeScanners($event: string[]) {
    this.filter.scanner = $event;
    updateQueryParams(this.router, this.filter);
  }

  onSelectFinding(findingId: string, $event: CheckboxChangeEvent) {
    if ($event.checked) {
      this.selectedFindings.add(findingId);
    } else {
      this.selectedFindings.delete(findingId);
    }
  }

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

  onSortChange($event: SortByState) {
    this.filter.sortBy = $event.sortBy;
    this.filter.desc = $event.desc;
    updateQueryParams(this.router, this.filter);
  }

  onChangeRule($event: string) {
    this.filter.ruleId = $event;
    updateQueryParams(this.router, this.filter);
  }

  onChangeSeverity($event: FindingSeverity[]) {
    this.filter.severity = $event;
    updateQueryParams(this.router, this.filter);
  }

  onChangeProjectManager($event: any) {
    this.filter.projectManagerId = $event;
    updateQueryParams(this.router, this.filter);
  }

  onExport($event: ExportType) {
    this.loadingExport = true;
    this.findingService.exportFinding$Any({
      body: this.filter
    }).pipe(
      finalize(() => {
        this.loadingExport = false;
      })
    ).subscribe(data => {
      const fileName = `${formatDate(Date.now(), 'yyyyMMddhhmmss', this.locale)}_export.xlsx`;
      const a = document.createElement('a');
      const objectUrl = URL.createObjectURL(data);
      a.href = objectUrl;
      a.download = fileName;
      a.click();
      URL.revokeObjectURL(objectUrl);
    });
  }

  onChangeSourceControl($event: string) {
    this.filter.sourceControlId = $event;
    updateQueryParams(this.router, this.filter);
  }

  onChangeCategory($event: string) {
    this.filter.category = $event;
    updateQueryParams(this.router, this.filter);
  }

  onChangeFixedAtRangeDate($event: RangeDateState) {
    this.filter.fixedAtRange = JSON.stringify($event);
    updateQueryParams(this.router, this.filter);
  }

  onChangeCreatedAtRangeDate($event: RangeDateState) {
    this.filter.createdAtRange = JSON.stringify($event);
    updateQueryParams(this.router, this.filter);
  }
}
