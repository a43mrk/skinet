import { ShopService } from './../shop.service';
import { Component, OnInit } from '@angular/core';
import { IProduct } from 'src/app/shared/models/products';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;

  constructor(private shopService: ShopService, private activateRoute: ActivatedRoute, private bcService: BreadcrumbService) { }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(){
    this.shopService.getProduct(
      +this.activateRoute.snapshot.paramMap.get('id')
    ).subscribe(
      response =>{
        this.product = response;
        this.bcService.set('@productDetails', response.name);
      },
      error => console.log(error));
  }

}
