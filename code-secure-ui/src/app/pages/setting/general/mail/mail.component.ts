import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from "../../../../shared/ui/button/button.directive";
import {FormGroup, FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../core/forms';
import {MailSetting} from '../../../../api/models/mail-setting';
import {SettingService} from '../../../../api/services/setting.service';
import {ToastrService} from '../../../../shared/components/toastr/toastr.service';
import {finalize} from 'rxjs';

@Component({
  selector: 'app-mail-setting',
  standalone: true,
  imports: [
    ButtonDirective,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './mail.component.html',
  styleUrl: './mail.component.scss'
})
export class MailComponent implements OnInit {
  formConfig = new FormSection<ConfigOf<MailSetting>>({
    password: new FormField(''),
    port: new FormField(0),
    server: new FormField(''),
    userName: new FormField(''),
    useSsl: new FormField(false),
  });
  form: FormGroup<ControlsOf<MailSetting>>
  loadingTestMail = false;
  constructor(
    private formService: FormService,
    private settingService: SettingService,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    this.settingService.getMailSetting().subscribe(mailSetting => {
      this.form.patchValue(mailSetting);
    })
  }

  saveConfig() {
    this.form.disable();
    this.settingService.updateMailSetting({
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(() => {
      this.toastr.success('Update config success!');
    })
  }

  testMail() {
    this.loadingTestMail = true;
    this.settingService.testMailSetting({
      email: undefined
    }).pipe(
      finalize(() => this.loadingTestMail = false)
    ).subscribe(email => {
      this.toastr.success('An email sent to ' + email);
    })
  }

}
