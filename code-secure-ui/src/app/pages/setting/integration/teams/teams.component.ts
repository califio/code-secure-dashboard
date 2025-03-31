import {Component, OnInit} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ToastrService} from '../../../../shared/services/toastr.service';
import {finalize} from 'rxjs';
import {IntegrationService} from '../../../../api/services/integration.service';
import {ButtonDirective} from 'primeng/button';
import {ToggleSwitch} from "primeng/toggleswitch";
import {InputText} from 'primeng/inputtext';
import {TeamsAlertSetting} from '../../../../api/models/teams-alert-setting';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    ButtonDirective,
    ToggleSwitch,
    InputText,
  ],
  templateUrl: './teams.component.html',
})
export class TeamsComponent implements OnInit {
  setting: TeamsAlertSetting = {
    active: false,
    fixedFindingEvent: false,
    needTriageFindingEvent: false,
    newFindingEvent: false,
    projectWithoutMemberEvent: false,
    scanCompletedEvent: false,
    scanFailedEvent: false,
    securityAlertEvent: false,
    webhook: ''
  };
  loading = false;
  loadingTest = false;

  constructor(
    private integrationService: IntegrationService,
    private toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.integrationService.getTeamsIntegrationSetting().subscribe(setting => {
      this.setting = setting;
    })
  }

  saveConfig() {
    this.loading = true;
    this.integrationService.updateTeamsIntegrationSetting({
      body: this.setting
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update config success!'
      });
    })
  }

  testConnection() {
    this.loadingTest = true;
    this.integrationService.testTeamsIntegrationSetting().pipe(
      finalize(() => this.loadingTest = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Send notification success'
      });
    })
  }
}
