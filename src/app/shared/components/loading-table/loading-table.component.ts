import {Component, input} from '@angular/core';

@Component({
  selector: 'loading-table',
  standalone: true,
  imports: [],
  templateUrl: './loading-table.component.html',
  styleUrl: './loading-table.component.scss'
})
export class LoadingTableComponent {
  row = input<number>(8);
}
