import {Component, computed, inject, OnInit, signal} from '@angular/core';
import {ProjectCommitFilter} from '../../../../../api/models/project-commit-filter';
import {ProjectCommitScanSummary} from '../../../../../api/models/project-commit-scan-summary';
import {finalize} from 'rxjs';
import {ProjectService} from '../../../../../api/services/project.service';
import {ProjectStore} from '../../project.store';
import {Checkbox} from 'primeng/checkbox';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {NgIcon} from '@ng-icons/core';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {Panel} from 'primeng/panel';
import {
  ScanBranchLabelComponent
} from '../../../../../shared/components/scan/scan-branch-label/scan-branch-label.component';
import {ScanStatusComponent} from '../../../../../shared/components/scan/scan-status/scan-status.component';
import {ScannerLabelComponent} from '../../../../../shared/components/scanner-label/scanner-label.component';
import {TableModule} from 'primeng/table';
import {TimeagoModule} from 'ngx-timeago';
import {Tooltip} from 'primeng/tooltip';
import {TruncatePipe} from '../../../../../shared/pipes/truncate.pipe';
import {FindingStatus} from '../../../../../api/models/finding-status';
import {ScanStatus} from '../../../../../api/models/scan-status';
import {CommitType} from '../../../../../api/models/commit-type';
import {RouterLink} from '@angular/router';
import {cast} from '../../../../../core/transform';
import {LayoutService} from '../../../../../layout/layout.service';
import {FormsModule} from '@angular/forms';
import {FindingSeverity} from '../../../../../api/models/finding-severity';

@Component({
  selector: 'list-commit',
  imports: [
    Checkbox,
    IconField,
    InputIcon,
    InputText,
    NgIcon,
    Paginator,
    Panel,
    ScanBranchLabelComponent,
    ScanStatusComponent,
    ScannerLabelComponent,
    TableModule,
    TimeagoModule,
    Tooltip,
    TruncatePipe,
    RouterLink,
    FormsModule
  ],
  templateUrl: './commit.component.html',
  styleUrl: './commit.component.scss'
})
export class CommitComponent implements OnInit {
  loading = false;
  commits = signal<ProjectCommitScanSummary[]>([]);
  filter: ProjectCommitFilter = {
    size: 20,
    page: 1,
    desc: true,
    name: null,
  }
  currentPage = signal(1);
  pageSize = signal(20);
  totalRecords = signal(0);
  firstRecord = computed(() => {
    return (this.currentPage() - 1) * this.pageSize();
  });
  private projectService = inject(ProjectService);
  private store = inject(ProjectStore);
  projectId = this.store.projectId;
  isDesktop = inject(LayoutService).isDesktop();
  ngOnInit(): void {
    this.loadCommit();
  }
  private loadCommit() {
    this.loading = true;
    this.projectService.getProjectCommitScanSummary({
      projectId: this.store.projectId(),
      body: this.filter
    }).pipe(
      finalize(() => {
        this.loading = false
      })
    ).subscribe(result => {
      this.currentPage.set(this.filter.page!);
      this.pageSize.set(this.filter.size!);
      this.commits.set(result.items!);
      this.totalRecords.set(result.count!);
    });
  }

  totalFindingByStatus(commit: ProjectCommitScanSummary, status: FindingStatus): number {
    var total = 0;
    commit.scans?.forEach(scan => {
      switch (status) {
        case FindingStatus.Open:
          total += scan.open ?? 0;
          break;
        case FindingStatus.AcceptedRisk:
          total += scan.ignored ?? 0;
          break;
        case FindingStatus.Fixed:
          total += scan.fixed ?? 0;
          break;
        case FindingStatus.Confirmed:
          total += scan.confirmed ?? 0;
          break;
      }
    });
    return total;
  }

  totalFindingBySeverity(commit: ProjectCommitScanSummary, severity: FindingSeverity): number {
    var total = 0;
    commit.scans?.forEach(scan => {
      switch (severity) {
        case FindingSeverity.Critical:
          total += scan.critical ?? 0;
          break;
        case FindingSeverity.High:
          total += scan.high ?? 0;
          break;
        case FindingSeverity.Medium:
          total += scan.medium ?? 0;
          break;
        case FindingSeverity.Low:
          total += scan.low ?? 0;
          break;
      }
    });
    return total;
  }


  startScan(commit: ProjectCommitScanSummary) {
    const dates = commit.scans!
      .filter(item => item.started != null)
      .map(item => new Date(item.started!).getTime());
    if (dates.length > 0) {
      return new Date(Math.min(...dates)).toISOString()
    }
    return "-"
  }

  duration(start?: string, end?: string | null): string {
    if (end == null || start == null) {
      return "-";
    }
    const startDate = new Date(start);
    const endDate = new Date(end);
    const durationInMs = endDate.getTime() - startDate.getTime();
    const seconds = Math.floor((durationInMs / 1000) % 60);
    const minutes = Math.floor((durationInMs / (1000 * 60)) % 60);
    const hours = Math.floor((durationInMs / (1000 * 60 * 60)) % 24);
    const days = Math.floor(durationInMs / (1000 * 60 * 60 * 24));
    const result = [];
    if (days > 0) result.push(`${days} days`);
    if (hours > 0) result.push(`${hours} hours`);
    if (minutes > 0) result.push(`${minutes} minutes`);
    if (seconds > 0) result.push(`${seconds} seconds`);
    return result.length > 0 ? result.join(", ") : "0 seconds";
  }

  onPageChange($event: PaginatorState) {
    this.filter.page = $event.page;
    this.loadCommit();
  }
  onSearchChange() {
    this.filter.name = this.search;
    this.loadCommit();
  }
  protected readonly FindingStatus = FindingStatus;
  protected readonly ScanStatus = ScanStatus;
  protected readonly CommitType = CommitType;
  protected readonly cast = cast<ProjectCommitScanSummary>;
  search = '';
  protected readonly FindingSeverity = FindingSeverity;
}
