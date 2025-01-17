import {FindingStatus} from '../../../../api/models/finding-status';

export interface FindingStatusOption {
  status: FindingStatus
  label: string
  description: string
  icon: string
  style: string
}
