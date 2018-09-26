import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private authservice: AuthService) { }

  model: any = {};

  ngOnInit() {
  }

  login(): void {
      this.authservice.login(this.model).subscribe(
        x => {
          console.log('Logged in successfully');
        }, err => console.log('Failed to Login')
      );
     }

    loggedIn() {
      const token = localStorage.getItem('token');
      return !!token;
    }

    logout() {
      localStorage.removeItem('token');
    }

}
