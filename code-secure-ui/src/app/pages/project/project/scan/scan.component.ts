import {Component, computed, OnDestroy, OnInit, signal} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';
import {ActivatedRoute, RouterLink} from '@angular/router';
import {ProjectService} from '../../../../api/services/project.service';
import {ProjectScanFilter} from '../../../../api/models/project-scan-filter';
import {finalize, Subject, switchMap, takeUntil} from 'rxjs';
import {bindQueryParams} from '../../../../core/router';
import {ProjectStatistics} from '../../../../api/models/project-statistics';
import {ProjectStore} from '../project.store';
import {
  ScanBranchLabelComponent
} from '../../../../shared/components/scan/scan-branch-label/scan-branch-label.component';
import {ScanStatusComponent} from '../../../../shared/components/scan/scan-status/scan-status.component';
import {ProjectScan, ScanStatus} from '../../../../api/models';
import {SeverityChartComponent} from '../../../dashboard/severity-chart/severity-chart.component';
import {FindingStatusChartComponent} from '../../../dashboard/finding-status-chart/finding-status-chart.component';
import {LowerCasePipe} from '@angular/common';
import {TruncatePipe} from '../../../../shared/pipes/truncate.pipe';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {Checkbox} from 'primeng/checkbox';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {TableModule} from 'primeng/table';
import {Tooltip} from 'primeng/tooltip';
import {LayoutService} from '../../../../layout/layout.service';
import {Panel} from 'primeng/panel';

@Component({
  selector: 'app-scan',
  standalone: true,
  imports: [
    NgIcon,
    ReactiveFormsModule,
    TimeagoModule,
    FormsModule,
    RouterLink,
    ScanBranchLabelComponent,
    ScanStatusComponent,
    SeverityChartComponent,
    FindingStatusChartComponent,
    LowerCasePipe,
    TruncatePipe,
    IconField,
    InputIcon,
    InputText,
    Checkbox,
    Paginator,
    TableModule,
    Tooltip,
    Panel,
  ],
  templateUrl: './scan.component.html',
  styleUrl: './scan.component.scss'
})
export class ScanComponent implements OnInit, OnDestroy {
  loading = true;
  statistic: ProjectStatistics = {
    severitySast: {
      critical: 0,
      high: 0,
      info: 0,
      low: 0,
      medium: 0
    },
    severitySca: {
      critical: 0,
      high: 0,
      info: 0,
      low: 0,
      medium: 0
    },
    statusSast: {
      acceptedRisk: 0,
      confirmed: 0,
      fixed: 0,
      open: 0
    },
    statusSca: {
      acceptedRisk: 0,
      confirmed: 0,
      fixed: 0,
      open: 0
    }
  };
  statisticLoading = false;
  scans = signal<ProjectScan[]>([]);
  filter: ProjectScanFilter = {
    size: 20,
    page: 1,
    desc: true,
    scanner: null,
  }
  isDesktop = true;
  // paginator
  currentPage = signal(1);
  pageSize = signal(20);
  totalRecords = signal(0);
  firstRecord = computed(() => {
    return (this.currentPage() - 1) * this.pageSize();
  });

  constructor(
    private projectService: ProjectService,
    public store: ProjectStore,
    private route: ActivatedRoute,
    private layoutService: LayoutService
  ) {
    this.isDesktop = this.layoutService.isDesktop();
  }

  ngOnInit(): void {
    this.statisticLoading = true;
    this.projectService.getProjectStatistic({
      projectId: this.store.projectId()
    }).pipe(
      finalize(() => {
        this.statisticLoading = false;
      })
    ).subscribe(response => {
      this.statistic = response;
    })
    this.route.queryParams.pipe(
      switchMap(params => {
        this.loading = true;
        bindQueryParams(params, this.filter);
        this.currentPage.set(this.filter.page!);
        this.pageSize.set(this.filter.size!);
        return this.projectService.getProjectScans({
          projectId: this.store.projectId(),
          body: this.filter
        }).pipe(
          finalize(() => {
            this.loading = false
          })
        );
      }),
      takeUntil(this.destroy$)
    ).subscribe(response => {
      this.scans.set(response.items!);
      this.totalRecords.set(response.count!);
    })
  }

  onSearchChange() {

  }

  duration(start?: string, end?: string | null): string {
    if (end == null || start == null) {
      return "-";
    }
    const startDate = new Date(start);
    const endDate = new Date(end);
    const durationInMs = endDate.getTime() - startDate.getTime();
    const seconds = Math.floor((durationInMs / 1000) % 60);
    const minutes = Math.floor((durationInMs / (1000 * 60)) % 60);
    const hours = Math.floor((durationInMs / (1000 * 60 * 60)) % 24);
    const days = Math.floor(durationInMs / (1000 * 60 * 60 * 24));
    const result = [];
    if (days > 0) result.push(`${days} days`);
    if (hours > 0) result.push(`${hours} hours`);
    if (minutes > 0) result.push(`${minutes} minutes`);
    if (seconds > 0) result.push(`${seconds} seconds`);
    return result.length > 0 ? result.join(", ") : "0 seconds";
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  private destroy$ = new Subject();
  protected readonly ScanStatus = ScanStatus;
  onPageChange($event: PaginatorState) {

  }
}
