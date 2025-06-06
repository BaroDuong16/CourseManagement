import { Component } from '@angular/core';
import { RegisterReq } from '../../../Models/RegisterReq';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
@Component({
  selector: 'app-register',
  imports: [FormsModule, CommonModule, RouterModule, ReactiveFormsModule, NzButtonModule, NzCheckboxModule, NzFormModule, NzInputModule, NzLayoutModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  user: RegisterReq = {
    fullname: '',
    username: '',
    phonenumber: '',
    email: '',
    password: '',
    role: '',
  }

  constructor(private auth: AuthService, private router: Router) {}

  register() {
    this.auth.register(this.user).subscribe({
      next: () => {
        alert('Registered successfully!');
        this.router.navigate(['/welcome']);
      },
      error: err => alert(err.error)
    });
  }
}
