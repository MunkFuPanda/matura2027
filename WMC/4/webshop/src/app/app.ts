import { Component, signal, inject } from '@angular/core';
import { RouterOutlet, RouterLinkWithHref } from '@angular/router';
import { CartService } from './service/cart.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLinkWithHref],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('webshop');
  protected cartService = inject(CartService);
}
