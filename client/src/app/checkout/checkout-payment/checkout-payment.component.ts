import { NavigationExtras, Router } from '@angular/router';
import { IOrder, IOrderToCreate } from './../../shared/models/order';
import { ToastrService } from 'ngx-toastr';
import { CheckoutService } from './../checkout.service';
import { BasketService } from 'src/app/basket/basket.service';
import { AfterViewInit, Component, Input, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { IBasket } from 'src/app/shared/models/basket';
import { FormGroup } from '@angular/forms';

// import Stripe javascript version
declare var Stripe;

// 265-1 implement AfterViewInit
@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements AfterViewInit, OnDestroy {
  @Input() checkoutForm: FormGroup;
  // 265-2 to access template variables
  @ViewChild('cardNumber', { static: true }) cardNumberElement: ElementRef;
  @ViewChild('cardExpiry', { static: true }) cardExpiryElement: ElementRef;
  @ViewChild('cardCvc', { static: true }) cardCvcElement: ElementRef;
  stripe: any;
  cardNumber: any;
  cardExpiry: any;
  cardCvc: any;
  cardErrors: any;
  // 266-1 bind to this class
  cardHandler = this.onChange.bind(this);
  // 271-1 new property
  // used when started and finished.
  loading = false;

  // 274-1 flags to disable/enable submit button for payments
  cardNumberValid = false;
  cardExpiryValid = false;
  cardCvcValid = false;

  constructor(
    private basketService: BasketService,
    private checkoutService: CheckoutService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  // don't forget to dispose stripe elements w/ ngOnDestroy
  ngOnDestroy(): void {
    this.cardNumber.destroy();
    this.cardCvc.destroy();
    this.cardExpiry.destroy();
  }

  // 173-1 no destructure to get directly error prop.
  onChange(event) {
    // console.log(event);
    if (event.error) {
      this.cardErrors = event.error.message;
    } else {
      this.cardErrors = null;
    }

  // 173-3 set the flags accordingly to the changed stripe element.
    switch(event.elementType){
      case 'cardNumber':
        this.cardNumberValid = event.complete;
        break;
      case 'cardExpiry':
        this.cardExpiryValid = event.complete;
        break;
      case 'cardCvc':
        this.cardCvcValid = event.complete;
        break;
    }
  }

  ngAfterViewInit(): void {
    this.stripe = Stripe('pk_test_51HbigvCu3NZCawmgVfXJlCK2qZeL3adVgot2yDWcQVh8eH3sZOl4fKzJ3PVaBMWiBFPGaOQJLgLryAAfBnNsB55r00UV2ibYFd');
    const elements = this.stripe.elements();

    this.cardNumber = elements.create('cardNumber');
    this.cardNumber.mount(this.cardNumberElement.nativeElement);
    this.cardNumber.addEventListener('change', this.cardHandler);

    this.cardExpiry = elements.create('cardExpiry');
    this.cardExpiry.mount(this.cardExpiryElement.nativeElement);
    this.cardExpiry.addEventListener('change', this.cardHandler);

    this.cardCvc = elements.create('cardCvc');
    this.cardCvc.mount(this.cardCvcElement.nativeElement);
    this.cardCvc.addEventListener('change', this.cardHandler);
  }

  // 271-2 use async to use await other than then w/ promisees.
  async submitOrder() {
    // 271-3
    this.loading = true;
    const basket = this.basketService.getCurrentBasketValue();
    try {
      // 271-4
      const createdOrder = await this.createOrder(basket);

      // 271-5
      const paymentResult = await this.confirmPaymentWithStripe(basket);

      if (paymentResult.paymentIntent){
        // 276-4 remove the actual basket other than the local one
        this.basketService.deleteBasket(basket);
        const navigationExtras: NavigationExtras = { state: createdOrder };
        this.router.navigate(['checkout/success'], navigationExtras);
      } else {
        this.toastr.error(paymentResult.error.message);
      }
      // this.loading = false;
    } catch (error){
      console.log(error);
      // this.loading = false;
    } finally {
      this.loading = false;
    }
    // this.checkoutService.createOrder(orderToCreate).subscribe((order: IOrder) => {
    //   // 271. no toastr
    //   // this.toastr.success('Order created Successfully');

    //   // 268-1
    //   // beware of wrong method names, look for documentation too
    //   this.stripe.confirmCardPayment(basket.clientSecret, {
    //     payment_method: {
    //       card: this.cardNumber,
    //       billing_details: {
    //         // beware of misspelling! uppercase lowercase
    //         name: this.checkoutForm.get('paymentForm').get('nameOnCard').value
    //       }
    //     }
    //   }).then(result => {
    //     console.log(result);
    //     if (result.paymentIntent){
    //       this.basketService.deleteLocalBasket(basket.id);
    //       const navigationExtras: NavigationExtras = { state: order };
    //       this.router.navigate(['checkout/success'], navigationExtras);
    //     } else {
    //       this.toastr.error(result.error.message);
    //     }
    //   });
    // }, error => {
    //   this.toastr.error(error.message);
    //   console.log(error);
    // });
  }
  private async confirmPaymentWithStripe(basket) {
      return this.stripe.confirmCardPayment(basket.clientSecret, {
        payment_method: {
          card: this.cardNumber,
          billing_details: {
            // beware of misspelling! uppercase lowercase
            name: this.checkoutForm.get('paymentForm').get('nameOnCard').value
          }
        }
      });
  }
  private async createOrder(basket: IBasket) {
    const orderToCreate = this.getOrderToCreate(basket);
    return this.checkoutService.createOrder(orderToCreate).toPromise();
  }

  getOrderToCreate(basket: IBasket) {
    return {
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      shipToAddress: this.checkoutForm.get('addressForm').value,
    };
  }

}
