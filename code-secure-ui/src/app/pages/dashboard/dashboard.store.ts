import {Injectable, signal} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DashboardStore {
  textColor = signal<string>('');
  borderColor = signal<string>('');
}
