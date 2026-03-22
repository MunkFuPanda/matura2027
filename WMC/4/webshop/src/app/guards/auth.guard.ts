import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthenticationService } from '../service/authentication.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthenticationService);
  const router = inject(Router);

  console.log('AuthGuard: Checking authentication');

  if (authService.isLoggedIn()) {
    console.log('AuthGuard: User is authenticated');
    return true;
  }

  console.log('AuthGuard: User is not authenticated, redirecting to login');
  router.navigate(['/login']);
  return false;
};
