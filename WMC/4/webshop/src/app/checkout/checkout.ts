import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CartService } from '../service/cart.service';
import { OrderService } from '../service/order.service';
import { Order } from '../model/order.model';
import { ProductDto } from '../model/product-dto.model';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css'
})
export class Checkout {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  protected cartService = inject(CartService);
  private orderService = inject(OrderService);

  checkoutForm: FormGroup;
  submitted = false;

  constructor() {
    this.checkoutForm = this.fb.group({
      salutation: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      street: ['', Validators.required],
      zipCode: ['', [Validators.required, Validators.pattern(/^\d+$/)]],
      city: ['', Validators.required]
    });
  }

  get f() {
    return this.checkoutForm.controls;
  }

  onSubmit(): void {
    this.submitted = true;
    console.log('Checkout: Form submitted', this.checkoutForm.value);

    if (this.checkoutForm.invalid) {
      console.log('Checkout: Form is invalid');
      return;
    }

    if (this.cartService.items().length === 0) {
      console.log('Checkout: Cart is empty');
      alert('Ihr Warenkorb ist leer!');
      return;
    }

    const formValue = this.checkoutForm.value;

    const products: ProductDto[] = this.cartService.items().map(item => ({
      id: item.product.id,
      name: item.product.name,
      price: item.product.price,
      imageName: item.product.imageName,
      validFrom: item.product.validfrom,
      validTo: item.product.validto,
      quantity: item.quantity
    }));

    const order: Order = {
      salutation: formValue.salutation,
      firstName: formValue.firstName,
      lastName: formValue.lastName,
      street: formValue.street,
      city: formValue.city,
      zipCode: formValue.zipCode,
      totalPrice: this.cartService.totalPrice(),
      productList: products
    };

    console.log('Checkout: Sending order', order);

    this.orderService.createOrder(order).subscribe({
      next: (response) => {
        console.log('Checkout: Order created successfully', response);

        const orderItems = [...this.cartService.items()];
        const total = this.cartService.totalPrice();

        this.cartService.clearCart();

        this.router.navigate(['/confirmation'], {
          state: {
            orderComplete: true,
            items: orderItems,
            total: total
          }
        });
      },
      error: (error) => {
        console.error('Checkout: Error creating order', error);
        alert('Fehler beim Speichern der Bestellung: ' + error.message);
      }
    });
  }

  onCancel(): void {
    console.log('Checkout: Cancelled');
    this.router.navigate(['/shop']);
  }
}
