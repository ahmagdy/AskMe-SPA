import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
// import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { sharedConfig } from './app.module.shared';
import { SnotifyService } from "ng-snotify";
import { AuthGuard } from "./services/auth-guard.service";
import { AuthService } from "./services/auth.service";
import { CommService } from "./services/comm.service";
import { HttpModule } from "@angular/http";

@NgModule({
    bootstrap: sharedConfig.bootstrap,
    declarations: sharedConfig.declarations,
    imports: [
        BrowserModule,
        FormsModule,
         HttpModule,
        HttpClientModule,
        ...sharedConfig.imports
    ],
    providers: [
        { provide: 'ORIGIN_URL', useValue: location.origin },
        SnotifyService,
        AuthService,
        AuthGuard,
        CommService
    ]
})
export class AppModule {
}
