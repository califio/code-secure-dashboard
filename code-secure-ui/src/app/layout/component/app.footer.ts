import { Component } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-footer',
  template: `<div class="layout-footer">
        Code Secure by
        <a href="https://calif.io" target="_blank" rel="noopener noreferrer" class="text-primary font-bold hover:underline">calif.io</a>
    </div>`
})
export class AppFooter {}
