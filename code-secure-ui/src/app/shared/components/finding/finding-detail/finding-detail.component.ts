import {Component, effect, HostListener, input, Input, signal} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {RouterLink} from '@angular/router';
import {LowerCasePipe, NgClass} from '@angular/common';
import {FindingDetail} from '../../../../api/models/finding-detail';
import {FindingStatus} from '../../../../api/models/finding-status';
import {FindingService} from '../../../../api/services/finding.service';
import {ToastrService} from '../../../services/toastr.service';
import {FindingActivity} from '../../../../api/models/finding-activity';
import {FindingLocation, FindingScan, SourceType, TicketType} from '../../../../api/models';
import {TimeagoModule} from 'ngx-timeago';
import {MarkdownComponent} from 'ngx-markdown';
import {FindingSeverityComponent} from '../finding-severity/finding-severity.component';
import {FindingActivityComponent} from '../finding-activity/finding-activity.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {finalize} from 'rxjs';
import {TicketMenuComponent} from '../../ticket-menu/ticket-menu.component';
import {MarkdownEditorComponent} from '../../../ui/markdown-editor/markdown-editor.component';
import {ScanBranchLabelComponent} from '../../scan/scan-branch-label/scan-branch-label.component';
import {DatePicker} from 'primeng/datepicker';
import {FloatLabel} from 'primeng/floatlabel';
import {Fieldset} from 'primeng/fieldset';
import {ButtonDirective} from 'primeng/button';
import {ScannerLabelComponent} from '../../scanner-label/scanner-label.component';
import {TruncatePipe} from '../../../pipes/truncate.pipe';
import {Tooltip} from 'primeng/tooltip';
import {BranchFilterComponent, BranchOption} from '../../branch-filter/branch-filter.component';
import {FindingStatusMenuComponent} from '../finding-status-menu/finding-status-menu.component';
import {Skeleton} from 'primeng/skeleton';

@Component({
  selector: 'finding-detail',
  standalone: true,
  imports: [
    NgIcon,
    RouterLink,
    NgClass,
    TimeagoModule,
    LowerCasePipe,
    MarkdownComponent,
    FindingSeverityComponent,
    FindingActivityComponent,
    ReactiveFormsModule,
    FormsModule,
    TicketMenuComponent,
    MarkdownEditorComponent,
    ButtonDirective,
    ScanBranchLabelComponent,
    DatePicker,
    FloatLabel,
    Fieldset,
    ButtonDirective,
    ScannerLabelComponent,
    TruncatePipe,
    Tooltip,
    BranchFilterComponent,
    FindingStatusMenuComponent,
    FindingStatusMenuComponent,
    Skeleton,
  ],
  templateUrl: './finding-detail.component.html',
  styleUrl: 'finding-detail.component.scss'
})
export class FindingDetailComponent {
  findingId = input<string>();
  finding: FindingDetail | null = null;
  fixDeadline = signal<Date | undefined | null>(undefined);
  currentScan = signal<FindingScan | undefined | null>(undefined);
  dateFormat = 'dd/mm/yy';
  activities: FindingActivity[] = [];
  @Input()
  minimal = true;
  @Input()
  isProjectPage: boolean = false;
  comment = '';
  recommendationPreview = true;
  branchOptions: BranchOption[] = [];
  loading = {
    finding: false,
    activity: false,
    comment: false,
    ticket: false,
    recommendation: false
  }

  constructor(
    private findingService: FindingService,
    private toastr: ToastrService
  ) {
    effect(() => {
      const findingId = this.findingId();
      if (findingId) {
        this.finding = null;
        // load finding
        this.loading.finding = true;
        this.findingService.getFindingById({
          findingId: findingId
        }).pipe(
          finalize(() => this.loading.finding = false)
        ).subscribe(finding => {
          this.finding = finding;
          if (finding.fixDeadline) {
            this.fixDeadline.set(new Date(finding.fixDeadline));
          } else {
            this.fixDeadline.set(undefined);
          }
          // scan
          if (finding.scans) {
            let defaultScan = finding.scans.find(scan => scan.isDefault);
            if (!defaultScan && finding.scans.length > 0) {
              defaultScan = finding.scans[0];
            }
            this.currentScan.set(defaultScan);
            this.branchOptions = finding.scans.map(item => {
              return <BranchOption>{
                commitType: item.action,
                commitBranch: item.branch,
                targetBranch: item.targetBranch,
                id: item.scanId
              }
            });
          }
        })
        this.loadActivities();
      }
    });
  }

