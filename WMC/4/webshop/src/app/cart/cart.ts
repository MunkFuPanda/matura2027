import { Component, inject, ChangeDetectionStrategy } from '@angular/core';
import { CartService } from '../service/cart.service';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-cart',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './cart.html',
  styleUrl: './cart.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Cart {
  protected cartService = inject(CartService);

  removeFromCart(productId: number | undefined): void {
    this.cartService.removeProduct(productId);
  }

  getTotalForItem(price: number | undefined, quantity: number): number {
    return (price || 0) * quantity;
  }
}
