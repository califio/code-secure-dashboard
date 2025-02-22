import {Component, OnInit} from '@angular/core';
import {FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from "../../../core/forms";
import {NgIcon} from '@ng-icons/core';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {AuthRequest} from '../../../api/models/auth-request';
import {AuthService} from '../../../api/services/auth.service';
import {finalize} from 'rxjs';
import {AuthStore} from '../../../core/auth/auth.store';
import {environment} from '../../../../environments/environment';
import {AuthResponse} from '../../../api/models/auth-response';
import {bindQueryParams} from '../../../core/router';
import {ToastrService} from '../../../shared/services/toastr.service';
import {AuthConfig} from '../../../api/models/auth-config';
import {AppFloatingConfigurator} from '../../../layout/component/app.floatingconfigurator';
import {InputText} from 'primeng/inputtext';
import {Password} from 'primeng/password';
import {Checkbox} from 'primeng/checkbox';
import {ButtonDirective} from 'primeng/button';
import {Divider} from 'primeng/divider';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgIcon,
    RouterLink,
    AppFloatingConfigurator,
    InputText,
    Password,
    Checkbox,
    Divider,
    FormsModule,
    ButtonDirective
  ],
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  formConfig = new FormSection<ConfigOf<AuthRequest>>({
    password: new FormField('', Validators.required),
    userName: new FormField('', Validators.required),
  });
  form: FormGroup<ControlsOf<AuthRequest>>;
  authResponse: AuthResponse = {
    accessToken: null,
    refreshToken: null,
    requireConfirmEmail: null,
    requireTwoFactor: null,
  }
  authConfig: AuthConfig = {};

  constructor(
    private formService: FormService,
    private authService: AuthService,
    private authStore: AuthStore,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
    const params = this.route.snapshot.queryParams;
    const oidc = params['oidc'];
    if (oidc) {
      bindQueryParams(params, this.authResponse);
      if (params['message']) {
        this.toastr.error({
          message: params['message'],
          duration: 50000
        });
      }
      if (this.authResponse.accessToken && this.authResponse.refreshToken) {
        this.authStore.accessToken = this.authResponse.accessToken;
        this.authStore.refreshToken = this.authResponse.refreshToken;
        const returnUrl = this.activatedRoute.snapshot.queryParamMap.get('returnUrl') || '/dashboard';
        this.router.navigateByUrl(returnUrl).then();
      }
    }
    this.authService.getAuthConfig().subscribe(config => {
      this.authConfig = config;
    });
  }

  onOidcLogin() {
    location.href = `${environment.baseUrl}/api/login/oidc`;
  }

  onPasswordSignIn() {
    if (this.authConfig.disablePasswordLogon) {
      this.toastr.warning({
        message: 'The administrator disabled password logon'
      });
      return;
    }
    if (this.form.invalid || this.form.disabled) {
      return;
    }
    this.form.disable();
    this.authService.login({
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(response => {
        // todo: handle require two factor, verify email
        this.authStore.accessToken = response.accessToken!;
        this.authStore.refreshToken = response.refreshToken!;
        const returnUrl = this.activatedRoute.snapshot.queryParamMap.get('returnUrl') || '/dashboard';
        this.router.navigateByUrl(returnUrl).then();
      }
    );
  }
}
