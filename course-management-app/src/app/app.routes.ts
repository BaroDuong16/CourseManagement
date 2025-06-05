import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { AuthGuard } from './auth/auth.guard';

export const routes: Routes = [

  {path: "", component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'welcome', loadChildren: () => import('./pages/welcome/welcome.routes').then(m => m.WELCOME_ROUTES), canActivate: [AuthGuard] },
  {path: 'course-table', loadChildren: () => import('./course-table/course-table.routes').then(m => m.COURSETABLE_ROUTES), canActivate: [AuthGuard] }
];
