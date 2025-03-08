import {Component, Input} from '@angular/core';
import {PackageStatusSeries} from './package-status';
import {UIChart} from 'primeng/chart';

@Component({
  selector: 'package-status-chart',
  imports: [
    UIChart
  ],
  templateUrl: './package-status-chart.component.html',
  standalone: true,
})
export class PackageStatusChartComponent {
  @Input() set status(value: PackageStatusSeries) {
    this.initCharts(value);
  }

  option: any;
  data: any;

  initCharts(status: PackageStatusSeries) {
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
      labels: ['Open', 'Accepted Risk', 'Fixed'],
      datasets: [
        {
          data: [status.open, status.ignore, status.fixed],
          backgroundColor: [
            documentStyle.getPropertyValue('--p-gray-500'),
            documentStyle.getPropertyValue('--p-orange-500'),
            documentStyle.getPropertyValue('--p-green-500')
          ],
          hoverBackgroundColor: [
            documentStyle.getPropertyValue('--p-gray-400'),
            documentStyle.getPropertyValue('--p-orange-400'),
            documentStyle.getPropertyValue('--p-green-400')
          ]
        }
      ]
    };
  }
}
