import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NgIcon} from "@ng-icons/core";
import {FindingScan} from '../../../../api/models/finding-scan';
import {GitAction} from '../../../../api/models/git-action';
import {ClickOutsideDirective} from '../../../directives/click-outside.directive';
import {NgClass} from '@angular/common';
import {getFindingStatusColor, getFindingStatusIcon, getGitActionIcon} from '../helper';

@Component({
  selector: 'finding-scan-dropdown',
  standalone: true,
  imports: [
    NgIcon,
    ClickOutsideDirective,
    NgClass
  ],
  templateUrl: './finding-scan-dropdown.component.html',
  styleUrl: './finding-scan-dropdown.component.scss'
})
export class FindingScanDropdownComponent {
  hidden = true;
  label = 'Branch';
  selectedOption: FindingScan | undefined;

  @Input()
  set selected(scanId: string | null | undefined) {
    this.selectedOption = this.options.find(option => option.scanId == scanId);
  }

  @Input()
  options: FindingScan[] = [];
  @Output()
  selectChange = new EventEmitter<string>();

  onClick(option: FindingScan) {
    this.selectedOption = option;
    this.selectChange.emit(option.scanId);
  }
  protected readonly GitAction = GitAction;
  protected readonly getGitActionIcon = getGitActionIcon;
  protected readonly getFindingStatusIcon = getFindingStatusIcon;
  protected readonly getFindingStatusColor = getFindingStatusColor;
}
