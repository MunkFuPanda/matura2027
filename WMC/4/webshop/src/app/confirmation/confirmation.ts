import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CartItem } from '../model/cart-item.model';
import { Cart } from '../cart/cart';

@Component({
  selector: 'app-confirmation',
  standalone: true,
  imports: [Cart, RouterLink],
  templateUrl: './confirmation.html',
  styleUrl: './confirmation.css'
})
export class Confirmation {
  private router = inject(Router);

  protected orderItems = signal<CartItem[]>([]);
  protected orderComplete = signal(false);

  constructor() {
    console.log('Confirmation: Component initialized');
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras?.state;

    if (state && state['orderComplete']) {
      this.orderComplete.set(true);
      const items = state['items'] || [];
      this.orderItems.set(items);
      console.log('Confirmation: Order complete', items);
    } else {
      console.log('Confirmation: No order data found, redirecting to shop');
      this.router.navigate(['/shop']);
    }
  }
}
