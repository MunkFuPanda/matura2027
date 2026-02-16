import { Component, signal, inject } from '@angular/core';
import { RouterOutlet, RouterLinkWithHref, Router } from '@angular/router';
import { CartService } from './service/cart.service';
import { AuthenticationService } from './service/authentication.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLinkWithHref],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('webshop');
  protected cartService = inject(CartService);
  protected authService = inject(AuthenticationService);
  private router = inject(Router);
  protected searchTerm = signal('');

  constructor() {
    this.authService.currentUser.subscribe(user => {
      console.log('App: Current user changed', user);
      if (user === null && this.router.url.includes('/employee')) {
        this.router.navigate(['/login']);
      }
    });
  }

  onSearch(event: Event): void {
    event.preventDefault();
    const term = this.searchTerm();
    console.log('App: Searching for', term);
    
    if (term) {
      this.router.navigate(['/shop'], { queryParams: { search: term } });
    } else {
      this.router.navigate(['/shop']);
    }
  }

  onLoginLogout(): void {
    if (this.authService.isLoggedIn()) {
      console.log('App: Logging out');
      this.authService.logout();
      this.router.navigate(['/']);
    } else {
      this.router.navigate(['/login']);
    }
  }
}
