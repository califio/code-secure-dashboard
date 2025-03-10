import {Component, input, Input} from '@angular/core';
import {Severity} from './severity';
import {UIChart} from 'primeng/chart';
import {RangeDateState} from '../../../shared/ui/range-date/range-date.model';
import {Router} from '@angular/router';
import {FindingSeverity} from '../../../api/models';

@Component({
  selector: 'severity-chart',
  standalone: true,
  imports: [
    UIChart
  ],
  templateUrl: './severity-chart.component.html',
})
export class SeverityChartComponent {
  rangeDate = input<RangeDateState>();
  type = input<'sast' | 'sca'>('sast');
  @Input() set severity(value: Severity) {
    this.initCharts(value);
  }
  findingSeverity = [FindingSeverity.Critical, FindingSeverity.High, FindingSeverity.Medium, FindingSeverity.Low, FindingSeverity.Info]
  option: any;
  data: any;

  constructor(
    private router: Router
  ) {
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
      },
      onClick: (event: any, elements: any) => {
        if (elements.length > 0) {
          if (this.type() == "sast"){
            const index = elements[0].index;
            this.router.navigate(['/finding'], {
              queryParams: {
                severity: [this.findingSeverity[index]],
                createdAtRange: JSON.stringify(this.rangeDate())
              }
            }).then();
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
