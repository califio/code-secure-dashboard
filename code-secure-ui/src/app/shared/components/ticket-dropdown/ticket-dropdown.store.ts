import {Injectable, signal} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TicketDropdownStore {
  loading = signal(false);

  constructor() {
  }
}
