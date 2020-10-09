import { PaginationModule } from 'ngx-bootstrap/pagination';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TextInputComponent } from './components/text-input/text-input.component';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { StepperComponent } from './components/stepper/stepper.component';

@NgModule({
  declarations: [PagingHeaderComponent, PagerComponent, OrderTotalsComponent, TextInputComponent, StepperComponent],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    CarouselModule.forRoot(),
    // 190-1
    ReactiveFormsModule,
    // 194-1 adding dropdown
    BsDropdownModule.forRoot(),
    // 230-1 ng add @angular/cdk
    // 230-2 add Wizard capable Module from Material
    CdkStepperModule
  ],
  exports: [
    PaginationModule,
    PagingHeaderComponent,
    PagerComponent,
    CarouselModule,
    OrderTotalsComponent,
    // 190-2
    ReactiveFormsModule,
    // 194-2
    BsDropdownModule,
    // 197-1 export new TextInputComponent to be available to all who imports shared modules
    TextInputComponent,
    // 230-3 export cdk to be used elsewhere
    CdkStepperModule,
    // 230-4 export stepper component
    StepperComponent
  ]
})
export class SharedModule { }
