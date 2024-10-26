import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ConfigOf, ControlsOf, FormField, FormSection, FormService} from "../../../core/forms";
import {NgIcon} from '@ng-icons/core';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {NgClass} from '@angular/common';
import {AuthRequest} from '../../../api/models/auth-request';
import {AuthService} from '../../../api/services/auth.service';
import {finalize} from 'rxjs';
import {AuthStoreService} from '../../../core/auth/auth.store';
import {NgButtonComponent} from '../../../shared/components/ng-button/ng-button.component';

interface LoginForm {
  username: FormControl,
  password: FormControl
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgIcon,
    RouterLink,
    NgClass,
    NgButtonComponent
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  formConfig = new FormSection<ConfigOf<AuthRequest>>({
    password: new FormField('', Validators.required),
    userName: new FormField('', Validators.required),
  });
  form: FormGroup<ControlsOf<AuthRequest>>;
  submitted = false;
  passwordTextType = false;

  constructor(
    private formService: FormService,
    private authService: AuthService,
    private authStore: AuthStoreService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.formService.group(this.formConfig);
  }

  ngOnInit(): void {
  }

  login() {
    console.log(this.form.value)
  }

  onSignIn() {
    if (this.form.invalid || this.form.disabled) {
      return;
    }
    this.form.disable();
    this.authService.login({
      body: this.form.getRawValue()
    }).pipe(
      finalize(() => this.form.enable())
    ).subscribe(response => {
        // todo: handle require two factor
        this.authStore.accessToken = response.accessToken!;
        this.authStore.refreshToken = response.refreshToken!;
        const returnUrl = this.activatedRoute.snapshot.queryParamMap.get('returnUrl') || '/dashboard';
        this.router.navigateByUrl(returnUrl).then();
      }
    );
  }
}
