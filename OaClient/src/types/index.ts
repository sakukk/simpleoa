export enum ApplicationStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2
}

export enum ApplicationType {
  AnnualLeave = 0,
  PersonalLeave = 1
}

export const getStatusLabel = (status: ApplicationStatus): string => {
  switch (status) {
    case ApplicationStatus.Pending:
      return 'Pending'
    case ApplicationStatus.Approved:
      return 'Approved'
    case ApplicationStatus.Rejected:
      return 'Rejected'
    default:
      return 'Unknown'
  }
}

export const getTypeLabel = (type: ApplicationType): string => {
  switch (type) {
    case ApplicationType.AnnualLeave:
      return 'Annual Leave'
    case ApplicationType.PersonalLeave:
      return 'Personal Leave'
    default:
      return 'Unknown'
  }
} 