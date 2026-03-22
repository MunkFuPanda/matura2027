import { Component, inject, ChangeDetectionStrategy, Input, computed, signal } from '@angular/core';
import { CartService } from '../service/cart.service';
import { CartItem } from '../model/cart-item.model';
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
  @Input() showButtons: boolean = true;
  @Input() set cartItems(items: CartItem[] | null) {
    if (items) {
      this._cartItems.set(items);
    }
  }

  protected cartService = inject(CartService);
  private _cartItems = signal<CartItem[] | null>(null);

  protected displayItems = computed(() => {
    const customItems = this._cartItems();
    return customItems !== null ? customItems : this.cartService.items();
  });

  protected displayItemCount = computed(() => {
    const items = this.displayItems();
    return items.reduce((sum, item) => sum + item.quantity, 0);
  });

  protected displayTotalPrice = computed(() => {
    const items = this.displayItems();
    return items.reduce((sum, item) => sum + (item.product.price || 0) * item.quantity, 0);
  });

  removeFromCart(productId: number | undefined): void {
    this.cartService.removeProduct(productId);
  }

  getTotalForItem(price: number | undefined, quantity: number): number {
    return (price || 0) * quantity;
  }
}
