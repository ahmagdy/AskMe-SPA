import { CommService } from './../../../../services/comm.service';
import { AuthService } from './../../../../services/auth.service';
import { IUser } from './../../../../interfaces/IUser';
import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  user: IUser = <IUser>{};

  @ViewChild("imgInput") imgInput;

  constructor(private authService: AuthService, private service: CommService) { }

  ngOnInit() {
    this.user = this.authService.FullUserDetails as IUser;
  }

  onSubmit() {
    this.service.upadteUser(this.user, this.getImageFile());
  }

  getImageFile(): File {
    const fileBrowser = this.imgInput.nativeElement;
    if (fileBrowser.files && fileBrowser.files[0]) {
      return fileBrowser.files[0];
    }
    return null;
  }

}
