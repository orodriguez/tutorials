import { AuthService } from './user/auth.service';
import { Component, OnInit } from '@angular/core';
@Component({
  selector: 'events-app',
  templateUrl: 'app/events-app.component.html'
})
export class EventsAppComponent implements OnInit {
  constructor(private auth: AuthService) {}

  ngOnInit(): void {
    this.auth.checkAuthenticationStatus();
  }
}