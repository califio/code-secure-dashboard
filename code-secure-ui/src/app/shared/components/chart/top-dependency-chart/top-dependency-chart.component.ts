import {Component, effect, EffectRef, Input, OnDestroy} from '@angular/core';
import {ChartComponent} from "ng-apexcharts";
import {ThemeService} from '../../../../core/theme';
import {TopDependency} from '../../../../api/models/top-dependency';
import {ChartOptions, HSLToHex} from '../char-options';

@Component({
  selector: 'top-dependency-chart',
  standalone: true,
  imports: [
    ChartComponent
  ],
  templateUrl: './top-dependency-chart.component.html',
  styleUrl: './top-dependency-chart.component.scss'
})
export class TopDependencyChartComponent implements OnDestroy {
  @Input()
  set dependencies(value: TopDependency[]) {
    this.chartOptions.xaxis!.categories = value.map(item => this.dependencyName(item));
    this.chartOptions.series = [];
    this.chartOptions.series!.push({
      name: 'critical',
      data: value.map(item => item.critical!)
    });
    this.chartOptions.series.push({
      name: 'high',
      data: value.map(item => item.high!)
    });
    this.chartOptions.series.push({
      name: 'medium',
      data: value.map(item => item.medium!)
    });
    this.chartOptions.series.push({
      name: 'low',
      data: value.map(item => item.low!)
    });
    this.chartOptions.series.push({
      name: 'info',
      data: value.map(item => item.info!)
    });
  };

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
      legend: {
        show: false
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
        }
      },
      xaxis: {
        categories: [],
        labels: {
          show: true,
          style: {
            colors: [textColor, textColor, textColor]
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
        text: 'Top Vulnerable Packages',
        style: {
          color: primaryColor,
        }
      },
      colors: ['#8F243D', '#DC1E27', '#FF8E3D', '#FFC800', '#bef264'],
    };
  }

  private dependencyName(dependency: TopDependency) {
    if (dependency.group) {
      return `${dependency.group}.${dependency.name}@${dependency.version}`;
    } else {
      return `${dependency.name}@${dependency.version}`;
    }
  }

  ngOnDestroy(): void {
  }
}
