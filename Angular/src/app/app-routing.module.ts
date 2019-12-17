import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { AuthGuard } from './auth/auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { ScanPanelCreateComponent } from './scan-panel/scan-panel-create/scan-panel-create.component';
import { ScanPanelListComponent } from './scan-panel/scan-panel-list/scan-panel-list.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';

const routes: Routes = [
  { path: '', redirectTo: '/user/login', pathMatch: 'full' },
  {
    path: 'user', component: UserComponent,
    children: [
      { path: 'registration', component: RegistrationComponent },
      { path: 'login', component: LoginComponent }
    ]
  },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'forbidden', component: ForbiddenComponent },
  { path: 'adminpanel', component: AdminPanelComponent, canActivate: [AuthGuard], data: { permittedRoles: ['Admin'] } },
  { path: 'scan-create', component: ScanPanelCreateComponent, canActivate: [AuthGuard], data: { permittedRoles: ['Admin'] } },
  { path: 'scan-list', component: ScanPanelListComponent, canActivate: [AuthGuard], data: { permittedRoles: ['Admin'] } },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
