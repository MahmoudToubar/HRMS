import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateDepartment } from '../../shared/models/createDepartment';
import { Department } from '../../shared/models/department';
import { UpdateDepartment } from '../../shared/models/updateDepartment';
import { Pagination } from '../../shared/models/pagination';
import { LookupSpecParams } from '../../shared/models/lookupSpecParams';


@Injectable({
  providedIn: 'root'
})


export class DepartmentsService {
  private baseUrl = `${environment.apiUrl}/departments`;

  constructor(private http: HttpClient) {}


  getDepartments(params: LookupSpecParams): Observable<Pagination<Department>> {
    return this.http.get<Pagination<Department>>(this.baseUrl, {
      params: { pageIndex: params.pageIndex, pageSize: params.pageSize,
        ...(params.search && { search: params.search }),
        ...(params.sort && { sort: params.sort })
      }
    });
  }


  getDepartmentById(id: number): Observable<Department> {
    return this.http.get<Department>(`${this.baseUrl}/${id}`);
  }


  createDepartment(dto: CreateDepartment): Observable<Department> {
    return this.http.post<Department>(this.baseUrl, dto);
  }


  updateDepartment(id: number, dto: UpdateDepartment): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
  }


  deleteDepartment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
