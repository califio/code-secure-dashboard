import {Component, input, Input} from '@angular/core';
import {FindingStatusSeries} from './finding-status';
import {UIChart} from 'primeng/chart';
import {ChartOptions} from 'chart.js';
import {Router} from '@angular/router';
import {RangeDateState} from '../../../shared/ui/range-date/range-date.model';
import {FindingStatus} from '../../../api/models';

@Component({
  selector: 'finding-status-chart',
  standalone: true,
  imports: [
    UIChart
  ],
  templateUrl: './finding-status-chart.component.html',
})
export class FindingStatusChartComponent {
  rangeDate = input<RangeDateState>();

  @Input() set status(value: FindingStatusSeries) {
    this.initCharts(value);
  }
  findingStatus = [FindingStatus.Open, FindingStatus.Confirmed, FindingStatus.AcceptedRisk, FindingStatus.Fixed];
  option: ChartOptions = {};
  data: any;

  constructor(
    private router: Router
  ) {

  }

  initCharts(status: FindingStatusSeries) {
    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = documentStyle.getPropertyValue('--text-color');
    this.option = {
      plugins: {
        legend: {
          position: 'top',
          labels: {
            usePointStyle: true,
            color: textColor
          }
        },
        title: {
          display: true,
          text: 'Status',
          color: textColor,
          font: {
            size: 18,
          }
        },
        datalabels: {
          display: true,
          color: '#fff',
          font: {
            weight: 'bold',
            size: 14
          },
          formatter: (value: any) => {
            return value > 0 ? value : '';
          }
        }
      },
      onClick: (event: any, elements: any) => {
        if (elements.length > 0) {
          const index = elements[0].index;
          this.router.navigate(['/finding'], {
            queryParams: {
              status: [this.findingStatus[index]],
              createdAtRange: JSON.stringify(this.rangeDate())
            }
          }).then();
        }
      }
    };

    this.data = {
      labels: ['Open', 'Fixing', 'Accepted Risk', 'Fixed'],
      datasets: [
        {
          data: [status.open, status.confirmed, status.acceptedRisk, status.fixed],
          backgroundColor: [
            documentStyle.getPropertyValue('--p-gray-500'),
            documentStyle.getPropertyValue('--p-sky-500'),
            documentStyle.getPropertyValue('--p-orange-500'),
            documentStyle.getPropertyValue('--p-green-500')
          ],
          hoverBackgroundColor: [
            documentStyle.getPropertyValue('--p-gray-400'),
            documentStyle.getPropertyValue('--p-sky-400'),
            documentStyle.getPropertyValue('--p-orange-400'),
            documentStyle.getPropertyValue('--p-green-400')
          ]
        }
      ]
    };
  }
}
