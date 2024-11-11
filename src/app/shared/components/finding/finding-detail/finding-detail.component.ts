import {Component, EventEmitter, HostListener, Input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {FindingStatusComponent} from '../finding-status/finding-status.component';
import {RouterLink} from '@angular/router';
import {LowerCasePipe, NgClass} from '@angular/common';
import {FindingDetail} from '../../../../api/models/finding-detail';
import {FindingStatus} from '../../../../api/models/finding-status';
import {FindingService} from '../../../../api/services/finding.service';
import {ToastrService} from '../../toastr/toastr.service';
import {ScanBranchDropdownComponent} from '../../scan-branch-dropdown/scan-branch-dropdown.component';
import {FindingActivity} from '../../../../api/models/finding-activity';
import {FindingActivityType, FindingLocation, FindingScan, ProjectSource} from '../../../../api/models';
import {TimeagoModule} from 'ngx-timeago';
import {AvatarComponent} from '../../ui/avatar/avatar.component';
import {MarkdownComponent} from 'ngx-markdown';
import {FindingScanDropdownComponent} from '../finding-scan-dropdown/finding-scan-dropdown.component';

@Component({
  selector: 'finding-detail',
  standalone: true,
  imports: [
    NgIcon,
    FindingStatusComponent,
    RouterLink,
    NgClass,
    ScanBranchDropdownComponent,
    TimeagoModule,
    AvatarComponent,
    LowerCasePipe,
    MarkdownComponent,
    FindingScanDropdownComponent
  ],
  templateUrl: './finding-detail.component.html',
  styleUrl: './finding-detail.component.scss'
})
export class FindingDetailComponent {
  @Output()
  close = new EventEmitter();
  @Input()
  set finding(value: FindingDetail) {
    this._finding = value;
    this.currentScan = value.scans?.find(scan => scan.isDefault);
    if (!this.currentScan && value.scans && value.scans.length > 0) {
      this.currentScan = value.scans[0];
    }
    if (value.id) {
      this.findingService.getFindingActivities({
        id: value.id!,
        body: {}
      }).subscribe(activities => {
        this.activities = activities.items!;
      })
    }
  }
  currentScan: FindingScan | undefined;
  activities: FindingActivity[] = [];
  get finding() {
    return this._finding;
  }
  private _finding: FindingDetail = {};
  @Input()
  minimal = true;
  @Input()
  isProjectPage: boolean = false;

  constructor(
    private findingService: FindingService,
    private toastr: ToastrService
  ) {}

  onChangeStatus(status: FindingStatus) {
    this.findingService.updateFinding({
      id: this.finding.id!,
      body: {
        status: status
      }
    }).subscribe(success => {
      if (success) {
        this.toastr.success("update success");
      }
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
  protected readonly FindingActivityType = FindingActivityType;

  source(location: FindingLocation) {
    if (this._finding.project?.source == ProjectSource.GitLab) {
      return `${this._finding.project.repoUrl}/-/blob/${this.currentScan?.commitHash}/${location.path}#L${location.startLine}`;
    }
    // todo: support other git
    return '';
  }

  onScanChange(scanId: string) {
    this.currentScan = this.finding.scans?.find(value => value.scanId == scanId);
  }
}
