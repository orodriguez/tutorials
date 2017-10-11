import { VoterService } from './voter.service';
import { AuthService } from '../../user/auth.service';
import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { ISession,  } from "../index";

@Component({
  selector: 'session-list',
  templateUrl: 'app/events/event-details/session-list.component.html',
})
export class SessionListComponent implements OnChanges {
  @Input() sessions: ISession[]
  @Input() eventId: number;
  @Input() filterBy: string;
  @Input() sortBy: string;
  visibleSessions: ISession[] = [];

  constructor(private auth: AuthService, private voterService: VoterService) {}

  ngOnChanges() {
    if (this.sessions) {
      this.filterSessions(this.filterBy);
      this.sortBy === 'name'
        ? this.visibleSessions.sort(sortByNameAsc)
        : this.visibleSessions.sort(sortByVotesDesc);
    }
  }

  toggleVote(session: ISession) {
    if (this.userHasVoted(session)) {
      this.voterService.deleteVoter(this.eventId,
        session,
        this.auth.currentUser.userName);
    } else {
      this.voterService.addVoter(this.eventId,
        session,
        this.auth.currentUser.userName)
    }

    if (this.sortBy === 'votes')
      this.visibleSessions.sort(sortByVotesDesc)
  }

  userHasVoted(session: ISession) {
    return this.voterService.userHasVoted(session,
      this.auth.currentUser.userName);
  }

  filterSessions(filter) {
    if (filter === 'all') {
      // clones the array
      this.visibleSessions = this.sessions.slice(0);
    } else {
      this.visibleSessions = this.sessions.filter(s => {
        return s.level.toLocaleLowerCase() === filter;
      });
    }
  }
}

function sortByNameAsc(s1: ISession, s2: ISession) {
  if (s1.name > s2.name) return 1;
  if (s1.name === s2.name) return 0;
  return -1;
}

function sortByVotesDesc(s1: ISession, s2: ISession) {
  return s2.voters.length - s1.voters.length;
}