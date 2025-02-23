import {Component, OnInit} from '@angular/core';
import {FormGroup, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../core/forms';
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
  formConfig = new FormSection<ConfigOf<MailAlertSetting>>({
    receivers: new FormField([]),
    active: new FormField(true),
    scanCompletedEvent: new FormField(false),
    scanFailedEvent: new FormField(false),
    securityAlertEvent: new FormField(true),
    fixedFindingEvent: new FormField(true),
    newFindingEvent: new FormField(true),
  })
  form: FormGroup<ControlsOf<MailAlertSetting>>
  receiver: string[] = [];

  constructor(
    private formService: FormService,
    private integrationService: IntegrationService,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    this.integrationService.getMailIntegrationSetting().subscribe(setting => {
      this.form.patchValue(setting);
      this.receiver = setting.receivers!;
    })
  }

  saveConfig() {
    this.form.disable();
    this.form.controls.receivers!.setValue(this.receiver.filter(item => item.length > 0));
    this.integrationService.updateMailIntegrationSetting({
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    })
  }

  deleteReceiver(email: string) {
    this.receiver = this.receiver.filter(item => item != email);
  }

  addEmail() {
    this.receiver.push('');
  }
}
