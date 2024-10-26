import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class AuthStoreService {
  private readonly ACCESS_TOKEN_KEY = 'access_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private _accessToken: string | null = null;
  private _refreshToken: string | null = null;


  constructor() {
    this._accessToken = localStorage.getItem(this.ACCESS_TOKEN_KEY)
    this._refreshToken = localStorage.getItem(this.REFRESH_TOKEN_KEY)
  }

  get accessToken(): string | null {
    return this._accessToken
  }

  set accessToken(value: string) {
    this._accessToken = value;
    localStorage.setItem(this.ACCESS_TOKEN_KEY, value);
  }

  get refreshToken(): string | null {
    return this._refreshToken;
  }

  set refreshToken(value: string) {
    this._refreshToken = value;
    localStorage.setItem(this.REFRESH_TOKEN_KEY, value);
  }

  clearSession() {
    this._refreshToken = null;
    this._accessToken = null;
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
  }
}
