import {Component, inject, OnInit} from '@angular/core';
import {FindingStatusChartComponent} from '../../../../dashboard/finding-status-chart/finding-status-chart.component';
import {LowerCasePipe} from '@angular/common';
import {NgIcon} from '@ng-icons/core';
import {PackageStatusChartComponent} from '../../../../dashboard/package-status-chart/package-status-chart.component';
import {Panel} from 'primeng/panel';
import {SeverityChartComponent} from '../../../../dashboard/severity-chart/severity-chart.component';
import {finalize} from 'rxjs';
import {ProjectService} from '../../../../../api/services';
import {ProjectStore} from '../../project.store';
import {ProjectStatistics} from '../../../../../api/models/project-statistics';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'project-stats',
  imports: [
    FindingStatusChartComponent,
    LowerCasePipe,
    NgIcon,
    PackageStatusChartComponent,
    Panel,
    SeverityChartComponent,
    RouterLink
  ],
  templateUrl: './stats.component.html',
  styleUrl: './stats.component.scss'
})
export class StatsComponent implements OnInit {
  loading = false;
  statistic: ProjectStatistics = {
    severitySast: {
      critical: 0,
      high: 0,
      low: 0,
      medium: 0
    },
    severitySca: {
      critical: 0,
      high: 0,
      low: 0,
      medium: 0
    },
    statusSast: {
      acceptedRisk: 0,
      confirmed: 0,
      fixed: 0,
      open: 0
    },
    statusSca: {
      ignore: 0,
      fixed: 0,
      open: 0
    }
  };
  private projectService = inject(ProjectService);
  private store = inject(ProjectStore);
  project = this.store.project;

  ngOnInit(): void {

    this.loading = true;
    this.projectService.getProjectStatistic({
      projectId: this.store.projectId()
    }).pipe(
      finalize(() => {
        this.loading = false;
      })
    ).subscribe(response => {
      this.statistic = response;
    })
  }
}
