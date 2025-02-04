import {Component, EventEmitter, Input, OnDestroy, OnInit, Output, signal, TemplateRef} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {DropdownItem} from './dropdown.model';
import {NgClass, NgIf, NgTemplateOutlet} from '@angular/common';
import {ClickOutsideDirective} from '../../directives/click-outside.directive';
import {ButtonDirective} from "../button/button.directive";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'dropdown',
  standalone: true,
  imports: [
    NgIcon,
    NgClass,
    ClickOutsideDirective,
    ButtonDirective,
    FormsModule,
    NgTemplateOutlet,
    NgIf
  ],
  templateUrl: 'dropdown.component.html',
})
export class DropdownComponent implements OnInit, OnDestroy {
  @Input()
  showMaxOption = 2;
  @Input() optionTemplate!: TemplateRef<any>;
  @Input()
  showLabel = true;
  @Input()
  showSearch = true;
  @Input()
  mode: 'multiple' | 'default' = 'default';
  @Input()
  loading = false;
  @Input()
  set options(value: DropdownItem[]) {
    this._options = value;
    this.filterOptions.set(value);
    if (this._options.length < 5) {
      this.showSearch = false;
    }
    // update
    if (this._selected) {
      this.selected = this._selected;
    }
  }

  get options() {
    return this._options;
  }

  @Input()
  set selected(value: any) {
    if (value) {
      this._selected = value;
      this.mSelected.clear();
      if (Array.isArray(value)) {
        this.options
          .filter(item => value.findIndex(e => e == item.value) >= 0)
          .forEach(item => {
            this.mSelected.set(item.value, item);
          });
      } else {
        const option = this.options.find(item => item.value == value);
        if (option) {
          this.mSelected.set(option.value, option);
        }
      }
      this.selectedOptions.set(Array.from(this.mSelected.values()));
    }
  }
  private _selected: any = null;

  @Output()
  selectedChange = new EventEmitter<any>()
  hidden = true;
  search = '';
  filterOptions = signal<DropdownItem[]>([]);
  private _options: DropdownItem[] = [];
  selectedOptions = signal<DropdownItem[]>([]);
  private mSelected = new Map<any, DropdownItem>();

  constructor() {
  }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {
  }

  isSelected(value: any) {
    return this.mSelected.has(value);
  }

  onClick(option: DropdownItem) {
    if (this.mode == "multiple") {
      if (this.mSelected.has(option.value)) {
        this.mSelected.delete(option.value);
      } else {
        this.mSelected.set(option.value, option);
      }
    } else {
      this.mSelected.clear();
      this.mSelected.set(option.value, option);
      this.hidden = true;
      this.selectedChange.emit(option.value);
    }
    this.selectedOptions.set(Array.from(this.mSelected.values()));
  }

  selectChange() {
    this.hidden = true;
    this.selectedChange.emit(Array.from(this.mSelected.values()).map(item => item.value));
  }

  onSearch() {
    if (this.search) {
      const filterOptions = this.options.filter(value => {
        return value.label.includes(this.search)
      });
      this.filterOptions.set(filterOptions);
    } else {
      this.filterOptions.set(this.options);
    }
  }

  clearSelect() {
    this.mSelected.clear();
    this.selectedOptions.set([]);
  }
}
