import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order } from '../model/order.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseUrl = `${environment.apiUrl}/order`;

  constructor(private http: HttpClient) { }

  createOrder(order: Order): Observable<any> {
    console.log('OrderService: Creating order', order);
    return this.http.post<any>(`${this.baseUrl}/create`, order);
  }

  getAllOrders(): Observable<any> {
    console.log('OrderService: Fetching all orders');
    return this.http.get<any>(this.baseUrl);
  }

  getActiveOrders(): Observable<any> {
    console.log('OrderService: Fetching active orders');
    return this.http.get<any>(`${this.baseUrl}?cancelled=false&finished=false`);
  }

  finishOrder(id: number): Observable<any> {
    console.log('OrderService: Finishing order', id);
    return this.http.put<any>(`${this.baseUrl}/finished`, id);
  }

  cancelOrder(id: number): Observable<any> {
    console.log('OrderService: Cancelling order', id);
    return this.http.put<any>(`${this.baseUrl}/cancel`, id);
  }
}
