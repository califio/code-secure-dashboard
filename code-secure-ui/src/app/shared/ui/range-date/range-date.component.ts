import {Component, computed, EventEmitter, Inject, LOCALE_ID, model, Output, ViewChild} from '@angular/core';
import {RangeDateState} from './range-date.model';
import {Popover} from 'primeng/popover';
import {Button} from 'primeng/button';
import {formatDate, NgClass} from '@angular/common';
import {DatePicker} from 'primeng/datepicker';
import {FormsModule} from '@angular/forms';
import {getRangeDate, RangeDateType} from '../../date-util';

@Component({
  selector: 'range-date',
  imports: [
    Popover,
    Button,
    DatePicker,
    FormsModule,
    NgClass
  ],
  templateUrl: './range-date.component.html',
  standalone: true,
  styleUrl: './range-date.component.scss'
})
export class RangeDateComponent {
  @ViewChild('op') op!: Popover;
  type = model(RangeDateType.Last30Days);
  startDate = model<Date>();
  endDate = model<Date>();

  currentLabel = computed(() => {
    if (this.type() != RangeDateType.Custom) {
      return this.mRangeDateLabel.get(this.type())
    }
    if (this.type() == RangeDateType.Custom) {
      return `${formatDate(this.startDate()!, 'dd/MM/yyyy', this.locale)} - ${formatDate(this.endDate()!, 'dd/MM/yyyy', this.locale)}`;
    }
    return 'Select'
  });
  @Output()
  onChange = new EventEmitter<RangeDateState>();
  rangeDate: Date[] = [];
  rangeDateLabel = '';
  rangeDateTypes: RangeDateType[] = [
    RangeDateType.Last7Days,
    RangeDateType.ThisWeek,
    RangeDateType.Last30Days,
    RangeDateType.ThisMonth,
    RangeDateType.ThisYear,
    RangeDateType.LastYear,
  ];
  mRangeDateLabel = new Map<RangeDateType, string>([
    [RangeDateType.Last7Days, 'Last 7 Days'],
    [RangeDateType.ThisWeek, 'This Week'],
    [RangeDateType.Last30Days, 'Last 30 Days'],
    [RangeDateType.ThisMonth, 'This Month'],
    [RangeDateType.ThisYear, 'This Year'],
    [RangeDateType.LastYear, 'Last Year'],
  ]);
  showCustomRangeDate = false;

  constructor(@Inject(LOCALE_ID) private locale: string) {
  }

  changeDateRange(type: RangeDateType) {
    if (type != RangeDateType.Custom) {
      const {startDate, endDate} = getRangeDate(type);
      this.type.set(type);
      this.startDate.set(startDate);
      this.endDate.set(endDate);
      this.onChange.emit({
        type: type,
        startDate: startDate,
        endDate: endDate
      });
      this.op.hide();
      this.showCustomRangeDate = false;
    }
  }

  updateCustomRangeDateLabel() {
    if (this.rangeDate) {
      let result = '';
      if (this.rangeDate.length > 0) {
        result = formatDate(this.rangeDate[0], 'dd/MM/yyyy', this.locale);
      }
      if (this.rangeDate.length > 1 && this.rangeDate[1] > this.rangeDate[0]) {
        result += ` - ${formatDate(this.rangeDate[1], 'dd/MM/yyyy', this.locale)}`;
      }
      this.rangeDateLabel = result;
    }
    this.rangeDateLabel = '';
  }

  onCustomRangeDateChange() {
    if (this.rangeDate.length == 2 && this.rangeDate[1] > this.rangeDate[0]) {
      this.startDate.set(this.rangeDate[0]);
      this.endDate.set(this.rangeDate[1]);
      this.type.set(RangeDateType.Custom);
      this.onChange.emit({
        type: RangeDateType.Custom,
        startDate: this.rangeDate[0],
        endDate: this.rangeDate[1]
      });
      this.op.hide();
    }
  }

  onCustomRangeDateCancel() {
    this.op.hide();
    this.showCustomRangeDate = false;
  }

  protected readonly RangeDateType = RangeDateType;
}
