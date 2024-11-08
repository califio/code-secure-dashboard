import {Component, EventEmitter, Input, Output} from '@angular/core';
import {NgIcon} from '@ng-icons/core';

@Component({
  selector: 'confirm-popup',
  standalone: true,
  imports: [
    NgIcon
  ],
  templateUrl: './confirm-popup.component.html',
  styleUrl: './confirm-popup.component.scss'
})
export class ConfirmPopupComponent {
  @Input() hidden = true;
  @Input() title = 'Are you sure you want to delete this?';
  @Input() confirmText = 'Yes';
  @Input() cancelText = 'Cancel';
  @Output() cancel = new EventEmitter()
  @Output() confirm = new EventEmitter();

  onCancel() {
    this.cancel.emit();
  }

  onConfirm() {
    this.confirm.emit();
  }
}
