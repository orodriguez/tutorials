import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login.component';
import { ProfileComponent } from './profile.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from "@angular/router";
import { userRoutes } from "./user.route";

@NgModule({
  declarations: [ 
    ProfileComponent,
    LoginComponent
  ],
  imports: [ 
    CommonModule, 
    RouterModule.forChild(userRoutes),
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [],
  providers: [],
})
export class UserModule {}