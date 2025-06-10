import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient, private router: Router) {}

  register(data: any) {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  login(data: any) {
    return this.http.post(`${this.apiUrl}/login`, data);
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/']);
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  /** ✅ Trích xuất tất cả roles từ token, có thể là string hoặc array */
 getRolesFromToken(): string[] {
  const token = this.getToken();
  if (!token) return [];

  try {
    const payload = JSON.parse(atob(token.split('.')[1]));

    const roleClaim =
      payload['role'] ||
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    if (!roleClaim) return [];

    if (typeof roleClaim === 'string') return [roleClaim];
    if (Array.isArray(roleClaim)) return roleClaim;

    return [];
  } catch (e) {
    console.error('Error decoding token', e);
    return [];
  }
}


  /** ✅ Trả về true nếu user có role Teacher */
  isTeacher(): boolean {
    return this.getRolesFromToken().includes('Teacher');
  }

  /** ✅ Trả về true nếu user có role Student */
  isStudent(): boolean {
    return this.getRolesFromToken().includes('Student');
  }
}
