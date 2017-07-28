import { AuthService } from './../../services/auth.service';
import { IUser } from "./../../interfaces/IUser";
import { Component, OnInit, ViewChild } from "@angular/core";

@Component({
  selector: "app-signup",
  templateUrl: "./signup.component.html",
  styleUrls: ["./signup.component.css"]
})
export class SignupComponent implements OnInit {
  @ViewChild('imgInput') imgInput;
  user: IUser = <IUser>{};
  constructor(private service: AuthService) { }

  ngOnInit() {
  }

  onFormSubmit(x) {
    console.log(x);
    const img = this.getImageFile();
    this.service.register(this.user,img);
  }
  getImageFile(): File {
    const fileBrowser = this.imgInput.nativeElement;
    if (fileBrowser.files && fileBrowser.files[0]) {
      return fileBrowser.files[0];
    }
    return null;
  }

}
