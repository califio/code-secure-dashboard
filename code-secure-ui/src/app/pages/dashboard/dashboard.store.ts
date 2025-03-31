import {Injectable, signal} from '@angular/core';
import {SourceControlSummary} from '../../api/models/source-control-summary';

@Injectable({
  providedIn: 'root'
})
export class DashboardStore {
  textColor = signal<string>('');
  borderColor = signal<string>('');
  sourceControls = signal<SourceControlSummary[]>([]);
}
