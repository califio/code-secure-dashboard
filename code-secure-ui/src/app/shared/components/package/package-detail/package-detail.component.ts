import {Component, input, model} from '@angular/core';
import {ButtonDirective} from 'primeng/button';
import {DatePicker} from 'primeng/datepicker';
import {Divider} from 'primeng/divider';
import {Fieldset} from 'primeng/fieldset';
import {FindingActivityComponent} from '../../finding/finding-activity/finding-activity.component';
import {FindingSeverityComponent} from '../../finding/finding-severity/finding-severity.component';
import {FindingStatusLabelComponent} from '../../finding/finding-status-label/finding-status-label.component';
import {FloatLabel} from 'primeng/floatlabel';
import {LowerCasePipe} from '@angular/common';
import {MarkdownComponent} from 'ngx-markdown';
import {MarkdownEditorComponent} from '../../../ui/markdown-editor/markdown-editor.component';
import {NgIcon} from '@ng-icons/core';
import {Panel} from 'primeng/panel';
import {RouterLink} from '@angular/router';
import {ScanBranchLabelComponent} from '../../scan/scan-branch-label/scan-branch-label.component';
import {ScannerLabelComponent} from '../../scanner-label/scanner-label.component';
import {Select} from 'primeng/select';
import {RiskLevelIconComponent} from '../../risk-level-icon/risk-level-icon.component';
import {
  BranchStatusPackage,
  Packages,
  PackageStatus,
  RiskLevel,
  Tickets,
  TicketType,
  Vulnerabilities
} from '../../../../api/models';
import {PackageTypeComponent} from '../package-type/package-type.component';
import {ListPackageComponent} from '../list-package/list-package.component';
import {ListVulnerabilityComponent} from '../list-vulnerability/list-vulnerability.component';
import {Message} from 'primeng/message';
import {transformArrayNotNull} from '../../../../core/transform';
import {Chip} from 'primeng/chip';
import {PackageStatusComponent} from '../package-status/package-status.component';
import {PackageStatusMenuComponent} from '../package-status-menu/package-status-menu.component';
import {ProjectService} from '../../../../api/services/project.service';
import {TicketMenuComponent} from '../../ticket-menu/ticket-menu.component';
import {ToastrService} from '../../../services/toastr.service';
import {finalize} from 'rxjs';

const defaultValue: BranchStatusPackage[] = [];

@Component({
  selector: 'package-detail',
  imports: [
    ButtonDirective,
    DatePicker,
    Divider,
    Fieldset,
    FindingActivityComponent,
    FindingSeverityComponent,
    FindingStatusLabelComponent,
    FloatLabel,
    LowerCasePipe,
    MarkdownComponent,
    MarkdownEditorComponent,
    NgIcon,
    Panel,
    RouterLink,
    ScanBranchLabelComponent,
    ScannerLabelComponent,
    Select,
    RiskLevelIconComponent,
    PackageTypeComponent,
    ListPackageComponent,
    ListVulnerabilityComponent,
    Message,
    Chip,
    PackageStatusComponent,
    PackageStatusMenuComponent,
    TicketMenuComponent,
  ],
  templateUrl: './package-detail.component.html',
  standalone: true,
})
export class PackageDetailComponent {
  package = input<Packages>();
  dependencies = input([], {transform: transformArrayNotNull<Packages>});
  vulnerabilities = input([], {transform: transformArrayNotNull<Vulnerabilities>});
  // for package project
  projectId = input<string | null>();
  status = input<PackageStatus | null>();
  location = input<string | null>();
  branchStatus = model<BranchStatusPackage[] | null | undefined>();
  ticket = model<Tickets | null | undefined>();

  constructor(
    private projectService: ProjectService,
    private toastr: ToastrService
  ) {
  }

  packageName(pkg: Packages | null | undefined): string {
    if (pkg) {
      if (pkg.group) {
        return `${pkg.group}.${pkg.name}@${pkg.version}`;
      }
      return `${pkg.name}@${pkg.version}`;
    }
    return '';
  }

  protected readonly RiskLevel = RiskLevel;
  protected readonly PackageStatus = PackageStatus;
  loadingTicket = false;

  onChangeStatus($event: PackageStatus) {
    if (this.projectId()) {
      this.projectService.updateProjectPackage({
        projectId: this.projectId()!,
        packageId: this.package()?.id!,
        body: {
          status: $event
        }
      }).subscribe(projectPackage => {
        this.branchStatus.set(projectPackage.branchStatus);
      })
    }
  }

  onChangeBranchStatus($event: PackageStatus) {

  }


  createTicket(type: TicketType) {
    this.loadingTicket = true;
    this.projectService.createProjectTicket({
      projectId: this.projectId()!,
      packageId: this.package()?.id!,
      ticketType: type
    }).pipe(
      finalize(() => this.loadingTicket = false)
    ).subscribe(ticket => {
      this.ticket.set(ticket)
    })
  }

  deleteTicket() {
    this.projectService.deleteProjectTicket({
      projectId: this.projectId()!,
      packageId: this.package()?.id!
    }).subscribe(() => {
      this.ticket.set(null);
      this.toastr.success({
        message: 'Delete ticket success!'
      });
    })
  }
}
