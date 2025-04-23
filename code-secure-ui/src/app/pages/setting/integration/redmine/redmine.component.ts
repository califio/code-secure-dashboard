import {Component, OnInit} from '@angular/core';
import {Button} from "primeng/button";
import {InputText} from "primeng/inputtext";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ToggleSwitch} from "primeng/toggleswitch";
import {RedmineSetting} from '../../../../api/models/redmine-setting';
import {IntegrationService} from '../../../../api/services/integration.service';
import {ToastrService} from '../../../../shared/services/toastr.service';
import {Step, StepList, StepPanel, StepPanels, Stepper} from 'primeng/stepper';
import {Password} from 'primeng/password';
import {Select} from 'primeng/select';
import {RedmineMetadata} from '../../../../api/models/redmine-metadata';
import {finalize} from 'rxjs';

@Component({
  selector: 'app-redmine',
  imports: [
    InputText,
    ReactiveFormsModule,
    ToggleSwitch,
    FormsModule,
    Stepper,
    Step,
    StepList,
    StepPanels,
    StepPanel,
    Button,
    Password,
    Select
  ],
  templateUrl: './redmine.component.html',
  styleUrl: './redmine.component.scss'
})
export class RedmineComponent implements OnInit {
  setting: RedmineSetting = {};
  metadata: RedmineMetadata = {};
  loading = false;
  loadingMetadata = false;
  loadingTest = false;
  activeStep = 1;

  constructor(
    private integrationService: IntegrationService,
    private toastr: ToastrService,
  ) {
  }

  ngOnInit(): void {
    this.integrationService.getRedmineIntegrationSetting().subscribe(setting => {
      this.setting = setting;
      if (this.setting.url) {
        if (setting.statusId! < 1 || setting.priorityId! < 1 || setting.projectId! < 1 || setting.trackerId! < 1) {
          this.activeStep = 2;
        } else {
          this.activeStep = 3;
        }
      } else {
        this.activeStep = 1;
      }
    });
    this.loadRedmineMetadata(false);
  }

  testConnection() {
    this.loadingTest = true;
    this.integrationService.testRedmineIntegrationSetting().pipe(
      finalize(() => this.loadingTest = false)
    ).subscribe(success => {
      if (success) {
        this.toastr.success({
          message: 'Success!'
        });
      }
    });
  }

  loadRedmineMetadata(reload: boolean) {
    var body: RedmineSetting | undefined = this.setting;
    if (!this.setting.token || !this.setting.url) {
      body = undefined;
    }
    this.loadingMetadata = true;
    this.integrationService.getRedmineMetadataIntegration({
      reload: reload,
      body: body
    }).pipe(
      finalize(() => this.loadingMetadata = false)
    ).subscribe(metadata => {
      this.metadata = metadata;
    });
  }

  tabTwo(activateCallback: any) {

    if (this.setting.url) {
      this.loadRedmineMetadata(true);
      activateCallback(2);
    }
  }

  save(activateCallback: any) {
    if (this.setting.statusId! < 1 || this.setting.priorityId! < 1 || this.setting.projectId! < 1 || this.setting.trackerId! < 1) {
      this.toastr.warning({
        message: 'Missing default option'
      });
      return;
    }
    this.loading = true;
    this.integrationService.updateRedmineIntegrationSetting({
      body: this.setting
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(success => {
      if (success) {
        this.toastr.success({
          message: 'Success!'
        });
        activateCallback(3);
      }
    })
  }
}
