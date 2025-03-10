import {Component, Input, OnDestroy} from '@angular/core';
import {TopDependency} from '../../../api/models/top-dependency';
import {UIChart} from 'primeng/chart';
import {Chart, ChartOptions} from 'chart.js';
import {DashboardStore} from '../dashboard.store';

@Component({
  selector: 'top-dependency-chart',
  standalone: true,
  imports: [
    UIChart
  ],
  templateUrl: './top-dependency-chart.component.html',
  styleUrl: './top-dependency-chart.component.scss'
})
export class TopDependencyChartComponent implements OnDestroy {
  @Input()
  set dependencies(value: TopDependency[]) {
    this.initChart(value);
  };

  data: any;
  option: ChartOptions = {};
  plugins: any[] = [];

  constructor(
    private store: DashboardStore
  ) {
  }

  initChart(input: TopDependency[]) {
    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = this.store.textColor();
    const surfaceBorder = this.store.borderColor();
    this.option = {
      maintainAspectRatio: true,
      indexAxis: "y",
      scales: {
        x: {
          border: {
            display: true,
            color: surfaceBorder
          },
          stacked: true,
          beginAtZero: true,
          ticks: {
            color: textColor,
          },
          grid: {
            display: false
          }
        },
        y: {
          border: {
            color: surfaceBorder
          },
          stacked: true,
          ticks: {
            display: false
          },
          grid: {
            display: false,
          },
        }
      },
      plugins: {
        legend: {
          display: false
        },
        title: {
          display: true,
          text: 'Top Vulnerable Package',
          color: textColor,
          font: {
            size: 18,
          }
        },
        datalabels: {
          display: false,
        }
      },
      onClick: (event: any, elements: any) => {
        const element = elements[0];
        const index = element.index;
        console.log(this.data.labels[index]);
      }
    };
    this.plugins = [
      {
        id: "top-vulnerable-package-plugin",
        beforeDatasetsDraw(chart: Chart, args: any, plugins: any) {
          for (var i = 0; i < 4; i++) {
            chart.getDatasetMeta(i).data.forEach((element, index) => {
              (element as any).height = 8;
            });
          }
          chart.getDatasetMeta(0).data.forEach((element, index) => {
            chart.ctx.fillStyle = textColor;
            chart.ctx.fillText(chart.data.labels![index] as any, 10, element.y + 14);
          });
        }
      }
    ];
    const labels = input.map(item => this.dependencyName(item));
    const critical = input.map(item => item.critical);
    const high = input.map(item => item.high);
    const medium = input.map(item => item.medium);
    const low = input.map(item => item.low);
    this.data = {
      labels: labels,
      datasets: [
        {
          label: 'critical',
          data: critical,
          backgroundColor: documentStyle.getPropertyValue('--p-red-500'),
        },
        {
          label: 'high',
          data: high,
          backgroundColor: documentStyle.getPropertyValue('--p-orange-500'),
        },
        {
          label: 'medium',
          data: medium,
          backgroundColor: documentStyle.getPropertyValue('--p-yellow-500'),
        },
        {
          label: 'low',
          data: low,
          backgroundColor: documentStyle.getPropertyValue('--p-green-500'),
        },
      ],
    }
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
