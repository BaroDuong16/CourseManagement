import { Routes } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
import { NoAuthGuard } from './auth/noauth.guard';

export const routes: Routes = [

  {
    path: "", 
    loadChildren: () => import('./auth/login/login.routes').then(m => m.LOGIN_ROUTES), 
    canActivate: [NoAuthGuard]
  },
  
  {
    path: 'register', 
    loadChildren: () => import ('./auth/register/register.routes').then(m => m.REGISTER_ROUTES), 
    canActivate: [NoAuthGuard]
  },
  
  {
    path: 'welcome', 
    loadChildren: () => import('./pages/welcome/welcome.routes').then(m => m.WELCOME_ROUTES), 
    canActivate: [AuthGuard] 
  },
  
  {
    path: 'course-table', 
    loadChildren: () => import('./course-table/course-table.routes').then(m => m.COURSETABLE_ROUTES), 
    canActivate: [AuthGuard] 
  }
];
