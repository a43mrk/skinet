import { SharedModule } from './../shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BasketComponent } from './basket.component';
import { BasketRoutingModule } from './basket-routing.module';



@NgModule({
  declarations: [BasketComponent],
  imports: [
    CommonModule,
    // 145-5 import basket routing module
    // *ERROR in Maximum call stack size exceeded* is displayed if you import a wrong Module!
    BasketRoutingModule,
    SharedModule
  ]
})
export class BasketModule { }
