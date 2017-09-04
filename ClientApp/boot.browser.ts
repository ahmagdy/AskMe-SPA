///<reference types="webpack-env" />
import 'reflect-metadata';
import 'zone.js';
import 'bootstrap';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module.browser';
import { SnotifyService } from "ng-snotify";
import { AuthService } from "./app/services/auth.service";
import { AuthGuard } from "./app/services/auth-guard.service";
import { CommService } from "./app/services/comm.service";

if (module.hot) {
    module.hot.accept();
    module.hot.dispose(() => {
        // Before restarting the app, we create a new root element and dispose the old one
        const oldRootElem = document.querySelector('app');
        const newRootElem = document.createElement('app');
        oldRootElem!.parentNode!.insertBefore(newRootElem, oldRootElem);
        modulePromise.then(appModule => appModule.destroy());
    });
} else {
    enableProdMode();
}

// Note: @ng-tools/webpack looks for the following expression when performing production
// builds. Don't change how this line looks, otherwise you may break tree-shaking.
const modulePromise = platformBrowserDynamic().bootstrapModule(AppModule, { providers: [SnotifyService, AuthService, AuthGuard, CommService] });
