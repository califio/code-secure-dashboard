import {Component, OnDestroy, OnInit} from '@angular/core';
import {ComingSoonComponent} from "../../shared/ui/coming-soon/coming-soon.component";
import {LoadingTableComponent} from '../../shared/ui/loading-table/loading-table.component';
import {NgIcon} from '@ng-icons/core';
import {PaginationComponent} from '../../shared/ui/pagination/pagination.component';
import {ReactiveFormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';
import {FindingStatusChartComponent} from '../../shared/components/chart/finding-status-chart/finding-status-chart.component';
import {SeverityChartComponent} from '../../shared/components/chart/severity-chart/severity-chart.component';
import {SastStatistic} from '../../api/models/sast-statistic';
import {ScaStatistic} from '../../api/models/sca-statistic';
import {DashboardService} from '../../api/services/dashboard.service';
import {TopFindingChartComponent} from '../../shared/components/chart/top-finding-chart/top-finding-chart.component';
import {finalize, Subject, takeUntil} from 'rxjs';
import {TopDependencyChartComponent} from '../../shared/components/chart/top-dependency-chart/top-dependency-chart.component';
import {ActivatedRoute} from '@angular/router';
import {bindQueryParams} from '../../core/router';

interface DashboardFilter {
  from?: string,
  to?: string
}
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    ComingSoonComponent,
    LoadingTableComponent,
    NgIcon,
    PaginationComponent,
    ReactiveFormsModule,
    TimeagoModule,
    FindingStatusChartComponent,
    SeverityChartComponent,
    TopFindingChartComponent,
    TopDependencyChartComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy {
  sastStatistic: SastStatistic = {
    severity: {
      critical: 0,
      high: 0,
      info: 0,
      low: 0,
      medium: 0
    },
    status: {
      acceptedRisk: 0,
      confirmed: 0,
      fixed: 0,
      open: 0
    },
    topFindings: []
  };
  scaStatistic: ScaStatistic = {
    severity: {
      critical: 0,
      high: 0,
      info: 0,
      low: 0,
      medium: 0
    },
    status: {
      acceptedRisk: 0,
      confirmed: 0,
      fixed: 0,
      open: 0
    },
    topDependencies: [],
  };
  loadingSast = false;
  loadingSca = false;
  filter: DashboardFilter = {
    from: undefined, to: undefined
  }
  private destroy$ = new Subject();
  constructor(
    private dashboardService: DashboardService,
    private route: ActivatedRoute
  ) {
    this.route.queryParams.pipe(
      takeUntil(this.destroy$)
    ).subscribe(params => {
      bindQueryParams(params, this.filter);
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.loadingSast = true;
    this.loadingSca = true;
    this.dashboardService.sastStatistic({
      from: this.filter.from,
      to: this.filter.to
    }).pipe(
      finalize(() => this.loadingSast = false)
    ).subscribe(sast => {
      this.sastStatistic = sast;
    });
    this.dashboardService.scaStatistic({
      from: this.filter.from,
      to: this.filter.to
    })
      .pipe(
        finalize(() => this.loadingSca = false)
      ).subscribe(sca => {
      this.scaStatistic = sca;
    })
  }
}
