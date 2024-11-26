import {Component, effect, EffectRef, Input, OnDestroy} from '@angular/core';
import {ChartComponent} from "ng-apexcharts";
import {FindingStatusSeries} from './finding-status';
import {ThemeService} from '../../../../core/theme';
import {ChartOptions, HSLToHex} from '../char-options';

@Component({
  selector: 'finding-status-chart',
  standalone: true,
    imports: [
        ChartComponent
    ],
  templateUrl: './finding-status-chart.component.html',
  styleUrl: './finding-status-chart.component.scss'
})
export class FindingStatusChartComponent implements OnDestroy {
  @Input() set status(value: FindingStatusSeries) {
    this.series = [value.open, value.confirmed, value.acceptedRisk, value.fixed]
  }
  labels = ['Needs Confirm', 'Pending Fix', 'Accepted Risk', 'Fixed'];
  series = [0, 0, 0, 0];
  chartOptions: Partial<ChartOptions>;
  constructor(
    private themeService: ThemeService
  ) {
    let baseColor = '#FFFFFF';
    this.chartOptions = {
      chart: {
        fontFamily: 'inherit',
        type: 'donut',
        height: 300,
        width: 300,
        toolbar: {
          show: false,
        },
        sparkline: {
          enabled: true,
        },
      },
      legend: {
        show: true,
        labels: {
          useSeriesColors: true
        },
        position: "bottom",
      },
      dataLabels: {
        enabled: false,
      },
      stroke: {
        show: false,
      },
      plotOptions: {
        pie: {
          expandOnClick: true,
          donut: {
            size: '50%',
            labels: {
              show: true,
              total: {
                show: true,
                color: baseColor
              },
              name: {
                show: true,
                color: baseColor
              },
              value: {
                show: true,
                color: baseColor
              }
            }
          },
        }
      },
      tooltip: {
        theme: 'light',
        y: {
          formatter: function (val) {
            return val + '';
          },
        },
      },
      colors: ['#9ca3af', '#3b82f6', '#f97316', "#22c55e"],
      title: {
        text: 'Status',
        style: {
          color: baseColor
        }
      }
    };
    this.effectRef = effect(() => {
      this.chartOptions.tooltip = {
        theme: this.themeService.theme().mode,
      };
      let primaryColor = getComputedStyle(document.documentElement).getPropertyValue('--primary');
      primaryColor = HSLToHex(primaryColor);
      this.chartOptions.title!.style!.color = primaryColor;
      this.chartOptions.plotOptions!.pie!.donut!.labels!.total!.color = primaryColor;
      this.chartOptions.plotOptions!.pie!.donut!.labels!.name!.color = primaryColor;
      this.chartOptions.plotOptions!.pie!.donut!.labels!.value!.color = primaryColor;
    });
  }
  ngOnDestroy(): void {
    this.effectRef.destroy();
  }
  private effectRef: EffectRef;
}
