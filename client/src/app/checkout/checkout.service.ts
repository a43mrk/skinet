import { IOrderToCreate } from './../shared/models/order';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // 235-1 get delivery methods from backend
  getDeliveryMethods() {
    return this.http.get(this.baseUrl + 'orders/deliveryMethods').pipe(
      map((dm: IDeliveryMethod[]) => {
        return dm.sort((a, b) => b.price - a.price);
      })
    );
  }

  // 246-1
  createOrder(order: IOrderToCreate) {
    return this.http.post(this.baseUrl + 'orders', order);
  }
}
