import {Component, OnInit} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {SettingService} from '../../../../api/services/setting.service';
import {ToastrService} from '../../../../shared/services/toastr.service';
import {finalize} from 'rxjs';
import {Button} from 'primeng/button';
import {InputText} from 'primeng/inputtext';
import {Password} from 'primeng/password';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {SmtpSetting} from '../../../../api/models/smtp-setting';

@Component({
  selector: 'app-mail-setting',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    InputText,
    Password,
    Button,
    ToggleSwitch
  ],
  templateUrl: './mail.component.html',
  styleUrl: './mail.component.scss'
})
export class MailComponent implements OnInit {
  smtpSetting: SmtpSetting = {
    name: '',
    password: '',
    port: 0,
    server: '',
    userName: '',
    ignoreSsl: false,
    useSsl: false
  }
  loading = false;
  loadingTestMail = false;

  constructor(
    private settingService: SettingService,
    private toastr: ToastrService
  ) {
  }

  ngOnInit(): void {
    this.settingService.getSmtpSetting().subscribe(setting => {
      this.smtpSetting = setting;
    })
  }

  saveConfig() {
    this.loading = true;
    this.settingService.updateSmtpSetting({
      body: this.smtpSetting
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update config success!'
      });
    })
  }

  testConnection() {
    this.loadingTestMail = true;
    this.settingService.testSmtpSetting({
      email: undefined
    }).pipe(
      finalize(() => this.loadingTestMail = false)
    ).subscribe(email => {
      this.toastr.success({
        message: 'An email sent to ' + email
      });
    })
  }

}
