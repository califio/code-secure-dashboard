import {Component, OnInit} from '@angular/core';
import {ButtonDirective} from '../../../shared/ui/button/button.directive';
import {FormGroup, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {DropdownComponent} from '../../../shared/ui/dropdown/dropdown.component';
import {DropdownItem} from '../../../shared/ui/dropdown/dropdown.model';
import {ConfigService} from '../../../api/services/config.service';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../shared/components/toastr/toastr.service';
import {NgIcon} from '@ng-icons/core';
import {AuthSetting} from '../../../api/models/auth-setting';
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from '../../../core/forms';
import {OpenIdConnectSetting} from '../../../api/models/open-id-connect-setting';

type AuthMode = 'local' | 'oidc'

@Component({
  selector: 'app-authentication',
  standalone: true,
  imports: [
    ButtonDirective,
    FormsModule,
    DropdownComponent,
    NgIcon,
    ReactiveFormsModule
  ],
  templateUrl: './authentication.component.html',
  styleUrl: './authentication.component.scss'
})
export class AuthenticationComponent implements OnInit {
  authMode: AuthMode = 'local';
  authOptions: DropdownItem[] = [
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
    whiteListEmails: new FormField('')
  })
  form: FormGroup<ControlsOf<AuthSetting>>;
  constructor(
    private formService: FormService,
    private configService: ConfigService,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    this.configService.getAuthSetting().subscribe(setting => {
      this.form.patchValue(setting);
      if (setting.openIdConnectSetting.enable) {
        this.authMode = 'oidc';
      } else {
        this.authMode = 'local';
      }
    })
  }

  saveConfig() {
    this.form.disable()
    if (this.authMode == 'local') {
      this.form.controls.disablePasswordLogon.setValue(false);
    }
    this.configService.updateAuthSetting({
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(config => {
      this.form.patchValue(config);
      this.toastr.success('Update config success!');
    })
  }
  secretTextType = false;
}
