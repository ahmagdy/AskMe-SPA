import { IUser } from './../../../interfaces/IUser';
import { AuthService } from './../../../services/auth.service';
import { Component, OnInit, EventEmitter } from '@angular/core';
import { CommService } from "../../../services/comm.service";


@Component({
  selector: 'app-dashboard-content',
  templateUrl: './dashboard-content.component.html',
  styleUrls: ['./dashboard-content.component.css']
})
export class DashboardContentComponent implements OnInit {
  messagesCount;
  user: IUser = <IUser>{};
  constructor(private service: CommService, private authService: AuthService) { }

  ngOnInit() {
    this.service.messagesCount()
      .subscribe(res => {
        this.messagesCount = res.messagesCount;
      });
    this.user = this.authService.FullUserDetails as IUser;
  }

  getProfileUrl() {
    return `${window.location.origin}/profile/${this.user.username}`;
  }


}
