import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  templateUrl: 'app/user/login.component.html',
  styles: [`
    em { float: right; color: #E05C65; padding-left: 10px; }
  `]
})
export class LoginComponent implements OnInit {
  loginInvalid = false;

  constructor(
    private authService: AuthService,
    private router: Router) { }

  ngOnInit() { }

  login(formValues) {
    this.authService.loginUser(
        formValues.userName,
        formValues.password)
      .subscribe(resp => {
        if (!resp) {
          this.loginInvalid = true;
          return;
        }
        
        this.router.navigate(['events']);
      });
  }

  cancel() {
    this.router.navigate(['events']);
  }
}