import { JQ_TOKEN } from './jQuery.service';
import { Component, ElementRef, Inject, Input, ViewChild } from '@angular/core';
import { selector } from 'rxjs/operator/multicast';

@Component({
  selector: 'simple-modal',
  templateUrl: 'app/common/simple-modal.component.html',
  styles: [`
    .modal-body { height: 250px; overflow-y: scroll; }
  `]
})
export class SimpleModalComponent {
  @Input() title: string;
  @Input() elementId: string;
  @Input() closeOnBodyClick: string;
  @ViewChild('modalContainer') containerEL: ElementRef;

  constructor(@Inject(JQ_TOKEN) private $: any) {}

  closeModal() {
    if(this.closeOnBodyClick.toLocaleLowerCase() === 'true') {
      this.$(this.containerEL.nativeElement).modal('hide');
    }
  }
}