import {Component, input} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {Tooltip} from 'primeng/tooltip';

@Component({
  selector: 'package-type',
  imports: [
    NgIcon,
    Tooltip
  ],
  templateUrl: './package-type.component.html',
  standalone: true,
  styleUrl: './package-type.component.scss'
})
export class PackageTypeComponent {
  packageType = input('', {transform: transformString});
}

function transformString(value: string | undefined | null): string {
  if (!value) {
    return 'unknown';
  }
  return value;
}
