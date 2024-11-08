import {Component, OnDestroy, OnInit} from '@angular/core';
import {FindingDetailComponent} from '../../../shared/components/finding/finding-detail/finding-detail.component';
import {FindingService} from '../../../api/services/finding.service';
import {getPathParam} from '../../../core/router';
import {filter, Subject, switchMap, takeUntil} from 'rxjs';
import {FindingDetail} from '../../../api/models/finding-detail';

@Component({
  selector: 'app-finding',
  standalone: true,
  imports: [
    FindingDetailComponent
  ],
  templateUrl: './finding.component.html',
  styleUrl: './finding.component.scss'
})
export class FindingComponent implements OnInit, OnDestroy {
  finding: FindingDetail = {};
  constructor(
    private findingService: FindingService
  ) {
    getPathParam("id").pipe(
      filter(value => value != null),
      switchMap(findingId => {
        return findingService.getFinding({id: findingId})
      }),
      takeUntil(this.destroy$)
    ).subscribe(finding => {
      this.finding = finding;
    })
  }

  ngOnInit(): void {

  }

  private destroy$ = new Subject();

  ngOnDestroy(): void {
    this.destroy$.next(null);
    this.destroy$.complete();
  }
}
