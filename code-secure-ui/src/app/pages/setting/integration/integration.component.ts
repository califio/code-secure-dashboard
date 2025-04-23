import {Component, OnInit} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {NgIcon} from '@ng-icons/core';
import {TimeagoModule} from 'ngx-timeago';
import {TeamsComponent} from './teams/teams.component';
import {JiraComponent} from "./jira/jira.component";
import {IntegrationService} from '../../../api/services/integration.service';
import {Panel} from "primeng/panel";
import {MailComponent} from './mail/mail.component';
import {IntegrationStatus} from '../../../api/models/integration-status';
import {JiraWebhookComponent} from './jira-webhook/jira-webhook.component';
import {RedmineComponent} from './redmine/redmine.component';

@Component({
  selector: 'app-integration',
  standalone: true,
  imports: [
    FormsModule,
    NgIcon,
    TimeagoModule,
    TeamsComponent,
    JiraComponent,
    Panel,
    MailComponent,
    MailComponent,
    JiraWebhookComponent,
    RedmineComponent
  ],
  templateUrl: './integration.component.html',
})
export class IntegrationComponent implements OnInit {
  config = {
    mail: false,
    teams: false,
    jira: false,
    jiraWebhook: false,
    redmine: false
  }
  integrationSetting: IntegrationStatus = {};

  constructor(
    private integrationService: IntegrationService
  ) {
  }

  ngOnInit(): void {
    this.integrationService.getIntegrationSetting().subscribe(setting => {
      this.integrationSetting = setting;
    })
  }
}
