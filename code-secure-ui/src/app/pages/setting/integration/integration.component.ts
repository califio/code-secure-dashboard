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
    MailComponent
  ],
  templateUrl: './integration.component.html',
})
export class IntegrationComponent implements OnInit {
  config = {
    mail: false,
    teams: false,
    jira: false
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
