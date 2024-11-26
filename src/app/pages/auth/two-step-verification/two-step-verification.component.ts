import {Component} from '@angular/core';
import {ButtonDirective} from '../../../shared/ui/button/button.directive';
import {NgClass} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

@Component({
  selector: 'app-two-step-verification',
  standalone: true,
  imports: [
    ButtonDirective,
    NgClass,
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './two-step-verification.component.html',
  styleUrl: './two-step-verification.component.scss'
})
export class TwoStepVerificationComponent {
  otpValues: string[] = new Array(6).fill(''); // Giá trị nhập vào
  public inputs = new Array(6);
  index = 1;

  nextInput() {
    if (this.index < this.inputs.length) {
      this.index += 1;
    }
  }

  changeIndex($index: number) {
    this.index = $index;
  }

  onInput($event: Event, $index: number) {
    const input = $event.target as HTMLInputElement;
    if (input.value.match(/^[0-9]$/)) {
      this.otpValues[$index] = input.value;
      if ($index < this.otpValues.length - 1) {
        this.focusNext($index);
      }
    } else {
      input.value = '';
    }
  }

  onPaste($event: ClipboardEvent) {
    const clipboardData = $event.clipboardData?.getData('text') || '';
    if (clipboardData.match(/^[0-9]{6}$/)) {
      $event.preventDefault();
      clipboardData.split('').forEach((char, index) => {
        if (index < this.otpValues.length) {
          this.otpValues[index] = char;
        }
      });
    }
  }

  onKeyDown($event: KeyboardEvent, $index: number) {
    const input = $event.target as HTMLInputElement;
    if ($event.key === 'Backspace') {
      this.otpValues[$index] = '';
      if ($index > 0 && !input.value) {
        this.focusPrev($index);
      }
    }
  }

  focusNext(index: number): void {
    const nextInput = document.querySelectorAll<HTMLInputElement>('.otp-input')[index + 1];
    if (nextInput) {
      nextInput.focus();
    }
  }

  focusPrev(index: number): void {
    const prevInput = document.querySelectorAll<HTMLInputElement>('.otp-input')[index - 1];
    if (prevInput) {
      prevInput.focus();
    }
  }

}
