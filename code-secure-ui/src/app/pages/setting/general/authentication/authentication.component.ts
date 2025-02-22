import {Component, OnInit} from '@angular/core';
import {FormGroup, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../../shared/services/toastr.service';
import {AuthSetting} from '../../../../api/models/auth-setting';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../../core/forms';
import {OpenIdConnectSetting} from '../../../../api/models/open-id-connect-setting';
import {SettingService} from '../../../../api/services/setting.service';
import {Select} from 'primeng/select';
import {Button} from 'primeng/button';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {InputText} from 'primeng/inputtext';
import {Password} from 'primeng/password';

type AuthMode = 'local' | 'oidc'

@Component({
  selector: 'app-authentication',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    Select,
    ToggleSwitch,
    InputText,
    Password,
    Button
  ],
  templateUrl: './authentication.component.html',
  styleUrl: './authentication.component.scss'
})
export class AuthenticationComponent implements OnInit {
  authMode: AuthMode = 'local';
  authOptions = [
    {
      value: 'local',
      label: 'LOCAL'
    },
    {
      value: 'oidc',
      label: 'OIDC'
    },
  ];
  formConfig = new FormSection<ConfigOf<AuthSetting>>({
    disablePasswordLogon: new FormField(false),
    openIdConnectSetting: new FormSection<ConfigOf<OpenIdConnectSetting>>({
      displayName: new FormField(''),
      authority: new FormField(''),
      clientId: new FormField(''),
      clientSecret: new FormField(''),
      enable: new FormField(false)
    }),
    allowRegister: new FormField(false),
    whiteListEmails: new FormField('')
  })
  form: FormGroup<ControlsOf<AuthSetting>>;

  constructor(
    private formService: FormService,
    private settingService: SettingService,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    this.settingService.getAuthSetting().subscribe(setting => {
      this.form.patchValue(setting);
      if (setting.openIdConnectSetting!.enable) {
        this.authMode = 'oidc';
      } else {
        this.authMode = 'local';
      }
    })
  }

  saveConfig() {
    this.form.disable()
    if (this.authMode == 'local') {
      this.form.controls.disablePasswordLogon!.setValue(false);
    }
    this.settingService.updateAuthSetting({
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(() => {
      this.toastr.success({
        message: 'Update success!'
      });
    })
  }

  secretTextType = false;
}
