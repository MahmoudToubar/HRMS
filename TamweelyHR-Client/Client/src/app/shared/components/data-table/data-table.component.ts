import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DataTableColumn } from '../../models/interfaces/dataTableColumn';
import { PaginationData } from '../../models/interfaces/paginationData';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './data-table.component.html',
  styleUrl: './data-table.component.css'
})
export class DataTableComponent<TSort = any> {

  @Input() columns: DataTableColumn<TSort>[] = [];
  @Input() data: any[] = [];
  @Input() pagination?: PaginationData;
  @Input() showActions = true;

  @Input() pageSizeOptions: number[] = [5, 10, 20, 50];
  @Output() pageChange = new EventEmitter<{ pageIndex: number; pageSize: number }>();
  @Output() sortChange = new EventEmitter<TSort>();
  @Output() edit = new EventEmitter<any>();
  @Output() delete = new EventEmitter<any>();

  currentSortKey?: any;
  currentSortDirection: 'asc' | 'desc' = 'asc';

  onSort(column: DataTableColumn<TSort>): void {
    if (!column.sortable || !column.sortKey) return;

    if (this.currentSortKey === column.sortKey) {
      this.currentSortDirection =
        this.currentSortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.currentSortKey = column.sortKey;
      this.currentSortDirection = 'asc';
    }

    const sortValue =
      this.currentSortDirection === 'asc'
        ? column.sortKey
        : (column.sortKey + 'Desc') as TSort;

    this.sortChange.emit(sortValue);
  }

  changePage(pageIndex: number): void {
    if (!this.pagination) return;
    this.pageChange.emit({
      pageIndex,
      pageSize: this.pagination.pageSize
    });
  }

  get totalPages(): number {
    if (!this.pagination) return 0;
    return Math.ceil(this.pagination.totalCount / this.pagination.pageSize);
  }

  get pages(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  changePageSize(event: Event): void {
    if (!this.pagination) return;

    const pageSize = +(event.target as HTMLSelectElement).value;

    this.pageChange.emit({
      pageIndex: 1,
      pageSize
    });
  }

}
