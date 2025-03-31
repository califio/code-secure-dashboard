import {Component, OnInit} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {IntegrationService} from '../../../../api/services/integration.service';
import {ToastrService} from '../../../../shared/services/toastr.service';
import {finalize} from 'rxjs';
import {MailAlertSetting} from '../../../../api/models/mail-alert-setting';
import {NgIcon} from "@ng-icons/core";
import {Button, ButtonDirective} from 'primeng/button';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {InputText} from 'primeng/inputtext';

@Component({
  selector: 'app-mail-integration',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgIcon,
    ButtonDirective,
    ToggleSwitch,
    Button,
    InputText
  ],
  templateUrl: './mail.component.html',
})
export class MailComponent implements OnInit {
  setting: MailAlertSetting = {
    active: false,
    fixedFindingEvent: false,
    needTriageFindingEvent: false,
    newFindingEvent: false,
    projectWithoutMemberEvent: false,
    scanCompletedEvent: false,
    scanFailedEvent: false,
    securityAlertEvent: false,
    receivers: []
  };
  loading = false;

  constructor(
    private integrationService: IntegrationService,
    private toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.integrationService.getMailIntegrationSetting().subscribe(setting => {
      this.setting = setting;
    })
  }

  saveConfig() {
    this.loading = true;
    this.setting.receivers = this.setting.receivers?.filter(item => item.length > 0);
    this.integrationService.updateMailIntegrationSetting({
      body: this.setting
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    })
  }

  deleteReceiver(email: string) {
    this.setting.receivers = this.setting.receivers?.filter(item => item != email);
  }

  addEmail() {
    this.setting.receivers?.push('');
  }
}
