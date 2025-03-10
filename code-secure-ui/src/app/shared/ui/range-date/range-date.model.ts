import {RangeDateType} from '../../date-util';

export interface RangeDateState {
  type?: RangeDateType | null
  startDate?: Date | null,
  endDate?: Date | null
}
