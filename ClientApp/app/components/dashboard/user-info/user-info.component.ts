import { IUser } from './../../../interfaces/IUser';
import { AuthService } from './../../../services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent implements OnInit {
  user: IUser;
  constructor(private service: AuthService) { }

  ngOnInit() {
    this.user = this.service.FullUserDetails as IUser;
    this.user.imageUrl = newImageUrl(this.user.imageUrl);
  }

}

function newImageUrl(imgURl: string) {
  if (imgURl == null) return '';
  return imgURl.substring(imgURl.indexOf('uploads'))
}
