import {Component, effect, EffectRef, EventEmitter, Input, input, OnDestroy, OnInit, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {DropdownItem} from './dropdown.model';
import {NgClass} from '@angular/common';
import {ClickOutsideDirective} from '../../directives/click-outside.directive';

@Component({
  selector: 'dropdown',
  standalone: true,
  imports: [
    NgIcon,
    NgClass,
    ClickOutsideDirective
  ],
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.scss'
})
export class DropdownComponent implements OnInit, OnDestroy {
  private _defaultLabel = 'Select';
  label = '';

  @Input()
  set defaultLabel (value: string) {
    if (value != '') {
      this.label = value;
    }
    this._defaultLabel = value;
  }

  @Input()
  options: DropdownItem[] = [];
  @Input()
  set selected(value: any) {
    this._selected = this.options.find(item => item.value == value);
    if (this._selected) {
      this.label = this._selected.label;
    } else {
      this.label = this._defaultLabel;
    }
  }
  @Output()
  selectedChange = new EventEmitter<any>()
  hidden = true;
  _selected: DropdownItem | undefined = undefined;
  constructor() {
  }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {
  }

  onClick(option: DropdownItem) {
    if (option.value != this._selected?.value) {
      this._selected = option;
      if (option.value) {
        this.label = option.label;
      } else {
        this.label = this._defaultLabel;
      }
      this.selectedChange.emit(option.value);
    }
    this.hidden = true;
  }
}
