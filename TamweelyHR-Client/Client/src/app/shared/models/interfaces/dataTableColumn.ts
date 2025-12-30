export interface DataTableColumn<TSort = string> {
  field: string;    
  header: string;
  sortable?: boolean;
  sortKey?: TSort;
  type?: 'text' | 'date';    
}
