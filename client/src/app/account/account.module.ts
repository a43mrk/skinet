import { SharedModule } from './../shared/shared.module';
import { AccountRoutingModule } from './account-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';



@NgModule({
  declarations: [LoginComponent, RegisterComponent],
  imports: [
    CommonModule,
    // 186-2 import account router
    AccountRoutingModule,
    // 190-2
    SharedModule
  ]
})
export class AccountModule { }
