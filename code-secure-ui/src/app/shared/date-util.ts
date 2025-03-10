export enum RangeDateType {
  Last7Days,
  ThisWeek,
  Last30Days,
  ThisMonth,
  ThisYear,
  LastYear,
  AllTime,
  Custom
}

export function getRangeDate(rangeType: RangeDateType): { startDate?: Date, endDate?: Date } {
  const today = new Date();
  let startDate: Date | undefined;
  let endDate: Date | undefined = today;
  switch (rangeType) {
    case RangeDateType.Last7Days:
      startDate = new Date();
      startDate.setDate(today.getDate() - 7);
      break;

    case RangeDateType.ThisWeek:
      startDate = new Date();
      startDate.setDate(today.getDate() - today.getDay());
      break;

    case RangeDateType.Last30Days:
      startDate = new Date();
      startDate.setDate(today.getDate() - 30);
      break;

    case RangeDateType.ThisMonth:
      startDate = new Date(today.getFullYear(), today.getMonth(), 1); // Ngày đầu tháng
      break;

    case RangeDateType.ThisYear:
      startDate = new Date(today.getFullYear(), 0, 1); // Ngày đầu năm
      break;

    case RangeDateType.LastYear:
      startDate = new Date(today.getFullYear() - 1, 0, 1); // Ngày đầu năm ngoái
      endDate = new Date(today.getFullYear() - 1, 11, 31); // Ngày cuối năm ngoái
      break;
    case RangeDateType.AllTime:
      startDate = undefined;
      endDate = undefined;
      break;
    default:
      startDate = new Date();
      startDate.setDate(today.getDate() - 30);
  }

  return { startDate, endDate };
}
