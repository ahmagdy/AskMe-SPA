import { SnotifyService } from 'ng-snotify';
import { CommService } from './../../../../services/comm.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  oldPassword: string;
  newPassword: string;

  constructor(private service: CommService, private toast: SnotifyService) { }

  ngOnInit() {
  }
  onSubmit() {
    if (!this.oldPassword || !this.newPassword) {
      this.toast.warning('Enter a valid data');
    } else {
      this.service.updatePassword(this.oldPassword, this.newPassword);
    }


  }

}
