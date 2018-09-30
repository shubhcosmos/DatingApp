import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode: Boolean = false;

  constructor(private http: HttpClient) { }


  ngOnInit() {

  }
  registerToggle() {
  this.registerMode = true; // !this.registerMode;
  }

  cancelregisterMode(registerMode: boolean) {
this.registerMode = registerMode;

  }

 // getValues() {}

}
