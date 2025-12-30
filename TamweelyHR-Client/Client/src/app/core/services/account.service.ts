import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest } from '../../shared/models/loginRequest';
import { LoginResponse } from '../../shared/models/loginResponse';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<string | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {
    const token = localStorage.getItem('token');
    if (token) {
      this.currentUserSource.next(token);
    }
  }

  login(model: LoginRequest) {
    return this.http.post<LoginResponse>(
      `${this.baseUrl}/account/login`,
      model
    ).pipe(
      map(response => {
        if (response?.token) {
          localStorage.setItem('token', response.token);
          this.currentUserSource.next(response.token);
        }
        return response;
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));

    return (
      payload.role ||
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
      null
    );
  }


  isAdmin(): boolean {
    return this.getUserRole() === 'Admin';
  }

  isUser(): boolean {
    return this.getUserRole() === 'User';
  }

}
