export interface ProjectSummary {
  id: any,
  slug: string,
  name: string
  last_scan?: Date,
  finding: number
}

export interface Scan {
  id: any,
  branch: string,
  scanner: string,
  scan_type: string,
  status: string,
  severity_critical: number,
  severity_high: number,
  severity_medium: number,
  severity_low: number,
  severity_info: number,
  last_scan: Date,
  duration: string
}
