import {Injectable, signal} from "@angular/core";
import {UserProfile} from '../../api/models/user-profile';

@Injectable({
  providedIn: 'root'
})
export class AuthStore {
  private readonly ACCESS_TOKEN_KEY = 'access_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private _accessToken: string | null = null;
  currentUser = signal<UserProfile>({});

  constructor() {
    this._accessToken = localStorage.getItem(this.ACCESS_TOKEN_KEY)
  }

  get accessToken(): string | null {
    return this._accessToken
  }

  set accessToken(value: string) {
    this._accessToken = value;
    localStorage.setItem(this.ACCESS_TOKEN_KEY, value);
  }

  get refreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY)
  }

  set refreshToken(value: string) {
    localStorage.setItem(this.REFRESH_TOKEN_KEY, value);
  }

  clearSession() {
    this._accessToken = null;
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
  }
}
