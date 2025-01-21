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
    this.chartOptions.series = [
      {
        name: 'count',
        data: value.map(item => item.count)
      }
    ];
  }

  chartOptions: Partial<ChartOptions>;

  constructor(
    private themeService: ThemeService
  ) {
    let primaryColor = HSLToHex(getComputedStyle(document.documentElement).getPropertyValue('--primary'));
    let bgColor = HSLToHex(getComputedStyle(document.documentElement).getPropertyValue('--background'));
    let textColor = this.themeService.isDark ? 'white' : 'black';
    this.chartOptions = {
      theme: {
        mode: this.themeService.isDark ? 'dark' : 'light',
      },
      chart: {
        type: 'bar',
        stacked: true,
        height: 450,
        background: bgColor
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
                fontWeight: 'normal',
                color: textColor
              }
            }
          }
        },
      },
      series: [],
      grid: {
        yaxis: {
          lines: {
            show: false,
            offsetY: 10,
          },
        },
        padding: {
          top: 10,
          bottom: 10
        }
      },
      xaxis: {
        categories: [],
        labels: {
          show: true,
          style: {
            colors: [textColor, textColor, textColor, textColor, textColor, textColor, textColor, textColor, textColor, textColor]
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
            colors: [textColor, textColor, textColor, textColor, textColor, textColor, textColor, textColor, textColor, textColor]
          }
        },
        floating: true
      },
      title: {
        text: 'Top Findings',
        style: {
          color: primaryColor,
        }
      },
      colors: ['#FF0000'],
    };
  }

  ngOnDestroy(): void {
  }

}
