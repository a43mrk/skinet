import { Pagination } from './../shared/models/pagination';
import { ShopParams } from './../shared/models/shopParams';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPagination } from '../shared/models/pagination';
import { IBrand } from '../shared/models/brands';
import { IType } from '../shared/models/productType';
import { map, delay } from 'rxjs/operators';
import { IProduct } from '../shared/models/products';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  // 283-1 caching
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];

  // 284-2 caching for paginated data
  pagination = new Pagination();
  shopParams = new ShopParams();

  constructor(private http: HttpClient) { }

  setShopParams(params: ShopParams){
    this.shopParams = params;
  }

  getShopParams = () => this.shopParams;

  getProducts(useCache: boolean) {
    // 285 -1
    if (useCache === false) {
      this.products = [];
    }

    if (this.products.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.products.length / this.shopParams.pageSize);

      if (this.shopParams.pageNumber <= pagesReceived) {
        this.pagination.data = this.products.slice((this.shopParams.pageNumber -1) * this.shopParams.pageSize,
          this.shopParams.pageNumber * this.shopParams.pageSize);

        return of(this.pagination);
      }
    }

    let params = new HttpParams();
    if(this.shopParams.brandId !== 0){
      params = params.append('brandId', this.shopParams.brandId.toString());
    }

    if(this.shopParams.typeId !== 0){
      params = params.append('typeId', this.shopParams.typeId.toString());
    }

    if(this.shopParams.search){
      params = params.append('search', this.shopParams.search);
    }

    params = params.append('sort', this.shopParams.sort);
    params = params.append('pageIndex', this.shopParams.pageNumber.toString());
    params = params.append('pageSize', this.shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'products', {observe: 'response', params })
      .pipe(
        // delay(1000),
        map(response => {
          // 283-2 save to cache
          // 284- concatenate data
          this.products = [ ...this.products, ...response.body.data ];
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }

  getBrands(){
    // 183-5 caching for brands
    if (this.brands.length > 0){
      return of(this.brands);
    }

    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands').pipe(
      map(response => {
        // 183-7 populate brands cache when retrieving from backend.
        this.brands = response;
        return response;
      })
    );
  }

  getTypes() {
    // 183-4 caching for types
    if (this.types.length > 0) {
      return of(this.types);
    }

    return this.http.get<IType[]>(this.baseUrl + 'products/types').pipe(
      map(response => {
        // 183-6 populate the cache when retrieving from backend
        this.types = response;
        return response;
      })
    );
  }

  getProduct(id: number){
    // 183-3 caching for getProduct
    const product = this.products.find(p => p.id === id);
    if (product) {
      // return observable of product
      return of(product);
    }
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }
}
