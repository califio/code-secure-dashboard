import {Component} from '@angular/core';
import {AvatarComponent} from '../../../../../shared/ui/avatar/avatar.component';
import {ButtonDirective} from '../../../../../shared/ui/button/button.directive';
import {NgIcon, provideIcons} from '@ng-icons/core';
import {JiraComponent} from './jira/jira.component';
import {ProjectStore} from '../../project.store';
import {TeamsComponent} from './teams/teams.component';
import {MailComponent} from './mail/mail.component';
import {heroCheckCircle, heroEnvelope} from '@ng-icons/heroicons/outline';
import {ProjectService} from '../../../../../api/services/project.service';
import {ProjectIntegration} from '../../../../../api/models/project-integration';

@Component({
  selector: 'app-integration',
  standalone: true,
  imports: [
    AvatarComponent,
    ButtonDirective,
    NgIcon,
    TeamsComponent,
    JiraComponent,
    TeamsComponent,
    MailComponent
  ],
  templateUrl: './integration.component.html',
  styleUrl: './integration.component.scss',
  viewProviders: [provideIcons({heroCheckCircle, heroEnvelope})]
})
export class IntegrationComponent {
  integrationSetting: ProjectIntegration = {};
  config = {
    mail: false,
    teams: false,
    jira: false
  }

  constructor(
    private projectService: ProjectService,
    public projectStore: ProjectStore
  ) {
    this.projectService.getIntegrationProject({
      projectId: projectStore.projectId()
    }).subscribe(setting => {
      this.integrationSetting = setting;
    })
  }
}
