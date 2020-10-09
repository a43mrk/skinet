import { SharedModule } from './../shared/shared.module';
import { CheckoutRoutingModule } from './checkout-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckoutComponent } from './checkout.component';


@NgModule({
  declarations: [CheckoutComponent],
  imports: [
    CommonModule,
    // 158-2
    CheckoutRoutingModule,
    // 229- make Order Summary available for template at Checkout Module
    SharedModule
  ]
})
export class CheckoutModule { }
