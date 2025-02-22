import {Component} from '@angular/core';
import {ActivatedRoute, RouterLink} from '@angular/router';
import {ConfirmEmailRequest} from '../../../api/models/confirm-email-request';
import {bindQueryParams} from '../../../core/router';
import {AuthService} from '../../../api/services/auth.service';
import {ReactiveFormsModule} from '@angular/forms';
import {ConfirmEmailResult} from '../../../api/models/confirm-email-result';
import {finalize} from 'rxjs';
import {AppFloatingConfigurator} from '../../../layout/component/app.floatingconfigurator';
import {Button} from 'primeng/button';
import {NgIcon} from '@ng-icons/core';

@Component({
  selector: 'app-confirm-email',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    AppFloatingConfigurator,
    Button,
    NgIcon
  ],
  templateUrl: './confirm-email.component.html',
})
export class ConfirmEmailComponent {
  body: ConfirmEmailRequest = {
    token: '',
    username: ''
  }
  result: ConfirmEmailResult = {};
  loading = false;

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute
  ) {
    this.loading = true;
    bindQueryParams(this.route.snapshot.queryParams, this.body);
    this.authService.confirmEmail({
      body: this.body
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe(result => {
      this.result = result;
    })
  }
}
