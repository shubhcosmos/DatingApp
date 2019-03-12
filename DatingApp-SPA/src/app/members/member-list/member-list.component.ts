import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

   pagination: Pagination ;
  constructor(private userService: UserService, private alertify: AlertifyService, private actroute: ActivatedRoute) { }
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList = [{'value': 'male' , 'display': 'Males'} , {'value': 'female' , 'display': 'Females'}] ;
  pageSize = [ {'value': '5' , 'display': '5'} , {'value': '10' , 'display': '10'}] ;
  userParams: any = {} ;
  ngOnInit() {
    this.actroute.data.subscribe(data => {
    this.users = data['users'].result;
    this.pagination = data['users'].pagination;

   }
    );
    this.userParams.gender = this.user.gender;
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female' ;

    this.userParams.minAge = 18;
    this.userParams.maxAge = 99 ;
    this.userParams.orderBy = 'lastActive';
    this.pagination.itemsPerPage = 5;
  }


  resetFilters() {
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female' ;

    this.userParams.minAge = 18;
    this.userParams.maxAge = 99 ;
    this.loadUsers();



  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();

  }

  pageChangedEvent() {

    this.loadUsers();
  }

loadUsers() {

this.userService.getUsers(this.pagination.currentPage , this.pagination.itemsPerPage , this.userParams).subscribe( paginatedresult => {
  this.users = paginatedresult.result ;
  this.pagination = paginatedresult.pagination ;
 } , err => this.alertify.error(err));
}


}
