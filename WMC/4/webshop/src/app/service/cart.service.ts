import { Injectable, signal, computed } from '@angular/core';
import { CartItem } from '../model/cart-item.model';
import { Product } from '../model/product.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItems = signal<CartItem[]>([]);

  // Computed signals for cart information
  items = this.cartItems.asReadonly();
  itemCount = computed(() => this.cartItems().reduce((sum, item) => sum + item.quantity, 0));
  totalPrice = computed(() =>
    this.cartItems().reduce((sum, item) => sum + (item.product.price || 0) * item.quantity, 0)
  );

  addProduct(product: Product): void {
    const currentItems = this.cartItems();
    const existingItem = currentItems.find(item => item.product.id === product.id);

    if (existingItem) {
      // Update quantity if product already exists
      this.cartItems.update(items =>
        items.map(item =>
          item.product.id === product.id
            ? { ...item, quantity: item.quantity + 1 }
            : item
        )
      );
    } else {
      // Add new item
      this.cartItems.update(items => [...items, { product, quantity: 1 }]);
    }
  }

  removeProduct(productId: number | undefined): void {
    if (productId === undefined) return;

    this.cartItems.update(items =>
      items.filter(item => item.product.id !== productId)
    );
  }

  getItems(): CartItem[] {
    return this.cartItems();
  }

  clearCart(): void {
    this.cartItems.set([]);
  }
}

