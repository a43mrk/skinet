import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/internal/operators/map';
import { AccountService } from '../account/account.service';

// 203-1 create a guard that verify if the user is logged.
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accountService: AccountService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
      return this.accountService.currentUser$.pipe(
        map(auth => {
          if (auth) {
            return true;
          }
          this.router.navigate(['account/login'], {queryParams: {returnUrl: state.url }});
        })
      )
  }
  
}
