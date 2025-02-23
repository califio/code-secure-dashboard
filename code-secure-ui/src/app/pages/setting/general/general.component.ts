import {Component} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {NgIcon} from '@ng-icons/core';
import {MailComponent} from './mail/mail.component';
import {AuthenticationComponent} from './authentication/authentication.component';
import {SlaComponent} from './sla/sla.component';
import {Panel} from 'primeng/panel';

@Component({
  selector: 'app-general',
  standalone: true,
  imports: [
    FormsModule,
    MailComponent,
    NgIcon,
    MailComponent,
    AuthenticationComponent,
    SlaComponent,
    Panel
  ],
  templateUrl: './general.component.html',
})
export class GeneralComponent {

  config = {
    mail: false,
    auth: false,
    sla: false
  };
}
