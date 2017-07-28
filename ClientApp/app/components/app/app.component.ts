import { CookieService } from 'ngx-cookie';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    constructor(private cookie: CookieService) { }
    ngOnInit() {
        this.cookie.get(".AspNetCore.Identity.Application");
    }
}
