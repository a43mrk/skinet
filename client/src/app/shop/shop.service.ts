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

  constructor(private http: HttpClient) { }

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();
    if(shopParams.brandId !== 0){
      params = params.append('brandId', shopParams.brandId.toString());
    }

    if(shopParams.typeId !== 0){
      params = params.append('typeId', shopParams.typeId.toString());
    }

    if(shopParams.search){
      params = params.append('search', shopParams.search);
    }

    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'products', {observe: 'response', params })
      .pipe(
        // delay(1000),
        map(response => {
          // 183-2 save to cache
          this.products = response.body.data;
          return response.body;
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
