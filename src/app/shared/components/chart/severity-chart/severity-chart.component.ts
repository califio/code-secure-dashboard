import {Component, effect, EffectRef, Input, OnDestroy} from '@angular/core';
import {NgApexchartsModule} from 'ng-apexcharts';
import {Severity} from './severity';
import {ThemeService} from '../../../../core/theme';
import {ChartOptions, HSLToHex} from '../char-options';

@Component({
  selector: '[severity-chart]',
  standalone: true,
  imports: [
    NgApexchartsModule
  ],
  templateUrl: './severity-chart.component.html',
  styleUrl: './severity-chart.component.scss'
})
export class SeverityChartComponent implements OnDestroy {
  @Input() set severity(value: Severity) {
    this.series = [value.critical, value.high, value.medium, value.low, value.info]
  }

  labels = ['Critical', 'High', 'Medium', 'Low', 'Info'];
  series = [0, 0, 0, 0, 0];
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
        }
      },
      series: [],
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
      colors: ['#ef4444', '#f97316', '#eab308', '#6366f1', "#22c55e"],
      title: {
        text: 'Severity',
        style: {
          color: baseColor
        }
      }
    };
    this.effectRef = effect(() => {
      let primaryColor = getComputedStyle(document.documentElement).getPropertyValue('--primary');
      primaryColor = HSLToHex(primaryColor);
      this.chartOptions.tooltip = {
        theme: this.themeService.theme().mode,
      };
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
