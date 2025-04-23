import {Component} from '@angular/core';
import {NgIcon} from '@ng-icons/core';
import {JiraComponent} from './jira/jira.component';
import {ProjectStore} from '../../project.store';
import {TeamsComponent} from './teams/teams.component';
import {MailComponent} from './mail/mail.component';
import {ProjectService} from '../../../../../api/services/project.service';
import {ProjectIntegration} from '../../../../../api/models/project-integration';
import {Panel} from 'primeng/panel';
import {RedmineComponent} from './redmine/redmine.component';

@Component({
  selector: 'app-integration',
  standalone: true,
  imports: [
    NgIcon,
    TeamsComponent,
    JiraComponent,
    TeamsComponent,
    MailComponent,
    Panel,
    JiraComponent,
    MailComponent,
    TeamsComponent,
    JiraComponent,
    MailComponent,
    MailComponent,
    RedmineComponent
  ],
  templateUrl: './integration.component.html',
})
export class IntegrationComponent {
  integrationSetting: ProjectIntegration = {};
  config = {
    mail: false,
    teams: false,
    jira: false,
    redmine: false
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
