import {Component, Input} from '@angular/core';
import {RiskLevel} from '../../../api/models/risk-level';
import {NgIcon} from '@ng-icons/core';
import {LowerCasePipe, NgClass} from '@angular/common';
import {RiskImpact} from '../../../api/models/risk-impact';
import {Tooltip} from 'primeng/tooltip';

@Component({
  selector: 'risk-level-icon',
  standalone: true,
  imports: [
    NgIcon,
    LowerCasePipe,
    Tooltip,
    NgClass
  ],
  templateUrl: './risk-level-icon.component.html',
})
export class RiskLevelIconComponent {
  @Input()
  risk: RiskLevel = RiskLevel.None
  @Input()
  impact: RiskImpact = RiskImpact.None;

  mColor = new Map<RiskLevel, string>([
    [RiskLevel.None, 'text-green-500'],
    [RiskLevel.Low, 'text-lime-500'],
    [RiskLevel.Medium, 'text-yellow-500'],
    [RiskLevel.High, 'text-orange-500'],
    [RiskLevel.Critical, 'text-red-500'],
  ]);
  protected readonly RiskLevel = RiskLevel;
  protected readonly RiskImpact = RiskImpact;
}
