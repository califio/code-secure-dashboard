import {Component, EventEmitter, input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {NgClass} from '@angular/common';

@Component({
  selector: 'ng-button',
  standalone: true,
  imports: [
    NgIcon,
    NgClass
  ],
  templateUrl: './ng-button.component.html',
  styleUrl: './ng-button.component.scss'
})
export class NgButtonComponent {
  @Output()
  onClick = new EventEmitter();
  disabled = input<boolean>(false);
  loading = input<boolean>(false);
  label = input<string>('');
  ngClass = input<string>('');
  size = input<number>(16);

  onClickEvent() {
    if (!this.disabled() && !this.loading()){
      this.onClick.emit();
    }
  }
}
