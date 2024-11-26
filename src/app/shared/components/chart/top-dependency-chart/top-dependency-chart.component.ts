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
      series: [],
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
        }
      },
      yaxis: {
        show: true,
        labels: {
          align: 'left',
          offsetX: 5,
          offsetY: 15,
          maxWidth: 1400,
          style: {
            colors: [baseColor],
          },
        },
        floating: true
      },
      title: {
        text: 'Top Vulnerable Packages',
        style: {
          color: baseColor,
        }
      },
      colors: ['#8F243D', '#DC1E27', '#FF8E3D', '#FFC800', '#bef264'],
    }
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

  private dependencyName(dependency: TopDependency) {
    if (dependency.group) {
      return `${dependency.group}.${dependency.name}@${dependency.version}`;
    } else {
      return `${dependency.name}@${dependency.version}`;
    }
  }

  ngOnDestroy(): void {
    this.effectRef.destroy();
  }

  private effectRef: EffectRef;
}
