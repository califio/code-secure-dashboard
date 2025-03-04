import {Injectable} from '@angular/core';
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {BehaviorSubject, Observable, throwError} from 'rxjs';
import {catchError, filter, switchMap, take} from 'rxjs/operators';
import {Router} from "@angular/router";
import {AuthStore} from './auth.store';
import {AuthService} from '../../api/services';
import {ToastrService} from '../../shared/services/toastr.service';

interface ErrorResponse {
  status: number
  errors: string[]
}

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private refreshTokenInProgress = false;
  private refreshToken$: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(
    private authStore: AuthStore,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(this.requestWithToken(req)).pipe(
      catchError((error) => {
        // Catch "401 Unauthorized" responses
        if (error instanceof HttpErrorResponse) {
          switch (error.status) {
            case 401: {
              if (req.url.includes('refresh-token') || !this.authStore.refreshToken) {
                this.refreshTokenInProgress = false;
                this.authStore.clearSession()
                this.router.navigate(['/auth', 'login']).then();
              } else {
                this.refreshToken$.pipe(
                  filter(value => value === true),
                  switchMap(value => {
                    return next.handle(this.requestWithToken(req));
                  })
                ).subscribe();
                if (this.refreshTokenInProgress) {
                  return this.refreshToken$.pipe(
                    filter(token => token != null),
                    take(1),
                    switchMap(jwt => {
                      return next.handle(this.requestWithToken(req));
                    }));
                } else {
                  this.refreshTokenInProgress = true;
                  this.refreshToken$.next(null);
                  return this.authService.refreshToken({
                    body: {
                      refreshToken: this.authStore.refreshToken!
                    }
                  }).pipe(
                    switchMap((session) => {
                      this.refreshTokenInProgress = false;
                      this.authStore.accessToken = session.accessToken!;
                      this.authStore.refreshToken = session.refreshToken!;
                      this.refreshToken$.next(session.accessToken!);
                      return next.handle(this.requestWithToken(req));
                    }));
                }
              }
              break
            }
            case 400: {
              let err: ErrorResponse;
              if (typeof error.error === 'string') {
                err = JSON.parse(error.error)
              } else {
                err = error.error;
              }
              this.handleError(err);
              break
            }
            default: {
              if (req.url.includes("/api")) {
                this.router.navigate(['/error/', error.status]).then();
              }
            }
          }
        }
        return throwError(error);
      })
    );
  }

  private requestWithToken(request: HttpRequest<any>): HttpRequest<any> {
    if (!this.authStore.accessToken) {
      return request;
    }
    return request.clone({
      setHeaders: {
        'Authorization': `Bearer ${this.authStore.accessToken}`
      },
      withCredentials: false
    });
  }

  private handleError(err: ErrorResponse) {
    if (err.errors && err.errors.length > 0) {
      err.errors.forEach(e => {
        this.toastr.error({
          message: e
        });
      });
    }
  }
}


