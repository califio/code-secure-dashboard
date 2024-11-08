import { Component } from '@angular/core';
import {ComingSoonComponent} from '../../shared/components/ui/coming-soon/coming-soon.component';
import {ConfirmPopupComponent} from '../../shared/components/ui/confirm-popup/confirm-popup.component';

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
