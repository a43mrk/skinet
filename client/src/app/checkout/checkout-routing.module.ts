import { RouterModule } from '@angular/router';
import { CheckoutComponent } from './checkout.component';
import { NgModule } from '@angular/core';

// 158-0
const routes = [
  {path: '', component: CheckoutComponent }
];


@NgModule({
  declarations: [],
  imports: [
    // 158-1
    RouterModule.forChild(routes)
  ],
  // 158-1
  exports: [ RouterModule ]
})
export class CheckoutRoutingModule { }
