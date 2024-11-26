import {Component, EventEmitter, Input, OnInit, Output, TemplateRef} from '@angular/core';
import {DatePipe, NgClass, NgTemplateOutlet} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {ClickOutsideDirective} from '../../directives/click-outside.directive';

@Component({
  selector: 'date-picker',
  standalone: true,
  imports: [
    NgClass,
    FormsModule,
    DatePipe,
    ClickOutsideDirective,
    NgTemplateOutlet
  ],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.scss'
})
export class DatePickerComponent implements OnInit {
  @Input() titleTemplate?: TemplateRef<any>;
  @Input() afterDateTemplate?: TemplateRef<any>;
  @Input() format = 'yyyy-MM-dd';
  @Output() dateChange = new EventEmitter<Date>();
  @Input() set date(date: Date | null | undefined) {
    this._date = date;
    this.initDate();
    console.log(date);
  }
  get date() {
    return this._date;
  }
  private _date: Date | null | undefined = null;
  currentMonth!: number;
  currentYear!: number;
  today = new Date();
  isCalendarOpen = false; // Trạng thái mở/đóng Calendar
  daysOfWeek = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];
  weeks: Array<Array<{ value: number; active: boolean }>> = [];

  get currentMonthName(): string {
    return new Intl.DateTimeFormat('en-US', {month: 'long'}).format(new Date(this.currentYear, this.currentMonth));
  }

  constructor() {
  }

  ngOnInit() {
    this.generateCalendar();
  }

  initDate() {
    this.today = new Date();
    let currentDate = this.today;
    if (this._date) {
      currentDate = this._date;
    }
    this.currentMonth = currentDate.getMonth();
    this.currentYear = currentDate.getFullYear();
  }

  toggleCalendar() {
    this.isCalendarOpen = !this.isCalendarOpen;
    if (this.isCalendarOpen) {
      this.initDate();
    }
  }

  closeCalendar() {
    this.isCalendarOpen = false;
  }

  generateCalendar() {
    const firstDay = new Date(this.currentYear, this.currentMonth, 1);
    const lastDay = new Date(this.currentYear, this.currentMonth + 1, 0);

    const startDay = firstDay.getDay() === 0 ? 6 : firstDay.getDay() - 1; // Adjust for Monday start
    const totalDays = lastDay.getDate();

    const days: Array<{ value: number; active: boolean }> = [];

    // Previous month's padding days
    for (let i = startDay; i > 0; i--) {
      const date = new Date(this.currentYear, this.currentMonth, -i + 1);
      days.push({value: date.getDate(), active: false});
    }

    // Current month's days
    for (let i = 1; i <= totalDays; i++) {
      days.push({value: i, active: true});
    }

    // Next month's padding days
    const nextMonthDays = 7 - (days.length % 7);
    if (nextMonthDays < 7) {
      for (let i = 1; i <= nextMonthDays; i++) {
        const date = new Date(this.currentYear, this.currentMonth + 1, i);
        days.push({value: date.getDate(), active: false});
      }
    }

    // Break into weeks
    this.weeks = [];
    while (days.length) {
      this.weeks.push(days.splice(0, 7));
    }
  }

  prevMonth() {
    if (this.currentMonth === 0) {
      this.currentMonth = 11;
      this.currentYear -= 1;
    } else {
      this.currentMonth -= 1;
    }
    this.generateCalendar();
  }

  nextMonth() {
    if (this.currentMonth === 11) {
      this.currentMonth = 0;
      this.currentYear += 1;
    } else {
      this.currentMonth += 1;
    }
    this.generateCalendar();
  }

  selectDate(date: { value: number; active: boolean }) {
    if (date.active) {
      this.date = new Date(this.currentYear, this.currentMonth, date.value);
      this.dateChange.emit(this.date);
      this.closeCalendar();
    }
  }

  getDayClasses(date: { value: number; active: boolean }): string {
    const baseClass = 'm-px size-10 flex justify-center items-center text-sm rounded-full text-gray-800 dark:text-neutral-200 hover:text-primary';
    const isCurrentDate =
      this.date?.getDate() === date.value &&
      this.date.getMonth() === this.currentMonth &&
      this.date.getFullYear() === this.currentYear;
    const isToday =
      this.today.getDate() === date.value &&
      this.today.getMonth() === this.currentMonth &&
      this.today.getFullYear() === this.currentYear;
    const currentDateClass = isCurrentDate ? 'bg-primary text-white hover:text-white' : '';
    const todayClass = isToday ? 'border border-primary' : 'border-none';
    let clazz = `${baseClass} ${todayClass}`;
    if (!date.active) {
      clazz += " opacity-50 cursor-not-allowed";
    } else {
      clazz += " cursor-pointer"
      if (isCurrentDate) {
        clazz += ` ${currentDateClass}`
      }
      if (isToday) {
        clazz += ` ${todayClass}`;
      }
    }
    return clazz;
  }
}
