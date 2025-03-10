import {Component, input, Input} from '@angular/core';
import {TopFinding} from '../../../api/models/top-finding';
import {UIChart} from 'primeng/chart';
import {Chart, ChartOptions} from 'chart.js';
import {DashboardStore} from '../dashboard.store';
import {Router} from '@angular/router';
import {RangeDateState} from '../../../shared/ui/range-date/range-date.model';
import {FindingStatus} from '../../../api/models';

@Component({
  selector: 'top-finding-chart',
  standalone: true,
  imports: [
    UIChart
  ],
  templateUrl: './top-finding-chart.component.html',
})
export class TopFindingChartComponent {
  rangeDate = input<RangeDateState>();
  data: any;
  // option: ChartOptions = {};
  option: ChartOptions = {};
  plugins: any[] = [];

  @Input()
  set categories(value: TopFinding[]) {
    this.initChart(value);
  }

  constructor(
    private store: DashboardStore,
    private router: Router
  ) {
  }

  initChart(input: TopFinding[]) {
    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = this.store.textColor();
    const surfaceBorder = this.store.borderColor();
    this.plugins = [
      {
        id: "top-finding-plugin",
        beforeDatasetsDraw(chart: Chart, args: any, plugins: any) {
          chart.ctx.save();
          chart.getDatasetMeta(0).data.forEach((element, index) => {
            (element as any).height = 8;
            chart.ctx.fillStyle = textColor;
            chart.ctx.fillText(chart.data.labels![index] as any, 10, element.y + 14);
          });
        }
      }
    ];
    this.option = {
      maintainAspectRatio: true,
      indexAxis: "y",
      scales: {
        x: {
          border: {
            color: surfaceBorder
          },
          beginAtZero: true,
          ticks: {
            color: textColor,
          },
          grid: {
            display: false,
          }
        },
        y: {
          border: {
            color: surfaceBorder
          },
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
          text: 'Top Finding',
          color: textColor,
          font: {
            size: 18,
          }
        },
        datalabels: {
          align: 'end',
          anchor: 'end',
          color: textColor,
        },
      },
      onClick: (event: any, elements: any) => {
        if (elements.length > 0) {
          const index = elements[0].index;
          const category = this.data.labels[index];
          this.router.navigate(['/finding'], {
            queryParams: {
              category: category,
              status: [FindingStatus.Open, FindingStatus.Confirmed, FindingStatus.AcceptedRisk, FindingStatus.Fixed],
              createdAtRange: JSON.stringify(this.rangeDate())
            }
          }).then();
        }
      }
    };

    let categories = input.map(item => item.category);
    const count = input.map(item => item.count);
    this.data = {
      labels: categories,
      datasets: [
        {
          label: ["Count"],
          data: count,
          backgroundColor: [documentStyle.getPropertyValue('--p-red-500')],
        },
      ],
    }
  }
}
