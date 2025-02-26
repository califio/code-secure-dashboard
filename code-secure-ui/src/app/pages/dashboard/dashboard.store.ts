import {Injectable, signal} from '@angular/core';
import {SourceControl} from '../../api/models/source-control';

@Injectable({
  providedIn: 'root'
})
export class DashboardStore {
  textColor = signal<string>('');
  borderColor = signal<string>('');
  sourceControls = signal<SourceControl[]>([]);
}
