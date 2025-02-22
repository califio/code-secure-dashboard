import {Component, Input} from '@angular/core';
import {Severity} from './severity';
import {UIChart} from 'primeng/chart';

@Component({
  selector: 'severity-chart',
  standalone: true,
  imports: [
    UIChart
  ],
  templateUrl: './severity-chart.component.html',
})
export class SeverityChartComponent {

  @Input() set severity(value: Severity) {
    this.initCharts(value);
  }

  option: any;
  data: any;

  constructor() {
  }

  initCharts(severity: Severity) {
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
          text: 'Severity',
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
      labels: ['Critical', 'High', 'Medium', 'Low'],
      datasets: [
        {
          data: [severity.critical, severity.high, severity.medium, severity.low],
          backgroundColor: [
            documentStyle.getPropertyValue('--p-red-500'),
            documentStyle.getPropertyValue('--p-orange-500'),
            documentStyle.getPropertyValue('--p-yellow-500'),
            documentStyle.getPropertyValue('--p-sky-500')
          ],
          hoverBackgroundColor: [
            documentStyle.getPropertyValue('--p-red-400'),
            documentStyle.getPropertyValue('--p-orange-400'),
            documentStyle.getPropertyValue('--p-yellow-400'),
            documentStyle.getPropertyValue('--p-sky-400')
          ]
        }
      ]
    };
  }
}
