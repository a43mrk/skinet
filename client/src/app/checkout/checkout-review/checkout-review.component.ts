import { CdkStepper } from '@angular/cdk/stepper';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { Observable } from 'rxjs';
import { Component, Input, OnInit } from '@angular/core';
import { IBasket } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent implements OnInit {
  // 167-1 to hold stepper value that will comes from template.
  @Input() appStepper: CdkStepper;
  basket$: Observable<IBasket>;

  constructor(private basketService: BasketService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  createPaymentIntent() {
    return this.basketService.createPaymentItent().subscribe((response: any) => {
      // 173 - no toastr
      // this.toastr.success('Payment intent created');
      // 167-2 programmatically forward the step on the wizard.
      this.appStepper.next();
    }, error => {
      console.log(error);
      // this.toastr.error(error.message);
    });
  }

}
