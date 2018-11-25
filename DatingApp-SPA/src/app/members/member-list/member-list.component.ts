import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {


  constructor(private userService: UserService, private alertify: AlertifyService, private actroute: ActivatedRoute) { }
  users: User[];
  ngOnInit() {
    this.actroute.data.subscribe(data => {
    this.users = data['users'];
   }
    );
  }


}
