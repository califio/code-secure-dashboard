import {Component, OnDestroy, OnInit} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {PaginationComponent} from '../../../../shared/ui/pagination/pagination.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';
import {ActivatedRoute, RouterLink} from '@angular/router';
import {DropdownComponent} from '../../../../shared/ui/dropdown/dropdown.component';
import {DropdownItem} from '../../../../shared/ui/dropdown/dropdown.model';
import {ProjectScanPage} from '../../../../api/models/project-scan-page';
import {ProjectService} from '../../../../api/services/project.service';
import {ProjectScanFilter} from '../../../../api/models/project-scan-filter';
import {finalize, Subject, switchMap, takeUntil} from 'rxjs';
import {bindQueryParams} from '../../../../core/router';
import {GitAction} from '../../../../api/models/git-action';
import {ProjectStatistics} from '../../../../api/models/project-statistics';
import {ProjectStore} from '../project.store';
import {ScanBranchComponent} from '../../../../shared/components/scan-branch/scan-branch.component';
import {ScanStatusComponent} from '../../../../shared/components/scan-status/scan-status.component';
import {ScanStatus} from '../../../../api/models';
import {TooltipDirective} from '../../../../shared/ui/tooltip/tooltip.directive';
import {SeverityChartComponent} from '../../../../shared/components/chart/severity-chart/severity-chart.component';
import {
  FindingStatusChartComponent
} from '../../../../shared/components/chart/finding-status-chart/finding-status-chart.component';
import {LoadingTableComponent} from '../../../../shared/ui/loading-table/loading-table.component';

@Component({
  selector: 'app-scan',
  standalone: true,
  imports: [
    NgIcon,
    PaginationComponent,
    ReactiveFormsModule,
    TimeagoModule,
    FormsModule,
    RouterLink,
    DropdownComponent,
    ScanBranchComponent,
    ScanStatusComponent,
    TooltipDirective,
    SeverityChartComponent,
    FindingStatusChartComponent,
    LoadingTableComponent
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
  response: ProjectScanPage = {
    count: 0,
    currentPage: 1,
    pageCount: 0,
    items: [],
    size: 0,
  };
  filter: ProjectScanFilter = {
    size: 20,
    page: 1,
    desc: true,
    scanner: null,
  }

  constructor(
    private projectService: ProjectService,
    public store: ProjectStore,
    private route: ActivatedRoute
  ) {
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
      this.response = response;
    })
  }

  onSearchChange() {

  }

  onStatusChange($event: DropdownItem) {

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

  private mIcon: Map<GitAction, string> = new Map<GitAction, string>([
    [GitAction.CommitTag, 'git-tag'],
    [GitAction.CommitBranch, 'git-branch'],
    [GitAction.MergeRequest, 'git-merge'],
  ]);

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  private destroy$ = new Subject();
  protected readonly GitAction = GitAction;
  protected readonly Date = Date;
  protected readonly ScanStatus = ScanStatus;
}
