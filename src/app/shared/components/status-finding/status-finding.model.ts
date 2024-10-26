export type StatusFinding = "open" | "confirmed" | "fixed" | "false_positive" | "ignore"

export interface StatusFindingOption {
  status: StatusFinding
  label: string
  description: string
  icon: string
  style: string
}
