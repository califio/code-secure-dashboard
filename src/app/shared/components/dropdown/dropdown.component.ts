import {Component, effect, EffectRef, EventEmitter, input, OnDestroy, OnInit, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {DropdownItem} from './dropdown.model';
import {NgClass} from '@angular/common';

@Component({
  selector: 'dropdown',
  standalone: true,
  imports: [
    NgIcon,
    NgClass
  ],
  templateUrl: './dropdown.component.html',
  styleUrl: './dropdown.component.scss'
})
export class DropdownComponent implements OnInit, OnDestroy {

  defaultLabel = input<string>('Select');
  options = input<DropdownItem[]>([]);
  @Output()
  onSelect = new EventEmitter<DropdownItem>()
  label = '';
  selected: DropdownItem = {
    value: undefined,
    label: ''
  };
  hidden = true;

  constructor() {
    this.destroy = effect(() => {
      const defaultLabel = this.defaultLabel()
      if (defaultLabel != '') {
        this.label = defaultLabel
      }
    })
  }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {
    this.destroy?.destroy();
  }

  onClick(option: DropdownItem) {
    if (option.value != this.selected.value) {
      this.selected = option;
      if (option.value != null) {
        this.label = option.label;
      } else {
        this.label = this.defaultLabel();
      }
      this.onSelect.emit(option);
    }
    this.hidden = true;
  }

  private destroy?: EffectRef;
}
