import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Employee } from '../../shared/models/employee';
import { Pagination } from '../../shared/models/pagination';
import { environment } from '../../../environments/environment';
import { CreateEmployee } from '../../shared/models/createEmployee';
import { EmployeeParams } from '../../shared/models/employeeParams';
import { UpdateEmployee } from '../../shared/models/updateEmployee';



@Injectable({
  providedIn: 'root'
})


export class EmployeesService {
  private baseUrl = `${environment.apiUrl}/employees`;

  constructor(private http: HttpClient) {}


  getEmployees(paramsModel: EmployeeParams): Observable<Pagination<Employee>> {
    let params = new HttpParams();

    if (paramsModel.pageIndex)
      params = params.set('pageIndex', paramsModel.pageIndex);

    if (paramsModel.pageSize)
      params = params.set('pageSize', paramsModel.pageSize);

    if (paramsModel.search)
      params = params.set('search', paramsModel.search);

    if (paramsModel.sort)
      params = params.set('sort', paramsModel.sort);

    if (paramsModel.dateOfBirthFrom)
      params = params.set('dateOfBirthFrom', paramsModel.dateOfBirthFrom);

    if (paramsModel.dateOfBirthTo)
      params = params.set('dateOfBirthTo', paramsModel.dateOfBirthTo);

    if (paramsModel.departmentIds?.length) {
      paramsModel.departmentIds.forEach(id => {
        params = params.append('departmentIds', id);
      });
    }

    if (paramsModel.jobIds?.length) {
      paramsModel.jobIds.forEach(id => {
        params = params.append('jobIds', id);
      });
    }

    return this.http.get<Pagination<Employee>>(this.baseUrl, { params });
  }


  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.baseUrl}/${id}`);
  }


  createEmployee(dto: CreateEmployee): Observable<void> {
    return this.http.post<void>(this.baseUrl, dto);
  }


  updateEmployee(id: number, dto: UpdateEmployee): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
  }


  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }


  exportEmployees(paramsModel: EmployeeParams): Observable<Blob> {
    let params = new HttpParams();

    Object.entries(paramsModel).forEach(([key, value]) => {
      if (value === undefined || value === null) return;

      if (Array.isArray(value)) {
        value.forEach(v => params = params.append(key, v));
      } else {
        params = params.set(key, value as any);
      }
    });

    return this.http.get(`${this.baseUrl}/export`, {
      params,
      responseType: 'blob'
    });
  }
}
