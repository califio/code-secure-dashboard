import { Component } from '@angular/core';
import {ButtonDirective} from '../../../../../../shared/ui/button/button.directive';
import {ReactiveFormsModule} from '@angular/forms';
import {ComingSoonComponent} from '../../../../../../shared/ui/coming-soon/coming-soon.component';

@Component({
  selector: 'jira-integration',
  standalone: true,
  imports: [
    ButtonDirective,
    ReactiveFormsModule,
    ComingSoonComponent
  ],
  templateUrl: './jira.component.html',
  styleUrl: './jira.component.scss'
})
export class JiraComponent {

}
