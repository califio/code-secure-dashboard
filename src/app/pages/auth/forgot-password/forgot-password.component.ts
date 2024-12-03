import {Component} from '@angular/core';
import {ButtonDirective} from '../../../shared/ui/button/button.directive';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../../api/services/auth.service';
import {FormsModule} from '@angular/forms';
import {ToastrService} from '../../../shared/components/toastr/toastr.service';
import {finalize} from 'rxjs';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    ButtonDirective,
    RouterLink,
    FormsModule
  ],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent {
  loading = false;
  email = '';
  constructor(
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {
  }

  forgotPassword() {
    this.loading = true;
    if (this.email) {
      this.authService.forgotPassword({
        body: {
          username: this.email
        }
      }).pipe(
        finalize(() => this.loading = false)
      ).subscribe(() => {
        this.toastr.success('an email sent to your email account');
      })
    } else {
      this.toastr.error('email or username is required');
    }
  }
}
