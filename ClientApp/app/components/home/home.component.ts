import { Title } from '@angular/platform-browser';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CommService } from "../../services/comm.service";
import { SnotifyService } from "ng-snotify";

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

    constructor(private toast: SnotifyService, private http: HttpClient, private title: Title) { }

    ngOnInit() { this.title.setTitle('Home'); }

}
