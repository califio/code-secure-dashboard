import {Component, EventEmitter, HostListener, Input, Output, signal} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {RouterLink} from '@angular/router';
import {LowerCasePipe, NgClass} from '@angular/common';
import {FindingDetail} from '../../../../api/models/finding-detail';
import {FindingStatus} from '../../../../api/models/finding-status';
import {FindingService} from '../../../../api/services/finding.service';
import {ToastrService} from '../../../services/toastr.service';
import {FindingActivity} from '../../../../api/models/finding-activity';
import {
  FindingLocation,
  FindingScan,
  GitAction,
  ScannerType,
  SourceType,
  Tickets,
  TicketType
} from '../../../../api/models';
import {TimeagoModule} from 'ngx-timeago';
import {MarkdownComponent} from 'ngx-markdown';
import {FindingSeverityComponent} from '../finding-severity/finding-severity.component';
import {FindingActivityComponent} from '../finding-activity/finding-activity.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {finalize} from 'rxjs';
import {TicketMenuComponent} from '../../ticket-menu/ticket-menu.component';
import {MarkdownEditorComponent} from '../../../ui/markdown-editor/markdown-editor.component';
import {ScanBranchLabelComponent} from '../../scan/scan-branch-label/scan-branch-label.component';
import {Select} from 'primeng/select';
import {Panel} from 'primeng/panel';
import {DatePicker} from 'primeng/datepicker';
import {FloatLabel} from 'primeng/floatlabel';
import {Divider} from 'primeng/divider';
import {Fieldset} from 'primeng/fieldset';
import {ButtonDirective} from 'primeng/button';
import {FindingStatusLabelComponent} from '../finding-status-label/finding-status-label.component';
import {getFindingStatusOptions} from '../finding-status';
import {ScannerLabelComponent} from '../../scanner-label/scanner-label.component';
import {TruncatePipe} from '../../../pipes/truncate.pipe';
import {Tooltip} from 'primeng/tooltip';

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
    Select,
    Panel,
    DatePicker,
    FloatLabel,
    Divider,
    Fieldset,
    ButtonDirective,
    FindingStatusLabelComponent,
    ScannerLabelComponent,
    TruncatePipe,
    Tooltip,
  ],
  templateUrl: './finding-detail.component.html',
})
export class FindingDetailComponent {
  @Output()
  close = new EventEmitter();

  @Input()
  set finding(value: FindingDetail) {
    this._finding = value;
    this.ticket.set(value.ticket);
    this.fixDeadline.set(this.parseFixDeadline(value.fixDeadline));
    let defaultScan = value.scans?.find(scan => scan.isDefault);
    if (!defaultScan && value.scans && value.scans.length > 0) {
      defaultScan = value.scans[0];
    }
    this.currentScan.set(defaultScan);
    if (value.id) {
      this.loadActivities();
    }
  }

  dateFormat = 'dd/mm/yy';
  currentScan = signal<FindingScan | undefined>(undefined);
  activities: FindingActivity[] = [];

  get finding() {
    return this._finding;
  }

  private _finding: FindingDetail = {project: {}, scans: []};
  @Input()
  minimal = true;
  @Input()
  isProjectPage: boolean = false;
  fixDeadline = signal<Date | null>(null);
  ticket = signal<Tickets | undefined>(undefined);
  comment = '';
  commentLoading = false;
  loadingTicket = false;
  recommendationPreview = true;
  recommendationLoading = false;

  constructor(
    private findingService: FindingService,
    private toastr: ToastrService
  ) {
  }

  onChangeStatus(status: FindingStatus) {
    this.findingService.updateFinding({
      id: this.finding.id!,
      body: {
        status: status
      }
    }).subscribe(finding => {
      this.finding = finding;
      this.toastr.success({
        message: "Update success"
      });
      this.loadActivities();
    })
  }

  closeFinding() {
    this.close.emit();
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

  source(location: FindingLocation) {
    if (this._finding.project?.sourceType == SourceType.GitLab) {
      return `${this._finding.project!.repoUrl}/-/blob/${this.currentScan()?.commitHash}/${location.path}#L${location.startLine ?? '1'}`;
    }
    // todo: support other git
    return '';
  }

  onScanChange(scanId: string) {
    this.currentScan.set(this.finding.scans?.find(value => value.scanId == scanId));
  }

  private loadActivities() {
    this.findingService.getFindingActivities({
      id: this._finding.id!,
      body: {}
    }).subscribe(activities => {
      this.activities = activities.items!;
    })
  }

  protected readonly GitAction = GitAction;
  protected readonly FindingStatus = FindingStatus;

  onChangeFixDeadline($event: Date) {
    const currentFixDeadline = this.fixDeadline();
    this.fixDeadline.set($event);
    this.findingService.updateFinding({
      id: this._finding.id!,
      body: {
        fixDeadline: $event.toISOString()
      }
    }).subscribe({
      next: (finding) => {
        this.toastr.success({message: 'Change deadline success!'});
        this.finding = finding;
        this.loadActivities();
      },
      error: () => {
        this.fixDeadline.set(currentFixDeadline);
      }
    });
  }

  private parseFixDeadline(date: string | null | undefined): Date | null {
    if (date) {
      return new Date(date);
    }
    return null;
  }

  postComment() {
    if (this.comment) {
      this.commentLoading = true;
      this.findingService.addComment({
        id: this.finding.id!,
        body: {
          comment: this.comment
        }
      }).pipe(
        finalize(() => this.commentLoading = false)
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
    this.loadingTicket = true;
    this.findingService.createTicket({
      id: this.finding.id!,
      type: type
    }).pipe(
      finalize(() => this.loadingTicket = false)
    ).subscribe(ticket => {
      this.ticket.set(ticket);
    })
  }

  isSastFinding() {
    if (this.finding) {
      return this.finding.type == ScannerType.Sast || this.finding.type == ScannerType.Secret;
    }
    return false;
  }

  deleteTicket() {
    this.findingService.deleteTicket({
      id: this.finding.id!
    }).subscribe(() => {
      this.ticket.set(undefined);
    })
  }

  saveRecommendation() {
    this.recommendationLoading = true;
    this.findingService.updateFinding({
      id: this.finding.id!,
      body: {
        recommendation: this.finding.recommendation
      }
    }).pipe(
      finalize(() => this.recommendationLoading = false)
    ).subscribe(() => {
      this.toastr.success({message: 'Update recommendation success!'});
    })
  }

  statusOptions = getFindingStatusOptions();
}
