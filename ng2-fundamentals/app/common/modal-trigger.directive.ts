import { JQ_TOKEN } from './jQuery.service';
import { Directive, ElementRef, Inject, Input, OnInit } from '@angular/core';

@Directive({
  selector: '[modal-trigger]'
})
export class ModalTriggerDirective implements OnInit {
  @Input('modal-trigger') modalId: string;

  constructor(
    private el: ElementRef, 
    @Inject(JQ_TOKEN) private $: any) { }

  public ngOnInit(): void {
    this.el.nativeElement.addEventListener('click', e => {
      this.$(`#${this.modalId}`).modal({});
    });
  }
}