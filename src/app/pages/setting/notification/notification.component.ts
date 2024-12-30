import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from "../../../shared/ui/button/button.directive";
import {FormsModule} from "@angular/forms";
import {NgIcon} from '@ng-icons/core';
import {TimeagoModule} from 'ngx-timeago';
import {AvatarComponent} from '../../../shared/ui/avatar/avatar.component';
import {RouterLink} from '@angular/router';
import {MailComponent} from './mail/mail.component';
import {TeamsComponent} from './teams/teams.component';
import {ConfigService} from '../../../api/services/config.service';
import {TeamsNotificationSetting} from '../../../api/models/teams-notification-setting';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [
    ButtonDirective,
    FormsModule,
    NgIcon,
    TimeagoModule,
    AvatarComponent,
    RouterLink,
    MailComponent,
    TeamsComponent
  ],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.scss'
})
export class NotificationComponent implements  OnInit{
  config = {
    mail: false,
    teams: false
  }
  teamsSetting?: TeamsNotificationSetting;
  constructor(
    private configService: ConfigService
  ) {
  }

  ngOnInit(): void {
    this.configService.getTeamsSetting().subscribe(setting => {
      this.teamsSetting = setting;
    })
  }
}
