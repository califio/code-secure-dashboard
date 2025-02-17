import {Component} from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../../api/services/auth.service';
import {FormsModule} from '@angular/forms';
import {ToastrService} from '../../../shared/services/toastr.service';
import {finalize} from 'rxjs';
import {AppFloatingConfigurator} from "../../../layout/component/app.floatingconfigurator";
import {ButtonDirective} from "primeng/button";
import {InputText} from "primeng/inputtext";
import {NgIcon} from "@ng-icons/core";

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    RouterLink,
    FormsModule,
    AppFloatingConfigurator,
    InputText,
    NgIcon,
    ButtonDirective,
  ],
  templateUrl: './forgot-password.component.html',
})
export class ForgotPasswordComponent {
  loading = false;
  email = '';

  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router,
  ) {
  }

  forgotPassword() {
    if (this.email) {
      this.loading = true;
      this.authService.forgotPassword({
        body: {
          username: this.email
        }
      }).pipe(
        finalize(() => this.loading = false)
      ).subscribe(() => {
        this.toastr.success({
          message: 'An email sent to your email account'
        });
      })
    } else {
      this.toastr.error({
        message: 'Email or username is required'
      });
    }
  }
}
