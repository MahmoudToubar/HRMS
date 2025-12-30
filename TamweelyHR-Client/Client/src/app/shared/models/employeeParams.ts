export interface EmployeeParams {
  pageIndex?: number;
  pageSize?: number;
  search?: string;
  departmentIds?: number[];
  jobIds?: number[];
  dateOfBirthFrom?: string;
  dateOfBirthTo?: string;
  sort?: 'name' | 'email' | 'dateOfBirth';
}
