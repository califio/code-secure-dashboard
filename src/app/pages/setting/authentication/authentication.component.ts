import {Component} from '@angular/core';
import {ButtonDirective} from '../../../shared/directives/button.directive';
import {FormsModule} from '@angular/forms';
import {DropdownComponent} from '../../../shared/components/ui/dropdown/dropdown.component';
import {DropdownItem} from '../../../shared/components/ui/dropdown/dropdown.model';
import {AuthConfig} from '../../../api/models';
import {ConfigService} from '../../../api/services/config.service';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../shared/components/toastr/toastr.service';
import {NgIcon} from '@ng-icons/core';

type AuthMode = 'local' | 'oidc'

@Component({
  selector: 'app-authentication',
  standalone: true,
  imports: [
    ButtonDirective,
    FormsModule,
    DropdownComponent,
    NgIcon
  ],
  templateUrl: './authentication.component.html',
  styleUrl: './authentication.component.scss'
})
export class AuthenticationComponent {
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
  config: AuthConfig = {};
  loading = false;
  constructor(
    private configService: ConfigService,
    private toastr: ToastrService
  ) {
    configService.getAuthConfig().subscribe(config => {
      this.config = config;
      if (this.config.oidcEnable) {
        this.authMode = 'oidc';
      } else {
        this.authMode = 'local';
      }
      if (!this.config.oidcScope) {
        this.config.oidcScope = "openid profile email";
      }
    })
  }

  saveConfig() {
    this.loading = true;
    if (this.authMode == 'local') {
      this.config.disablePasswordLogon = false;
    }
    this.configService.updateAuthConfig({
      body: this.config
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(config => {
      this.config = config;
      this.toastr.success('Update config success!');
    })
  }
  secretTextType = false;
}
