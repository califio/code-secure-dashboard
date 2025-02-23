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
  FindingStatusMenuComponent
} from '../../../shared/components/finding/finding-status-menu/finding-status-menu.component';
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
import {ExportType} from '../../../api/models';
import {formatDate} from '@angular/common';

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
    FindingStatusMenuComponent,
    Panel,
    FindingStatusFilterComponent,
    FindingScannerFilterComponent,
    SortByComponent,
    FindingRuleFilterComponent,
    FindingSeverityFilterComponent,
    FloatLabel,
    Select,
    FindingExportMenuComponent
  ],
  templateUrl: './list-finding.component.html',
  standalone: true
})
export class ListFindingComponent implements OnInit {
  exportTypes = [ExportType.Excel]
  showDetailFinding = false;
  selectedFindings = new Set<string>();
  isDesktop = true;
  private destroy$ = new Subject();
  loadingExport = false;

  constructor(
    public store: ListFindingStore,
    private findingService: FindingService,
    private scannerService: ScannerService,
    private ruleService: RuleService,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService,
    @Inject(LOCALE_ID) private locale: string
  ) {
  }

  ngOnInit(): void {
    this.userService.getProjectManagerUsers().subscribe(users => {
      this.store.users.set(users);
    });
    this.scannerService.getScanners().subscribe(scanners => {
      this.store.scanners.set(scanners);
    });
    this.ruleService.getRules({
      body: {}
    }).subscribe(rules => {
      this.store.rules.set(rules);
    });
    this.route.queryParams.pipe(
      switchMap(params => {
        bindQueryParams(params, this.store.filter);
        this.store.pageSize.set(this.store.filter.size!);
        this.store.currentPage.set(this.store.filter.page!);
        return this.getFindings();
      }),
      takeUntil(this.destroy$)
    ).subscribe()
  }

  private getFindings() {
    this.store.loading.set(true);
    if (this.store.filter.scanner) {
      if (!Array.isArray(this.store.filter.scanner)) {
        this.store.filter.scanner = [this.store.filter.scanner];
      }
    }
    if (this.store.filter.severity) {
      if (!Array.isArray(this.store.filter.severity)) {
        this.store.filter.severity = [this.store.filter.severity];
      }
    }
    return this.findingService.getFindings({
      body: this.store.filter
    }).pipe(
      finalize(() => {
        this.store.loading.set(false);
      }),
      tap(response => {
        this.store.findings.set(response.items!);
        this.store.currentPage.set(response.currentPage!);
        this.store.totalRecords.set(response.count!);
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
    this.store.filter.status = $event;
    updateQueryParams(this.router, this.store.filter);
  }

  onChangeScanners($event: string[]) {
    this.store.filter.scanner = $event;
    updateQueryParams(this.router, this.store.filter);
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
    this.store.filter.sortBy = $event.sortBy;
    this.store.filter.desc = $event.desc;
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

  onChangeProjectManager($event: any) {
    this.store.filter.projectManagerId = $event;
    updateQueryParams(this.router, this.store.filter);
  }

  onExport($event: ExportType) {
    this.loadingExport = true;
    this.findingService.exportFinding$Any({
      body: this.store.filter
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
}
