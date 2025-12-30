import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateJob } from '../../shared/models/createJob ';
import { Job } from '../../shared/models/job';
import { UpdateJob } from '../../shared/models/updateJob';
import { Pagination } from '../../shared/models/pagination';
import { LookupSpecParams } from '../../shared/models/lookupSpecParams';


@Injectable({
  providedIn: 'root'
})


export class JobsService {
  private baseUrl = `${environment.apiUrl}/jobs`;

  constructor(private http: HttpClient) {}


  getJobs(params: LookupSpecParams): Observable<Pagination<Job>> {
    return this.http.get<Pagination<Job>>(this.baseUrl, {
      params: {pageIndex: params.pageIndex, pageSize: params.pageSize,
        ...(params.search && { search: params.search }),
        ...(params.sort && { sort: params.sort })
      }
    });
  }



  getJobById(id: number): Observable<Job> {
    return this.http.get<Job>(`${this.baseUrl}/${id}`);
  }


  createJob(dto: CreateJob): Observable<Job> {
    return this.http.post<Job>(this.baseUrl, dto);
  }


  updateJob(id: number, dto: UpdateJob): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
  }


  deleteJob(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
