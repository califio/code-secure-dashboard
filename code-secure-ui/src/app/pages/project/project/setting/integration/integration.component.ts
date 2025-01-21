import { Component } from '@angular/core';
import {AvatarComponent} from '../../../../../shared/ui/avatar/avatar.component';
import {ButtonDirective} from '../../../../../shared/ui/button/button.directive';
import {MailComponent} from '../../../../setting/notification/mail/mail.component';
import {NgIcon} from '@ng-icons/core';
import {TeamsComponent} from '../../../../setting/notification/teams/teams.component';
import {JiraComponent} from './jira/jira.component';

@Component({
  selector: 'app-integration',
  standalone: true,
  imports: [
    AvatarComponent,
    ButtonDirective,
    MailComponent,
    NgIcon,
    TeamsComponent,
    JiraComponent
  ],
  templateUrl: './integration.component.html',
  styleUrl: './integration.component.scss'
})
export class IntegrationComponent {
  config = {
    jira: false
  }
}
