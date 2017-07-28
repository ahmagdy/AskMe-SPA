import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router/";
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private service:AuthService,private router:Router,private title:Title) { }
  user = {email:'',password:''};
  ngOnInit() {
      if (this.service.isAuthenticated) this.router.navigate(['/dashboard']);
      this.title.setTitle('Login Page');
  }

  loginSubmit(vals){
    this.service.login(vals.email,vals.password);
  }
}
