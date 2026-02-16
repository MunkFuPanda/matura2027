import { Injectable, signal, computed } from '@angular/core';
import { CartItem } from '../model/cart-item.model';
import { Product } from '../model/product.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItems = signal<CartItem[]>(this.loadCartFromStorage());

  // Computed signals for cart information
  items = this.cartItems.asReadonly();
  itemCount = computed(() => this.cartItems().reduce((sum, item) => sum + item.quantity, 0));
  totalPrice = computed(() =>
    this.cartItems().reduce((sum, item) => sum + (item.product.price || 0) * item.quantity, 0)
  );

  private loadCartFromStorage(): CartItem[] {
    try {
      const stored = localStorage.getItem('cart');
      return stored ? JSON.parse(stored) : [];
    } catch {
      return [];
    }
  }

  private saveCartToStorage(): void {
    localStorage.setItem('cart', JSON.stringify(this.cartItems()));
  }

  addProduct(product: Product, quantity: number = 1, silent: boolean = false): void {
    if (!silent) {
      console.log('CartService: Adding product', product, 'Quantity:', quantity);
    }

    const currentItems = this.cartItems();
    const existingItem = currentItems.find(item => item.product.id === product.id);

    if (existingItem) {
      // Update quantity if product already exists
      this.cartItems.update(items =>
        items.map(item =>
          item.product.id === product.id
            ? { ...item, quantity: item.quantity + quantity }
            : item
        )
      );
    } else {
      // Add new item
      this.cartItems.update(items => [...items, { product, quantity }]);
    }

    this.saveCartToStorage();
  }

  removeProduct(productId: number | undefined): void {
    if (productId === undefined) return;

    this.cartItems.update(items =>
      items.filter(item => item.product.id !== productId)
    );

    this.saveCartToStorage();
  }

  getItems(): CartItem[] {
    return this.cartItems();
  }

  clearCart(): void {
    this.cartItems.set([]);
    this.saveCartToStorage();
  }
}

