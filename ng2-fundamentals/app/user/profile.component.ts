import { Toastr, TOASTR_TOKEN } from '../common/toastr.service';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Component, Inject, OnInit } from '@angular/core';

@Component({
  templateUrl: 'app/user/profile.component.html',
  styles: [`
    em { float: right; color: #E05C65; padding-left: 10px; }
    .error input { background-color: #E3C3C5; }
  `]
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  firstName: FormControl;
  lastName: FormControl;

  constructor(private auth: AuthService, 
    private router:  Router, 
    @Inject(TOASTR_TOKEN) private toastr: Toastr) { }

  ngOnInit(): void {
    this.firstName = new FormControl(
      this.auth.currentUser.firstName, [
        Validators.required,
        Validators.pattern('[a-zA-Z].*')]);
    this.lastName = new FormControl(
      this.auth.currentUser.lastName,
      Validators.required);
    this.profileForm = new FormGroup({
      firstName: this.firstName,
      lastName: this.lastName
    });
  }

  cancel() {
    this.router.navigate(['events']);
  }

  validateFirstName() {
    return this.firstName.valid || this.firstName.untouched; 
  }

  validateLastName() {
    return this.lastName.valid || this.lastName.untouched;
  }

  saveProfile(formValues) {
    if (this.profileForm.valid)
      this.auth.updateCurrentUser(
          formValues.firstName,
          formValues.lastName)
        .subscribe(() => {
          this.toastr.success('Profile Saved');
        });
  }

  logout() {
    this.auth.logout().subscribe(() => {
      this.router.navigate(['/user/login']);
    });
  }
}
