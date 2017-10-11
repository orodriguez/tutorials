import { Observable } from 'rxjs/Observable';
import { last } from '@angular/router/src/utils/collection';
import { IUser } from './user.model';
import { Injectable } from '@angular/core';
import { RequestOptions, Headers, Http } from '@angular/http';

@Injectable()
export class AuthService {
  public currentUser: IUser;

  constructor(private http: Http) { }

  loginUser(userName: string, password: string) {
    const loginInfo = { username: userName, password };
    
    return this.http
      .post('/api/login', loginInfo, new RequestOptions({
        headers: new Headers({
          'Content-Type': 'application/json'
        })
      }))
      .do(resp => {
        if (resp) {
          this.currentUser = resp.json().user as IUser;
        }
      })
      .catch(error => {
        return Observable.of(false);
      });
  }

  isAuthenticated() {
    return !!this.currentUser;
  }

  checkAuthenticationStatus() {
    return this.http
      .get('/api/currentIdentity')
      .map((resp: any) => {
        if (resp._body) {
          return resp.json();
        } else {
          return {};
        }
      })
      .do(currentUser => {
        if(!!currentUser.userName) {
          this.currentUser = currentUser;
        }
      })
      .subscribe();
  }

  updateCurrentUser(firstName: string, lastName: string) {
    this.currentUser.firstName = firstName;
    this.currentUser.lastName = lastName;

    return this.http
      .put(`/api/users/${this.currentUser.id}`, 
          JSON.stringify(this.currentUser), 
          new RequestOptions({
            headers: new Headers({
              'Content-Type': 'application/json'
          })
      }));
  }

  logout() {
    this.currentUser = undefined;

    return this.http
      .post('/api/logout', {}, new RequestOptions({
        headers: new Headers({
          'Content-Type': 'application/json'
        })
      }));
  }
}