  onChangeStatus(status: FindingStatus) {
    this.findingService.updateFinding({
      findingId: this.findingId()!,
      body: {
        status: status
      }
    }).subscribe(finding => {
      this.finding = finding;
      this.toastr.success({
        message: "Update success"
      });
      this.loadActivities();
    });
  }

  @HostListener('window:resize', ['$event'])
  getScreenSize() {
    if (!this.isProjectPage) {
      this.minimal = window.innerWidth < 1024;
    }
  }

  findingFlow(finding: FindingDetail): FindingLocation[] {
    if (finding.metadata && finding.metadata.findingFlow) {
      if (finding.metadata.findingFlow.length > 0) {
        return finding.metadata.findingFlow;
      }
    }
    return [];
  }

  findingLocation(location: FindingLocation, commitSha: string | null | undefined = undefined) {
    if (!commitSha) {
      commitSha = this.currentScan()?.commitHash;
    }
    if (this.finding!.project?.sourceType == SourceType.GitLab) {
      return `${this.finding!.project!.repoUrl}/-/blob/${commitSha}/${location.path}#L${location.startLine ?? '1'}`;
    }
    // todo: support other git
    return '';
  }

  onScanChange(scanId: string) {
    this.currentScan.set(this.finding!.scans?.find(value => value.scanId == scanId));
  }

  private loadActivities() {
    this.findingService.getFindingActivities({
      findingId: this.findingId()!,
      body: {}
    }).subscribe(activities => {
      this.activities = activities.items!;
    })
  }

  protected readonly FindingStatus = FindingStatus;

  onChangeFixDeadline($event: Date) {
    this.findingService.updateFinding({
      findingId: this.findingId()!,
      body: {
        fixDeadline: $event.toISOString()
      }
    }).subscribe(finding => {
      this.toastr.success({message: 'Change deadline success!'});
      this.finding = finding;
    });
  }

  postComment() {
    if (this.comment) {
      this.loading.comment = true;
      this.findingService.addComment({
        findingId: this.findingId()!,
        body: {
          comment: this.comment
        }
      }).pipe(
        finalize(() => this.loading.comment = false)
      ).subscribe(commentActivity => {
        const activities = [commentActivity];
        activities.push(...this.activities);
        this.activities = activities;
        this.comment = '';
        this.toastr.success({message: 'Add comment success!'});
      });
    }
  }

  createTicket(type: TicketType) {
    this.loading.ticket = true;
    this.findingService.createTicket({
      findingId: this.findingId()!,
      type: type
    }).pipe(
      finalize(() => this.loading.ticket = false)
    ).subscribe(ticket => {
      this.finding!.ticket = ticket;
    })
  }

  deleteTicket() {
    this.findingService.deleteTicket({
      findingId: this.findingId()!
    }).subscribe(() => {
      this.finding!.ticket = undefined;
    })
  }

  saveRecommendation() {
    this.loading.recommendation = true;
    this.findingService.updateFinding({
      findingId: this.findingId()!,
      body: {
        recommendation: this.finding!.recommendation
      }
    }).pipe(
      finalize(() => this.loading.recommendation = false)
    ).subscribe(() => {
      this.toastr.success({message: 'Update recommendation success!'});
    })
  }

  onChangeScanStatus(scanId: string, $event: FindingStatus) {
    this.findingService.updateStatusScanFinding({
      findingId: this.findingId()!,
      body: {
        scanId: scanId,
        status: $event
      }
    }).subscribe(() => {
      this.toastr.success({
        message: 'Update status success!'
      });
    })
  }
}
