import {Component, input, Input} from '@angular/core';
import {FindingSeverity} from '../../../../api/models/finding-severity';
import {LowerCasePipe, NgClass} from '@angular/common';
import {Tooltip} from 'primeng/tooltip';

@Component({
  selector: 'finding-severity',
  standalone: true,
  imports: [
    NgClass,
    LowerCasePipe,
    Tooltip
  ],
  templateUrl: './finding-severity.component.html',
})
export class FindingSeverityComponent {
  severity = input(FindingSeverity.Low, {
    transform: transformSeverity
  })

  style(): string {
    if (this.severity() == FindingSeverity.Critical) {
      return 'bg-critical';
    }
    if (this.severity() == FindingSeverity.High) {
      return 'bg-high';
    }
    if (this.severity() == FindingSeverity.Medium) {
      return 'bg-medium';
    }
    if (this.severity() == FindingSeverity.Low) {
      return 'bg-low';
    }
    return 'bg-info';
  }

  severityLabel() {
    if (this.severity().length > 0) {
      return this.severity().charAt(0);
    }
    return 'U';
  }
}

function transformSeverity(severity: FindingSeverity | null | undefined): FindingSeverity {
  if (!severity) {
    return FindingSeverity.Info;
  }
  return severity;
}
