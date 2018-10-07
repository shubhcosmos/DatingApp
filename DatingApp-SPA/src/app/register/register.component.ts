import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private authservice: AuthService, private alertify: AlertifyService) { }

  model: any = {};
 // @Input() valuesFromHome: any;
  @Output() cancelregister = new EventEmitter();

  ngOnInit() {
  }

  register() {
    this.authservice.register(this.model).subscribe(
      () => this.alertify.success('Registration successful') , err => this.alertify.error('Registration error - ' + JSON.stringify(err))
    );
  }
  cancel() {
    this.cancelregister.emit(false);
   // console.log('cancelled');
  }

}
