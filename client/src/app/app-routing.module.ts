import { AuthGuard } from './guards/auth.guard';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ShopComponent } from './shop/shop.component';
import { ProductDetailsComponent } from './shop/product-details/product-details.component';

const routes: Routes = [
  { path: '', component: HomeComponent, data: {breadcrumb: 'Home'} },
  { path: 'test-error', component: TestErrorComponent, data: {breadcrumb: 'Test Errors'} },
  { path: 'server-error', component: ServerErrorComponent, data: {breadcrumb: 'Server Error'}},
  { path: 'not-found', component: NotFoundComponent, data: {breadcrumb: 'Not Found'} },
  { path: 'shop', loadChildren: () => import('./shop/shop.module').then(mod => mod.ShopModule ), data: { breadcrumb: 'Shop'} },
  // 145-1 add new route for basket
  { path: 'basket', loadChildren: () => import('./basket/basket.module').then( mod => mod.BasketModule), data: { breadcrumb: 'Basket'}},
  // 158-3
  // 203-2 add the guard as parameter
  {
    path: 'checkout',
    canActivate: [AuthGuard],
    loadChildren: () => import('./checkout/checkout.module').then( mod => mod.CheckoutModule), data: { breadcrumb: 'Checkout'}},
  // 186-3 add account route to main route
  { path: 'account', loadChildren: () => import('./account/account.module')
    .then( mod => mod.AccountModule ), data: { breadcrumb: { skip: true }}},
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
