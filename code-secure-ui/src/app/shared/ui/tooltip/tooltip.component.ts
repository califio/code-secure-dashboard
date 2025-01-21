import {Component} from '@angular/core';
import {NgClass} from '@angular/common';
import {TooltipPosition, TooltipTheme} from './tooltip.model';

@Component({
  selector: 'app-tooltip',
  standalone: true,
  imports: [
    NgClass
  ],
  templateUrl: './tooltip.component.html',
  styleUrl: './tooltip.component.scss'
})
export class TooltipComponent {
  position: TooltipPosition = 'above';
  theme: TooltipTheme = "light"
  tooltip: string | null | undefined = '';
  left = 0;
  top = 0;
  visible = false;
}
