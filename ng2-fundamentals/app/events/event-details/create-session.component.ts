import { ISession, restrictedWords } from '../index';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'create-session',
  templateUrl: 'app/events/event-details/create-session.component.html',
  styles: [`
    em { float: right; color: #E05C65; padding-left: 10px; }
    .error input, .error select, .error textarea { background-color: #E3C3C5; }
    img { width: 200px; }
  `]
})
export class CreateSessionComponent implements OnInit {
  @Output() saveNewSession = new EventEmitter<ISession>();
  @Output() cancelAddSession = new EventEmitter();

  newSessionForm: FormGroup;
  abstract: FormControl;
  level: FormControl;
  duration: FormControl;
  presenter: FormControl;
  name: FormControl;

  constructor() { }

  ngOnInit() {
    this.name = new FormControl('', Validators.required);
    this.presenter = new FormControl('', Validators.required);
    this.duration = new FormControl('', Validators.required);
    this.level = new FormControl('', Validators.required);
    this.abstract = new FormControl('', [
      Validators.required,
      Validators.maxLength(400),
      restrictedWords(['foo', 'bar'])]);

    this.newSessionForm = new FormGroup({
      name: this.name,
      presenter: this.presenter,
      duration: this.duration,
      level: this.level,
      abstract: this.abstract
    });
  }

  save(form) {
    const session: ISession = {
      id: undefined,
      name: form.name,
      duration: +form.duration,
      level: form.level,
      presenter: form.presenter,
      abstract: form.abstract,
      voters: []
    };

    this.saveNewSession.emit(session);
  }

  cancel() {
    this.cancelAddSession.emit();
  }
}