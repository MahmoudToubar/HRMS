import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { LoginComponent } from './features/account/login/login.component';
import { DepartmentFormComponent } from './features/departments/department-form/department-form.component';
import { DepartmentListComponent } from './features/departments/department-list/department-list.component';
import { EmployeeFormComponent } from './features/employees/employee-form/employee-form.component';
import { EmployeeListComponent } from './features/employees/employee-list/employee-list.component';
import { JobFormComponent } from './features/jobs/job-form/job-form.component';
import { JobListComponent } from './features/jobs/job-list/job-list.component';
import { roleGuard } from './core/guards/role.guard';
import { loginGuard } from './core/guards/login.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },


  {
    path: 'employees',
    component: EmployeeListComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin', 'User'] }
  },

  {
    path: 'employees/create',
    component: EmployeeFormComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin', 'User'] } 
  },

  {
    path: 'employees/edit/:id',
    component: EmployeeFormComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin', 'User'] }
  },


  {
    path: 'departments',
    component: DepartmentListComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] }
  },

  {
    path: 'departments/create',
    component: DepartmentFormComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] }
  },

  {
    path: 'departments/edit/:id',
    component: DepartmentFormComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] }
  },


  {
    path: 'jobs',
    component: JobListComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] }
  },

  {
    path: 'jobs/create',
    component: JobFormComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] }
  },
  
  {
    path: 'jobs/edit/:id',
    component: JobFormComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] }
  },

  { 
  path: 'login',
  component: LoginComponent,
  canActivate: [loginGuard]
},

  { path: '**', redirectTo: 'employees' }
];
