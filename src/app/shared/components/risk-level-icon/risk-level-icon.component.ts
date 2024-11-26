import {Component, Input} from '@angular/core';
import {RiskLevel} from '../../../api/models/risk-level';
import {NgIcon} from '@ng-icons/core';
import {TooltipDirective} from '../../ui/tooltip/tooltip.directive';
import {LowerCasePipe} from '@angular/common';

@Component({
  selector: 'risk-level-icon',
  standalone: true,
  imports: [
    NgIcon,
    TooltipDirective,
    LowerCasePipe
  ],
  templateUrl: './risk-level-icon.component.html',
  styleUrl: './risk-level-icon.component.scss'
})
export class RiskLevelIconComponent {
  @Input()
  risk: RiskLevel | undefined | null = RiskLevel.None
  protected readonly RiskLevel = RiskLevel;
}
