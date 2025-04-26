import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from 'primeng/button';
import {InputText} from 'primeng/inputtext';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {JiraWebhookSetting} from '../../../../api/models/jira-webhook-setting';
import {IntegrationService} from '../../../../api/services/integration.service';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../../shared/services/toastr.service';

@Component({
  selector: 'app-jira-webhook-integration',
  imports: [
    ButtonDirective,
    InputText,
    ReactiveFormsModule,
    ToggleSwitch,
    FormsModule
  ],
  templateUrl: './jira-webhook.component.html',
})
export class JiraWebhookComponent implements OnInit {
  loading = false;
  loadingTest = false;
  setting: JiraWebhookSetting = {};

  constructor(
    private integrationService: IntegrationService,
    private toastr: ToastrService,
  ) {
  }

  ngOnInit(): void {
    this.integrationService.getJiraWebhookIntegrationSetting().subscribe(setting => {
      this.setting = setting;
    });
  }

  saveConfig() {
    this.loading = true;
    this.integrationService.updateJiraWebhookIntegrationSetting({
      body: this.setting
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    });
  }

  testConnection() {
    this.toastr.warning({
      message: 'Not support!'
    });
  }
}
