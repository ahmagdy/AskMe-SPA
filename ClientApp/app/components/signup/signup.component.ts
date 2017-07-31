import { AuthService } from './../../services/auth.service';
import { IUser } from "./../../interfaces/IUser";
import { Component, OnInit, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { Title } from "@angular/platform-browser";
import { SnotifyService } from 'ng-snotify';

@Component({
  selector: "app-signup",
  templateUrl: "./signup.component.html",
  styleUrls: ["./signup.component.css"]
})
export class SignupComponent implements OnInit {
  @ViewChild('imgInput') imgInput;
  user: IUser = <IUser>{};
  constructor(private service: AuthService, private router: Router, private title: Title,
    private toast: SnotifyService) { }

  ngOnInit() {
    if (this.service.isAuthenticated) this.router.navigate(['/dashboard']);
    this.title.setTitle('Signup Page');
  }

  onFormSubmit(x) {
    const toastId = this.toast.info('Wait....', 'just a moment', { timeout: 20000 });
    const img = this.getImageFile();
    this.service.register(this.user, img, toastId);
  }
  getImageFile(): File {
    const fileBrowser = this.imgInput.nativeElement;
    if (fileBrowser.files && fileBrowser.files[0]) {
      return fileBrowser.files[0];
    }
    return null;
  }

}
