import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from "../../../../shared/ui/button/button.directive";
import {FormGroup, FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../core/forms';
import {ConfigService} from '../../../../api/services/config.service';
import {ToastrService} from '../../../../shared/components/toastr/toastr.service';
import {finalize} from 'rxjs';
import {TeamsNotificationSettingRequest} from '../../../../api/models/teams-notification-setting-request';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [
    ButtonDirective,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './teams.component.html',
  styleUrl: './teams.component.scss'
})
export class TeamsComponent implements OnInit {
  formConfig = new FormSection<ConfigOf<TeamsNotificationSettingRequest>>({
    webhook: new FormField(''),
    active: new FormField(true),
    scanResultEvent: new FormField(false),
    securityAlertEvent: new FormField(true),
    fixedFindingEvent: new FormField(true),
    newFindingEvent: new FormField(true),
  })
  form: FormGroup<ControlsOf<TeamsNotificationSettingRequest>>
  loadingTest = false;

  constructor(
    private formService: FormService,
    private configService: ConfigService,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    this.configService.getTeamsSetting().subscribe(setting => {
      this.form.patchValue(setting as any);
    })
  }

  saveConfig() {
    this.form.disable();
    this.configService.updateTeamsSetting({
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(config => {
      this.toastr.success('Update config success!');
    })
  }

  testNotification() {
    this.loadingTest = true;
    this.configService.testTeamsSetting().pipe(
      finalize(() => this.loadingTest = false)
    ).subscribe(() => {
      this.toastr.success('Send notification success');
    })
  }
}
