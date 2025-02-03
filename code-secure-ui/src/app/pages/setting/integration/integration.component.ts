import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from "../../../shared/ui/button/button.directive";
import {FormsModule} from "@angular/forms";
import {NgIcon, provideIcons} from '@ng-icons/core';
import {TimeagoModule} from 'ngx-timeago';
import {AvatarComponent} from '../../../shared/ui/avatar/avatar.component';
import {RouterLink} from '@angular/router';
import {TeamsComponent} from './teams/teams.component';
import {JiraComponent} from "./jira/jira.component";
import {TeamsSetting} from "../../../api/models/teams-setting";
import {JiraSetting} from '../../../api/models/jira-setting';
import {heroCheckCircle, heroEnvelope} from '@ng-icons/heroicons/outline';
import {IntegrationService} from '../../../api/services/integration.service';

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
    JiraComponent
  ],
  templateUrl: './integration.component.html',
  styleUrl: './integration.component.scss',
  viewProviders: [provideIcons({heroEnvelope, heroCheckCircle})]
})
export class IntegrationComponent implements  OnInit{
  config = {
    mail: false,
    teams: false,
    jira: false
  }
  teamsSetting?: TeamsSetting;
  jiraSetting?: JiraSetting;
  constructor(
    private integrationService: IntegrationService
  ) {
  }

  ngOnInit(): void {
    this.integrationService.getTeamsSetting().subscribe(setting => {
      this.teamsSetting = setting;
    });
    this.integrationService.getJiraSetting().subscribe(setting => {
      this.jiraSetting = setting;
    })
  }
}
