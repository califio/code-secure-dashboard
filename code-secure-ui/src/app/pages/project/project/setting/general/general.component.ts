import { Component } from '@angular/core';
import {AuthenticationComponent} from "../../../../setting/general/authentication/authentication.component";
import {MailComponent} from "../../../../setting/general/mail/mail.component";
import {NgIcon} from "@ng-icons/core";
import {Panel} from "primeng/panel";
import {SlaComponent} from "../../../../setting/general/sla/sla.component";
import {SecurityThresholdComponent} from './security-threshold/security-threshold.component';
import {DefaultBranchComponent} from './default-branch/default-branch.component';

@Component({
  selector: 'app-general',
  imports: [
    NgIcon,
    Panel,
    SecurityThresholdComponent,
    DefaultBranchComponent
  ],
  templateUrl: './general.component.html',
  styleUrl: './general.component.scss'
})
export class GeneralComponent {

}
