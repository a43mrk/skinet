import { SharedModule } from './../shared/shared.module';
import { OrdersRoutingModule } from './orders-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersComponent } from './orders.component';
import { OrderDetailedComponent } from './order-detailed/order-detailed.component';



@NgModule({
  declarations: [OrdersComponent, OrderDetailedComponent],
  imports: [
    CommonModule,
    // don't forget to add routing module to make available for the template
    OrdersRoutingModule,
    // to use app-basket-summary we need to add SharedModule
    SharedModule
  ]
})
export class OrdersModule { }
