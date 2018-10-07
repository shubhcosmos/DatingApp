import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(public authservice: AuthService , private alertify: AlertifyService) { }

  model: any = {};
  logged: boolean;

  ngOnInit() {
  }

  login(): void {
      this.authservice.login(this.model).subscribe(
        x => {
          this.alertify.success('Logged in successfully');
                 }, err => this.alertify.error(err)
      );
     }

       loggedIn() {
        this.logged = this.authservice.loggedIn();
    }

    logout() {
      localStorage.removeItem('token');
           this.alertify.message('logged out');
    }

}
