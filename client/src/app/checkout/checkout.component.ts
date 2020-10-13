import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { AccountService } from '../account/account.service';
import { BasketService } from '../basket/basket.service';
import { IBasketTotals } from '../shared/models/basket';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  basketTotals$: Observable<IBasketTotals>;
  // 233-1 Be sure that you had imported and exported ReactiveFormsModule at sharedModule
  // and import the sharedModule inside checkout.module
  checkoutForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private basketService: BasketService,
    private accountService: AccountService
  ) { }

  ngOnInit(): void {
    this.createCheckoutForm();
    this.getAddressFormValues();
    //261-5
    this.getDeliveryMethodValue();
    this.basketTotals$ = this.basketService.basketTotal$;
  }

  // 242-2 populate address form with backend data
  getAddressFormValues(): void {
    this.accountService.getUserAddress().subscribe( address => {
      if (address) {
        this.checkoutForm.get('addressForm').patchValue(address);
      }
    }, error => {
      console.log(error);
    });
  }

  //261-4
  getDeliveryMethodValue() {
    const basket = this.basketService.getCurrentBasketValue();
    if (basket.deliveryMethodId !== null) {
      this.checkoutForm.get('deliveryForm').get('deliveryMethod').patchValue(basket.deliveryMethodId.toString());
    }
  }

  createCheckoutForm(): void {
    this.checkoutForm = this.fb.group({
      addressForm: this.fb.group({

      firstName: [null, Validators.required],
      lastName: [null, Validators.required],
      street: [null, Validators.required],
      city: [null, Validators.required],
      state: [null, Validators.required],
      zipcode: [null, Validators.required],
      }),
      deliveryForm: this.fb.group({
        deliveryMethod: [null, Validators.required],
      }),
      paymentForm: this.fb.group({
        nameOnCard: [null, Validators.required]
      })
    });
  }
}
