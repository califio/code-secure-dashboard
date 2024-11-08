export interface Notification {
  message: string,
  type: ToastrType,
  duration: number
}
export enum ToastrType {
  Success = 0,
  Warning = 1,
  Error = 2
}
