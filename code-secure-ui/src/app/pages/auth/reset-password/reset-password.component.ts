import { Component } from '@angular/core';
import {ResetPasswordRequest} from '../../../api/models/reset-password-request';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {bindQueryParams} from '../../../core/router';
import {ButtonDirective} from '../../../shared/ui/button/button.directive';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgIcon} from '@ng-icons/core';
import {AuthService} from '../../../api/services/auth.service';
import {finalize} from 'rxjs';
import {ToastrService} from '../../../shared/components/toastr/toastr.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    ButtonDirective,
    ReactiveFormsModule,
    RouterLink,
    NgIcon,
    FormsModule
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
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
      this.toastr.success('Reset password success');
      this.router.navigateByUrl('/auth/login').then();
    })
  }
}
