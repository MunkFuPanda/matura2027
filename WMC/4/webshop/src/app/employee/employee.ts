import { Component, OnInit, inject } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { Router } from '@angular/router';
import { OrderService } from '../service/order.service';
import { AuthenticationService } from '../service/authentication.service';
import { Order } from '../model/order.model';

@Component({
  selector: 'app-employee',
  standalone: true,
  imports: [CommonModule, CurrencyPipe],
  templateUrl: './employee.html',
  styleUrl: './employee.css'
})
export class Employee implements OnInit {
  private orderService = inject(OrderService);
  private authService = inject(AuthenticationService);
  private router = inject(Router);

  orders: Order[] = [];
  loading = true;

  ngOnInit(): void {
    console.log('Employee: Loading active orders');
    this.loadActiveOrders();
  }

  loadActiveOrders(): void {
    this.loading = true;
    this.orderService.getActiveOrders().subscribe({
      next: (response) => {
        console.log('Employee: Active orders loaded', response);
        this.orders = response || [];
        this.loading = false;
      },
      error: (error) => {
        console.error('Employee: Error loading orders', error);
        this.loading = false;
        alert('Fehler beim Laden der Bestellungen.');
      }
    });
  }

  finishOrder(orderId: number | undefined): void {
    if (!orderId) return;

    console.log('Employee: Finishing order', orderId);

    if (!confirm('Möchten Sie diese Bestellung als erledigt markieren?')) {
      return;
    }

    this.orderService.finishOrder(orderId).subscribe({
      next: (response) => {
        console.log('Employee: Order finished', response);
        this.loadActiveOrders();
      },
      error: (error) => {
        console.error('Employee: Error finishing order', error);
        alert('Fehler beim Abschließen der Bestellung.');
      }
    });
  }

  cancelOrder(orderId: number | undefined): void {
    if (!orderId) return;

    console.log('Employee: Cancelling order', orderId);

    if (!confirm('Möchten Sie diese Bestellung stornieren?')) {
      return;
    }

    this.orderService.cancelOrder(orderId).subscribe({
      next: (response) => {
        console.log('Employee: Order cancelled', response);
        this.loadActiveOrders();
      },
      error: (error) => {
        console.error('Employee: Error cancelling order', error);
        alert('Fehler beim Stornieren der Bestellung.');
      }
    });
  }

  logout(): void {
    console.log('Employee: Logging out');
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
