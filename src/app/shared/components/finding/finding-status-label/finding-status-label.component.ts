import {Component, Input} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {FindingStatus} from '../../../../api/models/finding-status';
import {getFindingStatusColor, getFindingStatusIcon, getFindingStatusLabel} from '../../../helper';
import {NgClass} from '@angular/common';

@Component({
  selector: 'finding-status-label',
  standalone: true,
  imports: [
    NgIcon,
    NgClass
  ],
  templateUrl: './finding-status-label.component.html',
  styleUrl: './finding-status-label.component.scss'
})
export class FindingStatusLabelComponent {
  @Input() defaultIcon = ''
  @Input()
  defaultLabel = 'Status';
  @Input()
  status: FindingStatus | undefined | null = FindingStatus.Open;
  @Input()
  ngClass: string = '';

  protected readonly getFindingStatusIcon = getFindingStatusIcon;
  protected readonly getFindingStatusLabel = getFindingStatusLabel;
  protected readonly getFindingStatusColor = getFindingStatusColor;
}
