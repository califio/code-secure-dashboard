import {Component, Input} from '@angular/core';
import {FindingStatusSeries} from './finding-status';
import {UIChart} from 'primeng/chart';

@Component({
  selector: 'finding-status-chart',
  standalone: true,
  imports: [
    UIChart
  ],
  templateUrl: './finding-status-chart.component.html',
})
export class FindingStatusChartComponent {
  @Input() set status(value: FindingStatusSeries) {
    this.initCharts(value);
  }

  option: any;
  data: any;

  constructor() {

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
