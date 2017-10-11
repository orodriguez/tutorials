import { FormGroup, NG_VALIDATORS, Validator } from '@angular/forms';
import { Directive } from '@angular/core';

@Directive({
  selector: '[validateLocation]',
  providers: [{
    provide: NG_VALIDATORS, 
    useExisting: LocationValidator, 
    multi: true
  }]
})
export class LocationValidator implements Validator {
  
  validate(formGroup: FormGroup): { [key: string] : any } {
    let addressControl = formGroup.controls['address'];
    let cityControl = formGroup.controls['city'];
    let conuntryControl = formGroup.controls['country'];
    let onlineUrlControl = (<FormGroup> formGroup.root)
      .controls['onlineUrl'];

    if ((addressControl && addressControl.value 
        && cityControl && cityControl.value 
        && conuntryControl && conuntryControl.value) 
        || (onlineUrlControl && onlineUrlControl.value)) {
      return null;
    } else {
      return { validateLocation: false };
    }
  }
}