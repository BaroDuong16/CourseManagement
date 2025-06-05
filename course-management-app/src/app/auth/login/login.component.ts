import { Component } from '@angular/core';
import { LoginReq } from '../../../Models/LoginReq';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  credentials: LoginReq = {
    email: '',
    password: ''
  };

  constructor(private auth: AuthService, private router: Router) {}

  login() {
    this.auth.login(this.credentials).subscribe({
      next: (res: any) => {
        this.auth.saveToken(res.token);
        alert('Login successful!');
        this.router.navigate(['/welcome']);
      },
      error: (err) => alert(err.error)
    });
  }
}