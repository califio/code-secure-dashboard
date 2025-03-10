import {Component, EventEmitter, input, model, Output} from '@angular/core';
import {FloatLabel} from 'primeng/floatlabel';
import {Select} from 'primeng/select';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'finding-category-filter',
  imports: [
    FloatLabel,
    Select,
    FormsModule
  ],
  templateUrl: './finding-category-filter.component.html',
  standalone: true,
  styleUrl: './finding-category-filter.component.scss'
})
export class FindingCategoryFilterComponent {
  options = input<string[]>([]);
  category = model<string | undefined | null>();
  @Output()
  onChange = new EventEmitter<string>();
}
