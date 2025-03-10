import {Component, OnDestroy, signal} from '@angular/core';
import {FindingDetailComponent} from '../../../shared/components/finding/finding-detail/finding-detail.component';
import {getPathParam} from '../../../core/router';
import {filter, Subject, takeUntil} from 'rxjs';
import {Panel} from 'primeng/panel';

@Component({
  selector: 'app-finding',
  standalone: true,
  imports: [
    FindingDetailComponent,
    Panel
  ],
  templateUrl: './finding.component.html',
})
export class FindingComponent implements OnDestroy {
  findingId = signal<string>('');

  constructor() {
    getPathParam("id").pipe(
      filter(value => value != null),
      takeUntil(this.destroy$)
    ).subscribe(findingId => {
      this.findingId.set(findingId);
    });
  }

  private destroy$ = new Subject();

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }
}
