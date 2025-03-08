import {
  Component,
  effect,
  EffectRef,
  EventEmitter,
  HostListener,
  Input,
  model,
  OnDestroy,
  Output,
  signal
} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {RouterLink} from '@angular/router';
import {LowerCasePipe, NgClass} from '@angular/common';
import {FindingDetail} from '../../../../api/models/finding-detail';
import {FindingStatus} from '../../../../api/models/finding-status';
import {FindingService} from '../../../../api/services/finding.service';
import {ToastrService} from '../../../services/toastr.service';
import {FindingActivity} from '../../../../api/models/finding-activity';
import {FindingLocation, FindingScan, ScannerType, SourceType, Tickets, TicketType} from '../../../../api/models';
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
import {ScannerLabelComponent} from '../../scanner-label/scanner-label.component';
import {TruncatePipe} from '../../../pipes/truncate.pipe';
import {Tooltip} from 'primeng/tooltip';
import {BranchFilterComponent, BranchOption} from '../../branch-filter/branch-filter.component';
import {FindingStatusSelectComponent} from '../finding-status-select/finding-status-select.component';
import {FindingStatusMenuComponent} from '../finding-status-menu/finding-status-menu.component';

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
    BranchFilterComponent,
    FindingStatusSelectComponent,
    FindingStatusMenuComponent,
    FindingStatusMenuComponent,
  ],
  templateUrl: './finding-detail.component.html',
  styleUrl: 'finding-detail.component.scss'
})
export class FindingDetailComponent implements OnDestroy {
  @Output()
  close = new EventEmitter();
  finding = model<FindingDetail>({});
  ticket = signal<Tickets | undefined>(undefined);
  fixDeadline = signal<Date | undefined | null>(undefined);
  currentScan = signal<FindingScan | undefined | null>(undefined);
  dateFormat = 'dd/mm/yy';
  activities: FindingActivity[] = [];
  @Input()
  minimal = true;
  @Input()
  isProjectPage: boolean = false;
  comment = '';
  commentLoading = false;
  loadingTicket = false;
  recommendationPreview = true;
  recommendationLoading = false;
  private effectRef!: EffectRef;
  branchOptions: BranchOption[] = [];

  constructor(
    private findingService: FindingService,
    private toastr: ToastrService
  ) {
    this.effectRef = effect(() => {
      const finding = this.finding();
      this.ticket.set(finding.ticket);
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
      if (finding.id) {
        this.loadActivities();
      }
    });
  }

  ngOnDestroy(): void {
    this.effectRef.destroy();
  }

  onChangeStatus(status: FindingStatus) {
    this.findingService.updateFinding({
      id: this.finding().id!,
      body: {
        status: status
      }
    }).subscribe(finding => {
      this.finding.set(finding);
      this.toastr.success({
        message: "Update success"
      });
      this.loadActivities();
    });
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

  findingLocation(location: FindingLocation, commitSha: string | null | undefined = undefined) {
    if (!commitSha) {
      commitSha = this.currentScan()?.commitHash;
    }
    if (this.finding().project?.sourceType == SourceType.GitLab) {
      return `${this.finding().project!.repoUrl}/-/blob/${commitSha}/${location.path}#L${location.startLine ?? '1'}`;
    }
    // todo: support other git
    return '';
  }

  onScanChange(scanId: string) {
    this.currentScan.set(this.finding().scans?.find(value => value.scanId == scanId));
  }

  private loadActivities() {
    this.findingService.getFindingActivities({
      id: this.finding().id!,
      body: {}
    }).subscribe(activities => {
      this.activities = activities.items!;
    })
  }

  protected readonly FindingStatus = FindingStatus;

  onChangeFixDeadline($event: Date) {
    this.findingService.updateFinding({
      id: this.finding().id!,
      body: {
        fixDeadline: $event.toISOString()
      }
    }).subscribe(finding => {
      this.toastr.success({message: 'Change deadline success!'});
      this.finding.set(finding);
    });
  }

  postComment() {
    if (this.comment) {
      this.commentLoading = true;
      this.findingService.addComment({
        id: this.finding().id!,
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
      id: this.finding().id!,
      type: type
    }).pipe(
      finalize(() => this.loadingTicket = false)
    ).subscribe(ticket => {
      this.ticket.set(ticket);
    })
  }

  isSastFinding() {
    if (this.finding()) {
      return this.finding().type == ScannerType.Sast || this.finding().type == ScannerType.Secret;
    }
    return false;
  }

  deleteTicket() {
    this.findingService.deleteTicket({
      id: this.finding().id!
    }).subscribe(() => {
      this.ticket.set(undefined);
    })
  }

  saveRecommendation() {
    this.recommendationLoading = true;
    this.findingService.updateFinding({
      id: this.finding().id!,
      body: {
        recommendation: this.finding().recommendation
      }
    }).pipe(
      finalize(() => this.recommendationLoading = false)
    ).subscribe(() => {
      this.toastr.success({message: 'Update recommendation success!'});
    })
  }

  onChangeScanStatus(scanId: string, $event: FindingStatus) {
    this.findingService.updateStatusScanFinding({
      findingId: this.finding().id!,
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
