import {Component, inject, ChangeDetectionStrategy, OnInit} from '@angular/core';
import {ProductService} from "../service/product.service";
import {Product} from '../model/product.model';
import {CartService} from '../service/cart.service';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.html',
  styleUrl: './shop.css',
})

export class Shop implements OnInit {
  private productService = inject(ProductService);
  private cartService = inject(CartService);
  private route = inject(ActivatedRoute);

  protected products: Product[] = [];
  protected searchTerm: string = '';

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.searchTerm = params['search'] || '';
      console.log('Shop: Search term from query params', this.searchTerm);
      
      if (this.searchTerm) {
        this.searchProducts(this.searchTerm);
      } else {
        this.loadProducts();
      }
    });
  }

  private loadProducts(): void {
    console.log('Shop: Loading all products');
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

  private searchProducts(term: string): void {
    console.log('Shop: Searching products with term', term);
    this.productService.searchProducts(term).subscribe({
      next: (data: Product[]) => {
        this.products = data;
        console.log('Search results:', this.products);
      },
      error: (error: any) => {
        console.error('Error searching products:', error);
      }
    });
  }

  addToCart(product: Product): void {
    this.cartService.addProduct(product);
    console.log(`Product added to cart: ${product.name}`);
  }
}
