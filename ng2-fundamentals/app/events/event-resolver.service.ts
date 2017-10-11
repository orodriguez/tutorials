import { EventService } from './shared/event.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';

@Injectable()
export class EventResolver implements Resolve<any> {
  constructor(private eventService: EventService) {}

  public resolve(route: ActivatedRouteSnapshot) { 
    return this.eventService.getEvent(route.params['id']);
  }
}