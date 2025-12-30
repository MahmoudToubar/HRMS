import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';

export const roleGuard: CanActivateFn = (route) => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  const allowedRoles = route.data['roles'] as string[];
  const userRole = accountService.getUserRole();

  if (userRole && allowedRoles.includes(userRole)) {
    return true;
  }

  router.navigate(['/employees'], { replaceUrl: true });
  return false;
};
