import { Component } from '@angular/core';
import {ButtonDirective} from '../../../../../../shared/ui/button/button.directive';
import {ReactiveFormsModule} from '@angular/forms';

@Component({
  selector: 'jira-integration',
  standalone: true,
  imports: [
    ButtonDirective,
    ReactiveFormsModule
  ],
  templateUrl: './jira.component.html',
  styleUrl: './jira.component.scss'
})
export class JiraComponent {

}
