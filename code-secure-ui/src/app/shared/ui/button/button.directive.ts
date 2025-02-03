import {Directive, ElementRef, Input, OnChanges, Renderer2, SimpleChanges} from '@angular/core';

@Directive({
  selector: '[app-button]',
  standalone: true
})
export class ButtonDirective implements OnChanges {
  @Input() type: 'default' | 'primary' | 'dashed' | 'link' = 'default';
  @Input() shape: 'default' | 'circle' | 'round' = 'default';
  @Input() loading: boolean = false;
  @Input() disabled: boolean = false;
  private spinnerElement: HTMLElement | null = null;
  private contentWrapper: HTMLElement | null = null;
  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngOnChanges(changes: SimpleChanges): void {
    this.applyClasses();
    if (changes['loading']) {
      this.updateLoadingState();
    }
  }

  private applyClasses() {
    const button = this.el.nativeElement;
    this.renderer.addClass(button, 'flex');
    this.renderer.addClass(button, 'flex-row');
    this.renderer.addClass(button, 'border-border');
    this.renderer.addClass(button, 'item-center');
    this.renderer.addClass(button, 'justify-center');
    this.renderer.addClass(button, 'gap-2');
    this.renderer.addClass(button, 'px-3');
    this.renderer.addClass(button, 'py-2');
    this.renderer.addClass(button, 'text-nowrap');
    // border
    if (this.type == "dashed") {
      this.renderer.addClass(button, 'border-dashed');
    } else {
      if (this.type == "link") {
        this.renderer.addClass(button, 'border-none');
      } else {
        this.renderer.addClass(button, 'border');
      }
    }
    // text
    if (this.type == "link") {
      this.renderer.addClass(button, 'text-primary');
    }
    if (this.type == "primary") {
      this.renderer.addClass(button, 'bg-primary');
      this.renderer.addClass(button, 'text-white');
    } else {
      this.renderer.addClass(button, 'bg-background');
    }
    if (this.disabled) {
      this.renderer.addClass(button, 'cursor-not-allowed');
      this.renderer.setAttribute(button, 'disabled', 'true');
    } else {
      this.renderer.addClass(button, 'cursor-pointer');
      this.renderer.removeAttribute(button, 'disabled');
    }
  }

  private updateLoadingState() {
    const button = this.el.nativeElement;
    if (this.loading) {
      if (!this.contentWrapper) {
        this.contentWrapper = this.renderer.createElement('div');
        while (button.firstChild) {
          this.renderer.appendChild(this.contentWrapper, button.firstChild);
        }
        this.renderer.appendChild(button, this.contentWrapper);
      }
      if (!this.spinnerElement) {
        this.spinnerElement = this.renderer.createElement('span');
        this.spinnerElement!.innerHTML = `
          <svg xmlns="http://www.w3.org/2000/svg" width="1.3em" height="1.3em" class="animate-spin" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.75" stroke-linecap="round" stroke-linejoin="round"><path d="M21 12a9 9 0 1 1-6.219-8.56"></path></svg>
        `;
        this.renderer.insertBefore(button, this.spinnerElement, this.contentWrapper);
      }
      this.renderer.setAttribute(button, 'disabled', 'true');
    } else {
      if (this.spinnerElement) {
        this.renderer.removeChild(button, this.spinnerElement);
        this.spinnerElement = null;
      }
      this.renderer.removeAttribute(button, 'disabled');
    }
  }
}
