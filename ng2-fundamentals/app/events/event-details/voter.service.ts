import { Observable } from 'rxjs/Observable';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { ISession } from '../';
import { Injectable } from '@angular/core';

@Injectable()
export class VoterService {

  constructor(private http: Http) { }

  deleteVoter(eventId: number, session: ISession, voterName: string) {
    session.voters = session.voters
      .filter(voter => voter !== voterName);

    const url = `/api/events/${eventId}/sessions/${session.id}/voters/${voterName}`;

    this.http.delete(url).catch(this.handleError).subscribe();
  }

  addVoter(eventId: number, session: ISession, voterName: string) {
    session.voters.push(voterName);
    const url = `/api/events/${eventId}/sessions/${session.id}/voters/${voterName}`;

    this.http.post(url, {}, new RequestOptions({
      headers: new Headers({
        'Content-Type': 'application/json'
      })
    })).catch(this.handleError).subscribe();
  }

  userHasVoted(session: ISession, voterName: string) {
    return session.voters
      .some(voter => voter === voterName);
  }

  private handleError(error: Response) {
    return Observable.throw(error.statusText);
  }
}