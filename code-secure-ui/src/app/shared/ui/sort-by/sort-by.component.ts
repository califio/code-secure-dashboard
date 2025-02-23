import {Component, EventEmitter, model, Output} from '@angular/core';
import {FloatLabel} from 'primeng/floatlabel';
import {Select} from 'primeng/select';
import {FormsModule} from '@angular/forms';
import {SortByState} from './sort-by-state';
import {NgClass} from '@angular/common';

@Component({
  selector: 'sort-by',
  imports: [
    FloatLabel,
    Select,
    FormsModule,
    NgClass
  ],
  templateUrl: './sort-by.component.html',
  standalone: true,
})
export class SortByComponent {
  options = model<{ label: string, value: any }[]>();
  sortBy = model<any>();
  desc = model<boolean>(true);
  @Output()
  onChange = new EventEmitter<SortByState>();

  onSortChange($event: any) {
    this.sortBy.set($event);
    this.onChange.emit({desc: this.desc(), sortBy: $event})
  }

  onOrderChange() {
    this.desc.set(!this.desc());
    this.onChange.emit({desc: this.desc(), sortBy: this.sortBy()})
  }
}
