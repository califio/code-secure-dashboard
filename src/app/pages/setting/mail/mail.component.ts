import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from "../../../shared/ui/button/button.directive";
import {FormGroup, FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ConfigService} from '../../../api/services/config.service';
import {ToastrService} from '../../../shared/components/toastr/toastr.service';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../core/forms';
import {MailSetting} from '../../../api/models/mail-setting';
import {finalize} from 'rxjs';

@Component({
  selector: 'app-mail',
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
  })
  form: FormGroup<ControlsOf<MailSetting>>
  loadingTestMail = false;
  constructor(
    private formService: FormService,
    private configService: ConfigService,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    this.configService.getMailSetting().subscribe(mailSetting => {
      this.form.patchValue(mailSetting);
    })
  }

  saveConfig() {
    this.form.disable();

    this.configService.updateMailSetting({
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(config => {
      this.toastr.success('Update config success!');
    })
  }

  testMail() {
    this.loadingTestMail = true;
    this.configService.testMailSetting({
      email: undefined
    }).pipe(
      finalize(() => this.loadingTestMail = false)
    ).subscribe(email => {
      this.toastr.success('An email sent to ' + email);
    })
  }
}
