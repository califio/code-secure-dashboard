import {Component, computed, EventEmitter, Inject, input, LOCALE_ID, model, Output, ViewChild} from '@angular/core';
import {RangeDateState} from './range-date.model';
import {Popover} from 'primeng/popover';
import {Button} from 'primeng/button';
import {formatDate, NgClass} from '@angular/common';
import {DatePicker} from 'primeng/datepicker';
import {FormsModule} from '@angular/forms';
import {getRangeDate, RangeDateType} from '../../date-util';
import {FloatLabel} from 'primeng/floatlabel';
import {InputText} from 'primeng/inputtext';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';

@Component({
  selector: 'range-date',
  imports: [
    Popover,
    Button,
    DatePicker,
    FormsModule,
    NgClass,
    FloatLabel,
    InputText,
    IconField,
    InputIcon
  ],
  templateUrl: './range-date.component.html',
  standalone: true,
  styleUrl: './range-date.component.scss'
})
export class RangeDateComponent {
  showClear = input(false);
  styleClassInput = input('');
  label = input('Range Date');
  labelType = input<'button' | 'text'>('button');
  severity = input<'primary' | 'secondary' | 'success' | 'info' | 'warn' | 'danger' | 'contrast' | null | undefined>('primary');
  @ViewChild('op') op!: Popover;
  type = model<RangeDateType | null | undefined>(RangeDateType.Last30Days);

  startDate = model<Date | null>();
  endDate = model<Date | null>();

  currentLabel = computed(() => {
    const dateType = this.type();
    if (dateType != null) {
      if (dateType != RangeDateType.Custom) {
        return this.mRangeDateLabel.get(dateType);
      }
      if (dateType== RangeDateType.Custom) {
        return `${formatDate(this.startDate()!, 'dd/MM/yyyy', this.locale)} - ${formatDate(this.endDate()!, 'dd/MM/yyyy', this.locale)}`;
      }
    }
    if (this.labelType() == "button") {
      return this.label()
    }
    return '';
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
    RangeDateType.AllTime
  ];
  mRangeDateLabel = new Map<RangeDateType, string>([
    [RangeDateType.Last7Days, 'Last 7 Days'],
    [RangeDateType.ThisWeek, 'This Week'],
    [RangeDateType.Last30Days, 'Last 30 Days'],
    [RangeDateType.ThisMonth, 'This Month'],
    [RangeDateType.ThisYear, 'This Year'],
    [RangeDateType.LastYear, 'Last Year'],
    [RangeDateType.AllTime, 'All Time'],
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

  clearRangeDate() {
    this.onChange.emit({
      type: null,
      startDate: null,
      endDate: null
    });
    this.op.hide();
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
