import { LoginComponent } from './login.component';
import { ProfileComponent } from './profile.component';
import { Routes } from "@angular/router/public_api";

export const userRoutes: Routes = [
  { path: 'profile', component: ProfileComponent },
  { path: 'login', component: LoginComponent }
];