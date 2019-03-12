import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()


 export class ListsResolver implements Resolve<User[]> {
  pageNumber = 1 ;
likesParam = 'Likers';
  pageSize = 5 ;

    constructor(private userService: UserService, private router: Router , private alertify: AlertifyService) {}

 resolve(route: ActivatedRouteSnapshot): Observable<User[]> {



 return this.userService.getUsers(this.pageNumber , this.pageSize, null, this.likesParam).pipe(catchError(err  => {
     this.alertify.error('Failed to get Users');
     this.router.navigate(['/home']);
     return of (null);
 }
   ));
 }
 }
