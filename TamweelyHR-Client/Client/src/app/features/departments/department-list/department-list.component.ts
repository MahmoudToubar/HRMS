import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { DepartmentsService } from '../../../core/services/departments.service';
import { Department } from '../../../shared/models/department';
import { DataTableColumn } from '../../../shared/models/interfaces/dataTableColumn';
import { PaginationData } from '../../../shared/models/interfaces/paginationData';
import { LookupSpecParams } from '../../../shared/models/lookupSpecParams';
import { DataTableComponent } from "../../../shared/components/data-table/data-table.component";
import { ConfirmDialogComponent } from "../../../shared/components/confirm-dialog/confirm-dialog.component";

@Component({
  selector: 'app-department-list',
  standalone: true,
  imports: [DataTableComponent, ConfirmDialogComponent, RouterModule],
  templateUrl: './department-list.component.html',
  styleUrl: './department-list.component.css'
})
export class DepartmentListComponent implements OnInit {

  departments: Department[] = [];

  columns: DataTableColumn[] = [
    { field: 'name', header: 'Name', sortable: true, sortKey: 'name' }
  ];

  pagination: PaginationData = {
    pageIndex: 1,
    pageSize: 10,
    totalCount: 0
  };

  params: LookupSpecParams = {
    pageIndex: 1,
    pageSize: 5,
    search: '',
    sort: 'name'
  };


  showConfirm = false;
  selectedDepartmentId?: number;

  loading = false;

  constructor(
    private departmentsService: DepartmentsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.loading = true;

    this.departmentsService.getDepartments(this.params).subscribe({
      next: res => {
        this.departments = res.data;
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
    this.loadDepartments();
  }


  onSort(field: string): void {
    this.params.sort = field;
    this.loadDepartments();
  }


  onPageChange(event: { pageIndex: number; pageSize: number }): void {
    this.params.pageIndex = event.pageIndex;
    this.params.pageSize = event.pageSize;
    this.loadDepartments();
  }


  onAdd(): void {
    this.router.navigate(['/departments/create']);
  }

 
  onEdit(department: Department): void {
    this.router.navigate(['/departments/edit', department.id]);
  }


  onDelete(department: Department): void {
    this.selectedDepartmentId = department.id;
    this.showConfirm = true;
  }

  onConfirmDelete(result: boolean): void {
    this.showConfirm = false;

    if (!result || !this.selectedDepartmentId) return;

    this.departmentsService
      .deleteDepartment(this.selectedDepartmentId)
      .subscribe(() => this.loadDepartments());
  }

}
