import { Component } from '@angular/core';
import {ComingSoonComponent} from '../../shared/ui/coming-soon/coming-soon.component';
import {ConfirmPopupComponent} from '../../shared/ui/confirm-popup/confirm-popup.component';

@Component({
  selector: 'app-rule',
  standalone: true,
  imports: [
    ComingSoonComponent,
    ConfirmPopupComponent
  ],
  templateUrl: './rule.component.html',
  styleUrl: './rule.component.scss'
})
export class RuleComponent {

}
