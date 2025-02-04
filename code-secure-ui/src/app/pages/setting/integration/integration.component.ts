import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from "../../../shared/ui/button/button.directive";
import {FormsModule} from "@angular/forms";
import {NgIcon} from '@ng-icons/core';
import {TimeagoModule} from 'ngx-timeago';
import {AvatarComponent} from '../../../shared/ui/avatar/avatar.component';
import {RouterLink} from '@angular/router';
import {TeamsComponent} from './teams/teams.component';
import {JiraComponent} from "./jira/jira.component";
import {IntegrationService} from '../../../api/services/integration.service';
import {IntegrationSetting} from '../../../api/models/integration-setting';
import {MailComponent} from './mail/mail.component';

@Component({
  selector: 'app-integration',
  standalone: true,
  imports: [
    ButtonDirective,
    FormsModule,
    NgIcon,
    TimeagoModule,
    AvatarComponent,
    RouterLink,
    TeamsComponent,
    JiraComponent,
    MailComponent,
    MailComponent
  ],
  templateUrl: './integration.component.html',
  styleUrl: './integration.component.scss',
})
export class IntegrationComponent implements OnInit {
  config = {
    mail: false,
    teams: false,
    jira: false
  }
  integrationSetting: IntegrationSetting = {};

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
