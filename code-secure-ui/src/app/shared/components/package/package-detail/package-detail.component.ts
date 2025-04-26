import {Component, input, model} from '@angular/core';
import {Fieldset} from 'primeng/fieldset';
import {Panel} from 'primeng/panel';
import {ScanBranchLabelComponent} from '../../scan/scan-branch-label/scan-branch-label.component';
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
import {PackageStatusMenuComponent} from '../package-status-menu/package-status-menu.component';
import {ProjectService} from '../../../../api/services/project.service';
import {TicketMenuComponent} from '../../ticket-menu/ticket-menu.component';
import {ToastrService} from '../../../services/toastr.service';
import {finalize} from 'rxjs';

@Component({
  selector: 'package-detail',
  imports: [
    Fieldset,
    Panel,
    ScanBranchLabelComponent,
    RiskLevelIconComponent,
    PackageTypeComponent,
    ListPackageComponent,
    ListVulnerabilityComponent,
    Message,
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
    this.projectService.createProjectPackageTicket({
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
