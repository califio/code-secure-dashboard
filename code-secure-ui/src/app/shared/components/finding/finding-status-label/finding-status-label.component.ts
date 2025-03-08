import {Component, input, Input} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {FindingStatus} from '../../../../api/models/finding-status';
import {NgClass} from '@angular/common';
import {
  getFindingStatusColor,
  getFindingStatusDescription,
  getFindingStatusIcon,
  getFindingStatusLabel
} from '../finding-status';
import {transformValueNotNull} from '../../../../core/transform';

@Component({
  selector: 'finding-status-label',
  standalone: true,
  imports: [
    NgIcon,
    NgClass
  ],
  templateUrl: './finding-status-label.component.html',
})
export class FindingStatusLabelComponent {
  status = input(FindingStatus.Open, {
    transform: (value: FindingStatus | null | undefined) => transformValueNotNull(value, FindingStatus.Open)
  });
  @Input() showDescription = false;
  @Input() styleClass: string = '';
  protected readonly getFindingStatusLabel = getFindingStatusLabel;
  protected readonly getFindingStatusIcon = getFindingStatusIcon;
  protected readonly getFindingStatusColor = getFindingStatusColor;
  protected readonly getFindingStatusDescription = getFindingStatusDescription;
}
