import {Component, OnDestroy, OnInit, signal} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';
import {TimeagoModule} from 'ngx-timeago';
import {FindingStatusChartComponent} from './finding-status-chart/finding-status-chart.component';
import {SeverityChartComponent} from './severity-chart/severity-chart.component';
import {DashboardService} from '../../api/services/dashboard.service';
import {TopFindingChartComponent} from './top-finding-chart/top-finding-chart.component';
import {Subject, takeUntil} from 'rxjs';
import {TopDependencyChartComponent} from './top-dependency-chart/top-dependency-chart.component';
import {Fluid} from 'primeng/fluid';
import {LayoutService} from '../../layout/layout.service';
import {Severity} from './severity-chart/severity';
import {FindingStatusSeries} from './finding-status-chart/finding-status';
import {TopFinding} from '../../api/models/top-finding';
import {Chart} from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import {TopDependency} from '../../api/models/top-dependency';
import {Card} from 'primeng/card';
import {RangeDateComponent} from '../../shared/ui/range-date/range-date.component';
import {getRangeDate, RangeDateType} from '../../shared/date-util';
import {RangeDateState} from '../../shared/ui/range-date/range-date.model';
import {DashboardStore} from './dashboard.store';
import {StatisticFilter} from '../../api/models/statistic-filter';
import {SourceControlSystemService} from '../../api/services/source-control-system.service';
import {
  SourceControlSelectComponent
} from '../../shared/components/source-control-select/source-control-select.component';

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
    Card,
    RangeDateComponent,
    SourceControlSelectComponent
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
  rangeDate: RangeDateState = {
    type: RangeDateType.Last30Days,
    ...getRangeDate(RangeDateType.Last30Days)
  }
  filter: StatisticFilter = {}
  private destroy$ = new Subject();

  constructor(
    private layoutService: LayoutService,
    private dashboardService: DashboardService,
    private sourceControlService: SourceControlSystemService,
    public store: DashboardStore
  ) {
    Chart.register(ChartDataLabels);
    this.layoutService.configUpdate$.pipe(
      takeUntil(this.destroy$),
    ).subscribe(() => {
      this.initCharts();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.sourceControlService.getSourceControlSystem().subscribe(sourceControl => {
      this.store.sourceControls.set(sourceControl);
    })
    this.initCharts();
  }

  initCharts() {
    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = documentStyle.getPropertyValue('--text-color');
    const borderColor = documentStyle.getPropertyValue('--surface-border');
    this.store.textColor.set(textColor);
    this.store.borderColor.set(borderColor);
    this.filter.startDate = this.rangeDate.startDate.toISOString();
    this.filter.endDate = this.rangeDate.endDate.toISOString();
    this.dashboardService.sastStatistic({
      body: this.filter
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
      body: this.filter
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

  onRangeDateChange($event: RangeDateState) {
    this.rangeDate = $event;
    this.initCharts();
  }

  onChangeSourceControl($event: string) {
    this.filter.sourceId = $event;
    this.initCharts();
  }
}
