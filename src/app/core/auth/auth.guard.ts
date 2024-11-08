import {Injectable} from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  Router,
  Route,
  UrlSegment
} from '@angular/router';
import {Observable, of} from 'rxjs';
import {switchMap} from "rxjs/operators";
import {AuthStore} from './auth.store';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authStore: AuthStore, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot,
              state: RouterStateSnapshot): Observable<boolean> {
    const redirectUrl = state.url === '/auth/login' ? '/' : state.url;
    return this.check(redirectUrl);
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.canActivate(childRoute, state);
  }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
    return this.check('/')
  }

  private check(redirectURL: string): Observable<boolean> {
    let canAccess = false
    if (this.authStore.accessToken) {
      canAccess = true
    }
    return of(canAccess).pipe(
      switchMap((authenticated) => {
        if (!authenticated) {
          this.router.navigate(['/auth', 'login'], {
            queryParams: {
              returnUrl: redirectURL
            }
          }).then();
        }
        return of(authenticated);
      })
    );
  }
}
