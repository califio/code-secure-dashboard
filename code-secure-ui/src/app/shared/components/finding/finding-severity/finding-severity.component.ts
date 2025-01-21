import {Component, Input} from '@angular/core';
import {FindingSeverity} from '../../../../api/models/finding-severity';
import {NgIcon} from '@ng-icons/core';
import {LowerCasePipe, NgClass} from '@angular/common';
import {TooltipDirective} from '../../../ui/tooltip/tooltip.directive';

@Component({
  selector: 'finding-severity',
  standalone: true,
  imports: [
    NgIcon,
    NgClass,
    TooltipDirective,
    LowerCasePipe
  ],
  templateUrl: './finding-severity.component.html',
  styleUrl: './finding-severity.component.scss'
})
export class FindingSeverityComponent {
  @Input()
  severity: FindingSeverity = FindingSeverity.Low;

  style(): string {
    if (this.severity == FindingSeverity.Critical) {
      return 'bg-critical';
    }
    if (this.severity == FindingSeverity.High) {
      return 'bg-high';
    }
    if (this.severity == FindingSeverity.Medium) {
      return 'bg-medium';
    }
    if (this.severity == FindingSeverity.Low) {
      return 'bg-low';
    }
    return 'bg-info';
  }
  severityLabel() {
    if (this.severity && this.severity.length > 0) {
      return this.severity.charAt(0);
    }
    return 'U';
  }
}
