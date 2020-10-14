import { IType } from './../shared/models/productType';
import { IBrand } from './../shared/models/brands';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IProduct } from '../shared/models/products';
import { ShopService } from './shop.service';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  // brandIdSelected = 0;
  // typeIdSelected = 0;
  // sortSelected = 'name';
  // 284-3 shopParams = new ShopParams();
  shopParams: ShopParams;
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to High', value: 'priceAsc'},
    {name: 'Price: High to Low', value: 'priceDesc'}
  ];
  totalCount: number;

  constructor(private shopService: ShopService) {
    this.shopParams = this.shopService.getShopParams();
   }

  ngOnInit(): void {
    // 285 enable caching
    this.getProducts(true);
    this.getBrands();
    this.getTypes();
  }

  // 285 parameter to use caching
  getProducts(useCache = false) {
    this.shopService.getProducts(useCache).subscribe(
      response => {
        this.products = response.data;
        // this.shopParams.pageNumber = response.pageIndex;
        // this.shopParams.pageSize = response.pageSize;
        this.totalCount = response.count;
    },
      error => console.log(error)
    );
  }

  getBrands(){
    this.shopService.getBrands().subscribe(
      response => {
        this.brands = [{id: 0, name: 'All'}, ...response ];
      },
      error => {
        console.log(error);
      }
    );
  }

  getTypes(){
    this.shopService.getTypes().subscribe(
      response => {
        this.types = [{id: 0, name: 'All'}, ...response ];
      },
      error => {
        console.log(error);
      }
    );
  }

  onBrandSelected(brandId: number){
    // 284-5 set filter at service
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onTypeSelected(typeId: number){
    // 284-6 set filter at service
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onSortSelected(sort: string){
    // 284-7 set filter at service
    const params = this.shopService.getShopParams();
    params.sort = sort;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onPageChanged(event: any){
    // 284-8 set filter at service
    const params = this.shopService.getShopParams();

    if(params.pageNumber !== event){
      params.pageNumber = event.page;
      this.shopService.setShopParams(params);
      // 285 enable caching
      this.getProducts(true);
    }
  }

  onSearch(){
    // 284-9 set filter at service
    const params = this.shopService.getShopParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onReset(){
    // 284-10 set new filter when reset
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.shopService.setShopParams(this.shopParams);
    this.getProducts();
  }
}
