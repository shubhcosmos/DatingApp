import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(public authservice: AuthService , private alertify: AlertifyService , private router: Router) { }

  model: any = {};
  logged: boolean;
  photoUrl: string ;

  // time: any = new Observable(observer =>setInterval(() => observer.next(new Date().toDateString()), 1000)).subscribe();
  ngOnInit() {
    this.authservice.currentPhotoUrl.subscribe(photoUrlfrmAuthService => this.photoUrl = photoUrlfrmAuthService);
  }

  login(): void {
      this.authservice.login(this.model).subscribe(
        x => {
          this.alertify.success('Logged in successfully');
                 }, err => this.alertify.error(err) , () => {
                   this.router.navigate(['/members']);
                 }
      );
     }

    //  login(): void {
    //   this.authservice.login(this.model);
    //  }

       loggedIn() {
        this.logged = this.authservice.loggedIn();
    }

    logout() {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      this.authservice.decodedToken = null;
      this.authservice.currentUser = null ;
           this.alertify.message('logged out');
           this.router.navigate(['/home']);
    }

}
