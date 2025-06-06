import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class NoAuthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(): boolean {
    const token = localStorage.getItem('token');
    if (token) {
      // Nếu đã đăng nhập, chuyển hướng về trang chính
      this.router.navigate(['/welcome']);
      return false;
    }
    // Nếu chưa đăng nhập, cho phép truy cập login/register
    return true;
  }
}
