import { Meta } from '@angular/platform-browser';
import { IMessage } from './../interfaces/IMessage';
import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from '@angular/common/http';
import { URLSearchParams } from '@angular/http';
import "rxjs/add/operator/map";
import 'rxjs/add/observable/of';
import { IUser } from '../interfaces/IUser';
import { Observable } from 'rxjs/Observable';
import { Router } from "@angular/router";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { CookieService } from "ngx-cookie";
import { AuthService } from "./auth.service";

@Injectable()
export class CommService {

    //The Message will passed to MessageReply Component to Get Data From it
    messageToReply: BehaviorSubject<IMessage> = new BehaviorSubject<IMessage>(<IMessage>{});

    constructor(private http: HttpClient, private authService: AuthService,
        private router: Router, private ck: CookieService, private meta: Meta) { }


    getUserIP() {
        return this.http.get("https://api.ipify.org/?format=json");
    }

    getUserDetails(username: string) {
        console.log(username);
        this.meta.updateTag({ property: 'viewport', content: 'xx-dev' });
        return this.http.get<IUser>('/api/public/user', { params: new HttpParams().set('username', username) })
            .catch(err => {
                return this.catchError(err, 'Something went wrong', true);
            });
    }

    sendMessage(username: string, message: string) {
        this.getUserIP().subscribe(val => {
            const mS = { username: username, message: message, ip: val['ip'] };
            this.http.post("/api/public/message", mS)
                .subscribe(res => {
                    this.authService.successHandler('Message Sent Successfully');
                }, err => {
                    this.authService.errorHandler("Something wrong happend when we try to send your message")
                });

        });
    }

    messagesCount() {
        return this.http.get<any>('/api/dashboard')
            .catch(err => {
                return this.catchError(err, 'Something went wrong', false);
            });
    }


    allMessages(): Observable<IMessage[]> {
        return this.http.get<IMessage[]>('/api/messages')
            .catch(err => {
                return this.catchError(err, 'Something went wrong', false);
            });
    }

    changeVisibility(id: number) {
        return this.http.post('/api/messages/visibility', { id })
            .catch(err => {
                return this.catchError(err, 'Unable to change message', false);
            });
    }
    deleteMessage(id) {
        return this.http.delete('/api/messages', { params: new HttpParams().set("id", id) })
            .catch(err => {
                return this.catchError(err, "Unable to change message", false);
            });
    }

    getPreviousReply(id) {
        return this.http.get("/api/messages/reply", { params: new HttpParams().set("id", id) })
            .catch(err => {
                return this.catchError(err, "Something wrong", false);
            });
    }

    passMessage(message: IMessage) {
        this.messageToReply.next(message);
    }

    sendReply(message: IMessage) {
        this.http.post("/api/messages/reply", message)
            .subscribe(res => {
                this.authService.successHandler("Reply Sent Successfully");
                this.router.navigate(["dashboard/messages"]);
            }, err => {
                this.authService.errorHandler("Something wrong happend when we try to send your message")
            });
    }

    upadteUser(user: IUser, img: File) {
        console.log(user);
        const fd = new FormData();

        for (var k in user) {
            fd.append(k, user[k]);
        }
        fd.append("image", img);
        this.http.put<IUser>("/api/dashboard/user", fd)
            .subscribe(res => {
                console.log(res);
                this.authService.successHandler("Updated Successfully");
                this.ck.putObject("user", res, { secure: true });
                this.router.navigate(["/dashboard"]);
                //TODO Login

            }, error => {
                console.log(error);
                this.authService.errorHandler(`Unable to update ${error.error || error.statusText}`);
            });
    }

    updatePassword(oldPassword: string, newPassword: string) {
        const form = new FormData();
        form.append("oldPassword", oldPassword);
        form.append("newPassword", newPassword);
        this.http.put("/api/dashboard/reset", form)
            .subscribe(() => {
                this.authService.successHandler("Updated Successfully");
                this.router.navigate(["/dashboard/info"]);
            },
            error => {
                console.log(error);
                this.authService.errorHandler(`Unable to complete process ${error.error || "Password mismatch"}`);
            });
    }

    getVisibleMessages(username: string) {
        return this.http.get<IMessage[]>("/api/public/user/messages",
            { params: new HttpParams().set("username", username) })
            .catch(err => {
                return this.catchError(err, "Something wrong", false);
            });

    }
    catchError(error, message: string, nav: boolean) {
        console.log(error);
        this.authService.errorHandler(`${message}, ${error.error.message || error.statusText}`);
        if (nav) { this.router.navigate(["/"]) };
        return Observable.of(error);
    }

}



