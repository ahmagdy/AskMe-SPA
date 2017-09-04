import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppModuleShared } from './app.module.shared';
import { AppComponent } from './components/app/app.component';
import { HttpModule } from "@angular/http";
import { HttpClientModule } from "@angular/common/http";

import { SnotifyService } from "ng-snotify";
import { AuthGuard } from "./services/auth-guard.service";
import { AuthService } from "./services/auth.service";
import { CommService } from "./services/comm.service";

@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        BrowserModule,
        HttpModule,
        HttpClientModule,
        AppModuleShared
    ],
    providers: [
        { provide: 'BASE_URL', useFactory: getBaseUrl },
        SnotifyService,
        AuthService,
        AuthGuard,
        CommService
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}
