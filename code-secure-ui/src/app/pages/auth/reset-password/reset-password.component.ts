import {Component} from '@angular/core';
import {ResetPasswordRequest} from '../../../api/models/reset-password-request';
import {ActivatedRoute, Router} from '@angular/router';
import {bindQueryParams} from '../../../core/router';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgIcon} from '@ng-icons/core';
import {AuthService} from '../../../api/services/auth.service';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../shared/services/toastr.service';
import {AppFloatingConfigurator} from '../../../layout/component/app.floatingconfigurator';
import {ButtonDirective} from 'primeng/button';
import {Password} from 'primeng/password';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgIcon,
    FormsModule,
    AppFloatingConfigurator,
    Password,
    ButtonDirective
  ],
  templateUrl: './reset-password.component.html',
})
export class ResetPasswordComponent {
  body: ResetPasswordRequest = {
    password: "", token: "", username: ""
  }
  loading = false;
  passwordTextType = false;

  constructor(
    private toastr: ToastrService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    bindQueryParams(this.route.snapshot.queryParams, this.body);
  }

  setPassword() {
    this.loading = true;
    this.authService.resetPassword({
      body: this.body
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(() => {
      this.toastr.success({
        message: 'Reset password success'
      });
      this.router.navigateByUrl('/auth/login').then();
    })
  }
}
