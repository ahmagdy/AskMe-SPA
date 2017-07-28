import { AuthService } from './../../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { CommService } from "../../../services/comm.service";

@Component({
  selector: 'app-dashboard-content',
  templateUrl: './dashboard-content.component.html',
  styleUrls: ['./dashboard-content.component.css']
})
export class DashboardContentComponent implements OnInit {
  messagesCount;
  username: string;
  constructor(private service: CommService, private authService: AuthService) { }

  ngOnInit() {
    this.service.messagesCount()
      .subscribe(res => {
        this.messagesCount = res.messagesCount;
      });
    this.username = this.authService.FullUserDetails['username']
  }

  getProfileUrl() {
    return `${window.location.origin}/profile/${this.username}`;
  }

}
