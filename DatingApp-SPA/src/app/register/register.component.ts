import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private authservice: AuthService, private alertify: AlertifyService, private fb: FormBuilder , private router: Router) { }

  user: User;
  bsDatepickerconfig: Partial<BsDatepickerConfig> ;
 // @Input() valuesFromHome: any;
  @Output() cancelregister = new EventEmitter();

  registerForm: FormGroup;

  ngOnInit() {
//     this.registerForm = new FormGroup({
// username : new FormControl('' , Validators.required) ,
// password : new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
// confirmPassword: new FormControl('', Validators.required)

//     }, this.passwordMatchValidator);

this.createregisterForm();

this.bsDatepickerconfig = {
containerClass: 'theme-red'

};

  }

  createregisterForm() {

this.registerForm = this.fb.group({
gender: ['male'],
  username: ['', Validators.required],
  knownAs: ['', Validators.required],
  dateOfBirth: [null, Validators.required],
  city: ['', Validators.required],
  country: ['', Validators.required],
   password : ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
  confirmPassword: ['', Validators.required]
}, {'validator': this.passwordMatchValidator} );

  }

passwordMatchValidator(g: FormGroup) {

  return g.get('password').value ===  g.get('confirmPassword').value ? null : {'mismatch': true} ;


}

  register() {
    // this.authservice.register(this.model).subscribe(
    //   () => this.alertify.success('Registration successful') , err => this.alertify.error('Registration error - ' + JSON.stringify(err))
    // );


    if (this.registerForm.valid) {
this.user = Object.assign({} , this.registerForm.value);

this.authservice.register(this.user).subscribe(() => {
this.alertify.success( 'Registered Successfully');

} , err => this.alertify.error(err) , () => {

this.authservice.login(this.user).subscribe(() => {

this.router.navigate(['/members']);
} , err => this.alertify.error('Could not Login after Registration ' + err)) ;
}
);

    }
  }
  cancel() {
    this.cancelregister.emit(false);
   // console.log('cancelled');
  }

}
