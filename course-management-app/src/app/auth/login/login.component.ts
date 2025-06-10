import { Component } from '@angular/core';
import { LoginReq } from '../../../Models/LoginReq';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterModule, ReactiveFormsModule, NzButtonModule, NzCheckboxModule, NzFormModule, NzInputModule, NzLayoutModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  credentials: LoginReq = {
    email: '',
    password: ''
  };
  remember = true;

  constructor(private auth: AuthService, private router: Router) {}

    login() {
    this.auth.login(this.credentials).subscribe({
      next: (res: any) => {
        console.log('Login response:', res); // 👈 In ra toàn bộ response
        this.auth.saveToken(res.token);

        const token = this.auth.getToken();
        console.log('Saved token:', token); // 👈 Xác nhận token được lưu

        const roles = this.auth.getRolesFromToken();
        console.log('Decoded roles:', roles); // 👈 Phải in ra ["Teacher"] hoặc ["Student"]

        if (roles.includes('Teacher')) {
          this.router.navigate(['/course-table']);
        } else if (roles.includes('Student')) {
          this.router.navigate(['/welcome']);
        } else {
          this.router.navigate(['/']);
        }
      },
      error: (err) => alert(err.error)
    });
  }
}
