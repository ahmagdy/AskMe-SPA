import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { SnotifyModule, SnotifyService } from 'ng-snotify';
import { CookieModule } from 'ngx-cookie';
import { LoginComponent } from "./components/login/login.component";
import { SignupComponent } from "./components/signup/signup.component";
import { ProfileComponent } from "./components/profile/profile.component";
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { DashboardContentComponent } from "./components/dashboard/dashboard-content/dashboard-content.component";
import { MessagesComponent } from "./components/dashboard/messages/messages.component";
import { MessageReplyComponent } from "./components/dashboard/message-reply/message-reply.component";
import { UserInfoComponent } from "./components/dashboard/user-info/user-info.component";
import { UserEditComponent } from "./components/dashboard/user-info/user-edit/user-edit.component";
import { AuthGuard } from "./services/auth-guard.service";
import { AboutComponent } from "./components/about/about.component";

import { ShareButtonsModule } from 'ngx-sharebuttons';
import { ChangePasswordComponent } from "./components/dashboard/user-info/change-password/change-password.component";

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        LoginComponent,
        SignupComponent,
        DashboardComponent,
        DashboardContentComponent,
        MessagesComponent,
        UserInfoComponent,
        UserEditComponent,
        MessageReplyComponent,
        ProfileComponent,
        ChangePasswordComponent,
        AboutComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        SnotifyModule,
        CookieModule.forRoot(),
        ShareButtonsModule.forRoot(),
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'login', component: LoginComponent },
            { path: 'signup', component: SignupComponent },
            { path: 'about', component: AboutComponent },
            { path: 'profile', component: ProfileComponent },
            { path: 'profile/:username', component: ProfileComponent },
            {
                path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard], children: [
                    { path: '', redirectTo: 'overview', pathMatch: 'full' },
                    { path: 'overview', component: DashboardContentComponent },
                    { path: 'messages', component: MessagesComponent },
                    { path: 'messages/reply/:id', component: MessageReplyComponent },
                    {
                        path: 'info', component: UserInfoComponent, children: [
                            { path: 'edit', component: UserEditComponent },
                            { path: 'change-password', component: ChangePasswordComponent }
                        ]
                    }

                ]
            },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
