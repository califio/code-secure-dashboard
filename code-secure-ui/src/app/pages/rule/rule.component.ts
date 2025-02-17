import {Component} from '@angular/core';
import {ComingSoonComponent} from '../../shared/ui/coming-soon/coming-soon.component';

@Component({
  selector: 'app-rule',
  standalone: true,
  imports: [
    ComingSoonComponent,
  ],
  templateUrl: './rule.component.html',
  styleUrl: './rule.component.scss'
})
export class RuleComponent {

}
