import { Component, ElementRef, Input, OnInit, Self, ViewChild } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

// 197-2 TextInputComponent implement ControlValueAccessor
@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements OnInit, ControlValueAccessor {
  @ViewChild('input', { static: true }) input: ElementRef;
  @Input() type = 'text';
  @Input() label: string;

  // 197-3 inject with @Self() the NgControl to access the validation
  // public to be accessed at template.
  constructor(@Self() public controlDir: NgControl) {
    this.controlDir.valueAccessor = this;
   }

  ngOnInit(): void {
    const control = this.controlDir.control;
    const validators = control.validator ? [control.asyncValidator] :[];
    const asyncValidators = control.asyncValidator ? [control.asyncValidator] :[];

    control.setValidators(validators);
    control.setAsyncValidators(asyncValidators);
    control.updateValueAndValidity();
  }

  // let this empty
  onChange(event) {}

  // let this empty
  onTouched() {}

  writeValue(obj: any): void {
    this.input.nativeElement.value = obj || '';
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  // won't be implemented for now on.
  // setDisabledState?(isDisabled: boolean): void {
  //   throw new Error('Method not implemented.');
  // }


}
