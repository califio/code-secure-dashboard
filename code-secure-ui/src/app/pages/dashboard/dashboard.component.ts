import {Component, OnDestroy, OnInit, signal} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';
import {FindingStatusChartComponent} from './finding-status-chart/finding-status-chart.component';
import {SeverityChartComponent} from './severity-chart/severity-chart.component';
import {DashboardService} from '../../api/services/dashboard.service';
import {TopFindingChartComponent} from './top-finding-chart/top-finding-chart.component';
import {debounceTime, Subject, takeUntil} from 'rxjs';
import {TopDependencyChartComponent} from './top-dependency-chart/top-dependency-chart.component';
import {ActivatedRoute} from '@angular/router';
import {bindQueryParams} from '../../core/router';
import {Fluid} from 'primeng/fluid';
import {LayoutService} from '../../layout/layout.service';
import {Severity} from './severity-chart/severity';
import {FindingStatusSeries} from './finding-status-chart/finding-status';
import {TopFinding} from '../../api/models/top-finding';
import {Chart, registerables} from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import {TopDependency} from '../../api/models/top-dependency';
import {Card} from 'primeng/card';


interface DashboardFilter {
  from?: string,
  to?: string
}

Chart.register(...registerables, ChartDataLabels);

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    TimeagoModule,
    FindingStatusChartComponent,
    SeverityChartComponent,
    TopFindingChartComponent,
    TopDependencyChartComponent,
    Fluid,
    Card
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy {
  sastSeverity = signal<Severity>({
    critical: 0, high: 0, info: 0, low: 0, medium: 0
  });
  scaSeverity = signal<Severity>({
    critical: 0, high: 0, info: 0, low: 0, medium: 0
  });
  sastStatus = signal<FindingStatusSeries>({acceptedRisk: 0, confirmed: 0, fixed: 0, open: 0});
  scaStatus = signal<FindingStatusSeries>({acceptedRisk: 0, confirmed: 0, fixed: 0, open: 0});
  topFindings = signal<TopFinding[]>([]);
  topDependencies = signal<TopDependency[]>([]);
  filter: DashboardFilter = {
    from: undefined, to: undefined
  }
  private destroy$ = new Subject();

  constructor(
    private layoutService: LayoutService,
    private dashboardService: DashboardService,
    private route: ActivatedRoute
  ) {
    this.layoutService.configUpdate$.pipe(
      takeUntil(this.destroy$),
      debounceTime(25)
    ).subscribe(() => {
      this.initCharts();
    });
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
    this.initCharts();
  }

  initCharts() {
    this.dashboardService.sastStatistic({
      from: this.filter.from,
      to: this.filter.to
    }).subscribe(sast => {
      this.sastSeverity.set({
        critical: sast.severity.critical,
        high: sast.severity.high,
        info: sast.severity.info,
        low: sast.severity.low,
        medium: sast.severity.medium
      });
      this.sastStatus.set({
        acceptedRisk: sast.status.acceptedRisk,
        confirmed: sast.status.confirmed,
        fixed: sast.status.fixed,
        open: sast.status.open
      });
      this.topFindings.set(sast.topFindings);
    });

    this.dashboardService.scaStatistic({
      from: this.filter.from,
      to: this.filter.to
    }).subscribe(sca => {
      this.scaSeverity.set({
        critical: sca.severity.critical,
        high: sca.severity.high,
        info: sca.severity.info,
        low: sca.severity.low,
        medium: sca.severity.medium
      });
      this.scaStatus.set({
        acceptedRisk: sca.status.acceptedRisk,
        confirmed: sca.status.confirmed,
        fixed: sca.status.fixed,
        open: sca.status.open
      });
      this.topDependencies.set(sca.topDependencies);
    });
  }
}
