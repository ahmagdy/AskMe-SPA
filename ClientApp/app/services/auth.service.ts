import { IUser } from "./../interfaces/IUser";
import { Injectable } from "@angular/core";
import { Http, Headers, RequestOptions } from "@angular/http";
import { HttpClient } from '@angular/common/http';
import { Router } from "@angular/router";
import { SnotifyService } from "ng-snotify";
import 'rxjs/add/operator/map'
import 'rxjs/add/operator/catch'
import { CookieService } from 'ngx-cookie';


@Injectable()
export class AuthService {
    userCK = "user";
    redirectUrl: string;

    constructor(private http: HttpClient, private router: Router,
        private toast: SnotifyService, private ck: CookieService) { }

    get FullUserDetails() {
        return this.ck.getObject(this.userCK);
    }

    get isAuthenticated() {
        return !!this.ck.get(this.userCK);
    }

    //Moved to HTTPOnly Cookie set from SErver
    // get tokenHeader() {
    //   const header = new Headers({
    //     "Autorization": `Bearer ${this.ck.get(this.token)}`
    //   });
    //   return new RequestOptions({ headers: header });
    // }

    register(user: IUser, img: File) {
        console.log(user);
        const fd = new FormData();

        for (var k in user) {
            fd.append(k, user[k]);
        }
        fd.append('image', img);
        fd.append('ImageUrl', 'texttest');
        this.http.post("/api/authapi/signup", fd)
            .subscribe(res => {
                this.successHandler('SignedUp Successfully please login');
                this.router.navigate(['/']);
                //TODO Login

            }, error => {
                console.log(error);
                this.errorHandler(`Unable to signup ${error.error.message || error.error || error.statusText}`);
            });
    }

    login(email: string, password: string) {
        this.http.post<{ user: any, role: string }>('/api/authapi/login', { email, password })
            .subscribe(res => {
                res.user.role = res.role;
                delete res.user.password;
                this.ck.putObject(this.userCK, res.user, { secure: true });
                this.successHandler('Done ;)');
                const redirect = this.redirectUrl ? this.redirectUrl : '/dashboard';
                this.router.navigate([redirect]);

            }, error => {
                console.log(error.error.message);
                this.errorHandler(`Unable to login ${error.error.message || error.statusText}`);
            });
    }

    logout() {
        this.http.delete('/api/authapi/logout').subscribe(res => {
            this.ck.removeAll();
            this.router.navigate(['/login']);
        }, err => this.errorHandler('Unable to logout'));
    }



    public errorHandler(text) {
        this.toast.error(text, "Something Wrong", { timeout: 5000 });
    }

    public successHandler(text) {
        this.toast.success(text, "Done", { timeout: 5000 });
    }
}
