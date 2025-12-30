import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { JobsService } from '../../../core/services/jobs.service';
import { DataTableColumn } from '../../../shared/models/interfaces/dataTableColumn';
import { PaginationData } from '../../../shared/models/interfaces/paginationData';
import { Job } from '../../../shared/models/job';
import { LookupSpecParams } from '../../../shared/models/lookupSpecParams';
import { ConfirmDialogComponent } from "../../../shared/components/confirm-dialog/confirm-dialog.component";
import { DataTableComponent } from "../../../shared/components/data-table/data-table.component";

@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [ConfirmDialogComponent, DataTableComponent, RouterModule],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.css'
})
export class JobListComponent implements OnInit {

  jobs: Job[] = [];

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
  selectedJobId?: number;

  loading = false;

  constructor(
    private jobsService: JobsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadJobs();
  }

  loadJobs(): void {
    this.loading = true;

    this.jobsService.getJobs(this.params).subscribe({
      next: res => {
        this.jobs = res.data;
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
    this.loadJobs();
  }


  onSort(field: string): void {
    this.params.sort = field;
    this.loadJobs();
  }


  onPageChange(event: { pageIndex: number; pageSize: number }): void {
    this.params.pageIndex = event.pageIndex;
    this.params.pageSize = event.pageSize;
    this.loadJobs();
  }


  onAdd(): void {
    this.router.navigate(['/jobs/create']);
  }


  onEdit(job: Job): void {
    this.router.navigate(['/jobs/edit', job.id]);
  }


  onDelete(job: Job): void {
    this.selectedJobId = job.id;
    this.showConfirm = true;
  }

  onConfirmDelete(result: boolean): void {
    this.showConfirm = false;

    if (!result || !this.selectedJobId) return;

    this.jobsService
      .deleteJob(this.selectedJobId)
      .subscribe(() => this.loadJobs());
  }

}
