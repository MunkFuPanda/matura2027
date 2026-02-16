import { Routes } from '@angular/router';
import { Lander } from './lander/lander';
import { Shop } from './shop/shop';
import { Cart } from './cart/cart';
import { Checkout } from './checkout/checkout';
import { Login } from './login/login';
import { Employee } from './employee/employee';
import { Confirmation } from './confirmation/confirmation';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: Lander },
  { path: 'shop', component: Shop },
  { path: 'warenkorb', component: Cart },
  { path: 'checkout', component: Checkout },
  { path: 'confirmation', component: Confirmation },
  { path: 'login', component: Login },
  { path: 'employee', component: Employee, canActivate: [authGuard] }
];
