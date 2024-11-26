import {Component, effect, EffectRef, Input, OnDestroy} from '@angular/core';
import {ChartComponent} from 'ng-apexcharts';
import {TopFinding} from '../../../../api/models/top-finding';
import {ThemeService} from '../../../../core/theme';
import {ChartOptions, HSLToHex} from '../char-options';

@Component({
  selector: 'top-finding-chart',
  standalone: true,
  imports: [
    ChartComponent
  ],
  templateUrl: './top-finding-chart.component.html',
  styleUrl: './top-finding-chart.component.scss'
})
export class TopFindingChartComponent implements OnDestroy {
  @Input()
  set categories(value: TopFinding[]) {
    this.chartOptions.xaxis!.categories = value.map(item => item.category);
    this.chartOptions.series![0].data = value.map(item => item.count);
  }

  chartOptions: Partial<ChartOptions>;

  constructor(
    private themeService: ThemeService
  ) {
    let baseColor = '#FFFFFF';
    this.chartOptions = {
      chart: {
        type: 'bar',
        stacked: true,
        width: 600,
        height: 380
      },
      legend: {
        position: "right",
        show: true,
        labels: {
          useSeriesColors: true
        }
      },
      dataLabels: {
        enabled: false,
      },
      plotOptions: {
        bar: {
          horizontal: true,
          barHeight: '10',
          distributed: false,
          dataLabels: {
            total: {
              enabled: true,
              offsetX: 5,
              style: {
                color: 'black',
                fontWeight: "normal"
              }
            }
          }
        },
      },
      series: [
        {
          name: 'count',
          data: []
        }
      ],
      grid: {
        yaxis: {
          lines: {
            show: false,
            offsetY: 10,
          },
        },
        padding: {
          top: 5,
          bottom: 5
        }
      },
      xaxis: {
        categories: [],
        labels: {
          show: true,
          style: {
            colors: [baseColor]
          }
        },
      },
      yaxis: {
        show: true,
        labels: {
          show: true,
          align: 'left',
          offsetX: 5,
          offsetY: 15,
          maxWidth: 500,
          style: {
            colors: [],
          }
        },
        floating: true
      },
      title: {
        text: 'Top Findings',
        style: {
          color: baseColor,
        }
      },
      colors: ['#FF0000'],
    };
    this.effectRef = effect(() => {
      let primaryColor = getComputedStyle(document.documentElement).getPropertyValue('--primary');
      primaryColor = HSLToHex(primaryColor);
      this.chartOptions.tooltip = {
        theme: this.themeService.theme().mode,
      };
      this.chartOptions.title!.style!.color = primaryColor;
      if (this.themeService.isDark) {
        this.chartOptions.yaxis!.labels!.style!.colors = ['white'];
        this.chartOptions.xaxis!.labels!.style!.colors = ['white'];
        this.chartOptions.plotOptions!.bar!.dataLabels!.total!.style!.color = 'white';
      } else {
        this.chartOptions.yaxis!.labels!.style!.colors = ['black'];
        this.chartOptions.xaxis!.labels!.style!.colors = ['black'];
        this.chartOptions.plotOptions!.bar!.dataLabels!.total!.style!.color = 'black';
      }
    });
  }

  ngOnDestroy(): void {
    this.effectRef.destroy();
  }

  private effectRef: EffectRef;
}
