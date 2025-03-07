import {Component, input} from '@angular/core';
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
  RiskImpact,
  RiskLevel,
  Vulnerabilities
} from '../../../../api/models';
import {PackageTypeComponent} from '../package-type/package-type.component';
import {ListPackageComponent} from '../list-package/list-package.component';
import {ListVulnerabilityComponent} from '../list-vulnerability/list-vulnerability.component';
import {Message} from 'primeng/message';
import {arrayNotNull} from '../../../../core/transform';
import {Chip} from 'primeng/chip';
import {PackageStatusComponent} from '../package-status/package-status.component';

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
  ],
  templateUrl: './package-detail.component.html',
  standalone: true,
})
export class PackageDetailComponent {
  package = input<Packages>();
  dependencies = input([], {transform: arrayNotNull<Packages>});
  vulnerabilities = input([], {transform: arrayNotNull<Vulnerabilities>});
  // for package project
  location = input<string | null>();
  branchStatus = input(defaultValue, {transform: arrayNotNull<BranchStatusPackage>});

  packageName(pkg: Packages | null | undefined): string {
    if (pkg) {
      if (pkg.group) {
        return `${pkg.group}.${pkg.name}@${pkg.version}`;
      }
      return `${pkg.name}@${pkg.version}`;
    }
    return '';
  }

  protected readonly RiskImpact = RiskImpact;
  protected readonly RiskLevel = RiskLevel;
  protected readonly PackageStatus = PackageStatus;
}
