import { Component } from '@angular/core';
import {ButtonDirective} from '../../../shared/ui/button/button.directive';
import {FormsModule} from '@angular/forms';
import {JiraComponent} from '../integration/jira/jira.component';
import {NgIcon, provideIcons} from '@ng-icons/core';
import {TeamsComponent} from '../integration/teams/teams.component';
import {MailComponent} from './mail/mail.component';
import {AuthenticationComponent} from './authentication/authentication.component';
import {SlaComponent} from './sla/sla.component';
import {heroEnvelope, heroKey, heroScale} from '@ng-icons/heroicons/outline';

@Component({
  selector: 'app-general',
  standalone: true,
  imports: [
    ButtonDirective,
    FormsModule,
    JiraComponent,
    MailComponent,
    NgIcon,
    TeamsComponent,
    MailComponent,
    AuthenticationComponent,
    SlaComponent
  ],
  templateUrl: './general.component.html',
  styleUrl: './general.component.scss',
  viewProviders: [provideIcons({heroScale, heroKey, heroEnvelope})]
})
export class GeneralComponent {

  config = {
    mail: false,
    auth: false,
    sla: false
  };
}
