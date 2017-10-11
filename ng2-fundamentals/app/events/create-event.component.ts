import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { EventService } from './index';

@Component({
  templateUrl: 'app/events/create-event.component.html',
  styles: [`
    em { float: right; color: #E05C65; padding-left: 10px; }
    .error input { background-color: #E3C3C5; }
    img { width: 200px; }
  `]
})
export class CreateEventComponent {
  isDirty: boolean = true;

  constructor(
    private router: Router,
    private eventService: EventService) { }

  saveEvent(formValues) {
    this.eventService.save(formValues).subscribe(event => {
      this.isDirty = false;
      this.router.navigate(['/events']);
    });
  }

  cancel() {
    this.router.navigate(['/events']);
  }
}