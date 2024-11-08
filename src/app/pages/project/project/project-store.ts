import {Injectable, signal} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProjectStore {
  slug = signal('')
  constructor() { }
}
