import { Injectable } from '@angular/core';

import {HttpClient, provideHttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Product} from "../model/product.model";
/*
import {environment} from "../../environments/environment";
*/

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http: HttpClient) { }
/*
  baseUrl = environment.apiUrl+'/users';
*/
  baseUrl = 'http://localhost:8081/product';

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.baseUrl);
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(this.baseUrl + '/'+id);
  }

  createProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.baseUrl, product);
  }

  updateProduct(product: Product): Observable<Product> {
    return this.http.put<Product>(this.baseUrl +'/'+ product.id, product);
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl +'/'+ id);
  }
}
