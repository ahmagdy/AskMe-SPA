import { AuthService } from './../../services/auth.service';
import { IUser } from "./../../interfaces/IUser";
import { Component, OnInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { Title } from "@angular/platform-browser";

@Component({
  selector: "app-signup",
  templateUrl: "./signup.component.html",
  styleUrls: ["./signup.component.css"]
})
export class SignupComponent implements OnInit {
  @ViewChild('imgInput') imgInput;
  user: IUser = <IUser>{};
  constructor(private service: AuthService,private router:Router,private title:Title) { }

  ngOnInit() {
      if (this.service.isAuthenticated) this.router.navigate(['/dashboard']);
      this.title.setTitle('Signup Page');
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
