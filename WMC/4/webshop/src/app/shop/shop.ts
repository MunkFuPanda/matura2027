import {Component, inject, ChangeDetectionStrategy} from '@angular/core';
import {ProductService} from "../service/product.service";
import {Product} from '../model/product.model';
import {CartService} from '../service/cart.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.html',
  styleUrl: './shop.css',
})

export class Shop {
  private productService = inject(ProductService);
  private cartService = inject(CartService);

  protected products: Product[] = [];

  constructor() {
    this.loadProducts();
  }

  private loadProducts(): void {
    this.productService.getProducts().subscribe({
      next: (data: Product[]) => {
        this.products = data;
        console.log('Products loaded:', this.products);
      },
      error: (error: any) => {
        console.error('Error fetching products:', error);
      }
    });
  }

  addToCart(product: Product): void {
    this.cartService.addProduct(product);
    console.log(`Product added to cart: ${product.name}`);
  }
}
