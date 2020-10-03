import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BasketComponent } from './basket.component';
import { RouterModule, Routes } from '@angular/router';

// 145-2 add new route to component
const routes: Routes = [
  { path: '', component: BasketComponent }
];

@NgModule({
  declarations: [],
  imports: [
    // 145-3 add new route module
    RouterModule.forChild(routes)
  ],
  // 145-4 export route module
  exports: [ RouterModule ]
})
export class BasketRoutingModule { }
