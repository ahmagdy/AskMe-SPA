import { CookieService } from 'ngx-cookie';
import { AuthService } from './auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';


@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private service: AuthService, private router: Router, private ck: CookieService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const url = state.url;
    return this.checkLogin(url);
  }

  checkLogin(url: string): boolean {
    // if (this.service.isAuthenticated) {
    //   return true;
    // }
    if (this.ck.get('user')) {
      return true;
    }


    this.service.redirectUrl = url;
    this.router.navigate(['/login']);
    return false;
  }


}
