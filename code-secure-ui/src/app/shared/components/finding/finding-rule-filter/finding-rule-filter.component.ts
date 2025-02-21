import {Component, EventEmitter, input, model, Output} from '@angular/core';
import {Select} from 'primeng/select';
import {FormsModule} from '@angular/forms';
import {FloatLabel} from 'primeng/floatlabel';

@Component({
  selector: 'finding-rule-filter',
  imports: [
    Select,
    FormsModule,
    FloatLabel,
  ],
  templateUrl: './finding-rule-filter.component.html',
  standalone: true,
})
export class FindingRuleFilterComponent {
  options = input<string[]>([]);
  rule = model<string | undefined | null>();
  @Output()
  onChange = new EventEmitter<string>();
}
