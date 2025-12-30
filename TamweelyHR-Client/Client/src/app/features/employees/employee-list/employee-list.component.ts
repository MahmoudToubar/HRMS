import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { DepartmentsService } from '../../../core/services/departments.service';
import { EmployeesService } from '../../../core/services/employees.service';
import { JobsService } from '../../../core/services/jobs.service';
import { Department } from '../../../shared/models/department';
import { Employee } from '../../../shared/models/employee';
import { DataTableColumn } from '../../../shared/models/interfaces/dataTableColumn';
import { PaginationData } from '../../../shared/models/interfaces/paginationData';
import { Job } from '../../../shared/models/job';
import { LookupSpecParams } from '../../../shared/models/lookupSpecParams';
import { EmployeeParams } from '../../../shared/models/employeeParams';
import { DataTableComponent } from "../../../shared/components/data-table/data-table.component";
import { ConfirmDialogComponent } from "../../../shared/components/confirm-dialog/confirm-dialog.component";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [DataTableComponent, ConfirmDialogComponent, CommonModule, FormsModule],
  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.css'
})
export class EmployeeListComponent implements OnInit {

  employees: Employee[] = [];

  columns: DataTableColumn<EmployeeParams['sort']>[] = [
  { field: 'fullName', header: 'Full Name', sortable: true, sortKey: 'name' },
  { field: 'email', header: 'Email', sortable: true, sortKey: 'email' },
  { field: 'mobile', header: 'Mobile' },
  { field: 'departmentName', header: 'Department' },
  { field: 'jobName', header: 'Job' },
  { field: 'dateOfBirth', header: 'Birth Date', sortable: true, sortKey: 'dateOfBirth', type: 'date' }
];

  pagination: PaginationData = {
    pageIndex: 1,
    pageSize: 5,
    totalCount: 0
  };

  params: EmployeeParams = {
    pageIndex: 1,
    pageSize: 5,
    search: '',
    departmentIds: [],
    jobIds: [],
    sort: 'name'
  };


  departments: Department[] = [];
  jobs: Job[] = [];


  showConfirm = false;
  selectedEmployeeId?: number;

  loading = false;

  constructor(
    private employeesService: EmployeesService,
    private departmentsService: DepartmentsService,
    private jobsService: JobsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
    this.loadLookups();
  }



  loadEmployees(): void {
    this.loading = true;

    this.employeesService.getEmployees(this.params).subscribe({
      next: res => {
        this.employees = res.data;
        this.pagination.totalCount = res.count;
        this.pagination.pageIndex = res.pageIndex;
        this.pagination.pageSize = res.pageSize;
        this.loading = false;
      },
      error: () => (this.loading = false)
    });
  }


  onSearch(value: string): void {
    this.params.search = value;
    this.params.pageIndex = 1;
    this.loadEmployees();
  }


  onSort(field: EmployeeParams['sort']): void {
  this.params.sort = field;
  this.loadEmployees();
}



  onPageChange(event: { pageIndex: number; pageSize: number }): void {
    this.params.pageIndex = event.pageIndex;
    this.params.pageSize = event.pageSize;
    this.loadEmployees();
  }


  onDepartmentChange(departmentId: string): void {
    this.params.departmentIds = departmentId ? [+departmentId] : [];
    this.params.pageIndex = 1;
    this.loadEmployees();
  }

 
  onJobChange(jobId: string): void {
    this.params.jobIds = jobId ? [+jobId] : [];
    this.params.pageIndex = 1;
    this.loadEmployees();
  }


  onAdd(): void {
    this.router.navigate(['/employees/create']);
  }


  onEdit(employee: Employee): void {
    this.router.navigate(['/employees/edit', employee.id]);
  }

 
  onDelete(employee: Employee): void {
    this.selectedEmployeeId = employee.id;
    this.showConfirm = true;
  }

  onConfirmDelete(result: boolean): void {
    this.showConfirm = false;

    if (!result || !this.selectedEmployeeId) return;

    this.employeesService
      .deleteEmployee(this.selectedEmployeeId)
      .subscribe(() => this.loadEmployees());
  }



  private loadLookups(): void {
    const lookupParams: LookupSpecParams = {
      pageIndex: 1,
      pageSize: 50,
      sort: 'name'
    };

    this.departmentsService.getDepartments(lookupParams).subscribe({
      next: res => (this.departments = res.data)
    });

    this.jobsService.getJobs(lookupParams).subscribe({
      next: res => (this.jobs = res.data)
    });
  }



  exportEmployees(): void {
  this.employeesService.exportEmployees(this.params).subscribe({
    next: (blob: Blob) => {
      const url = window.URL.createObjectURL(blob);

      const a = document.createElement('a');
      a.href = url;
      a.download = 'Employees.xlsx';
      a.click();

      window.URL.revokeObjectURL(url);
    },
    error: err => console.error(err)
  });
}

}
