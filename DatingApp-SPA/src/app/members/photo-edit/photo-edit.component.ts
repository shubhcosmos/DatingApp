import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/Photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-edit',
  templateUrl: './photo-edit.component.html',
  styleUrls: ['./photo-edit.component.css']
})
export class PhotoEditComponent implements OnInit {

   uploader: FileUploader;
   hasBaseDropZoneOver = false;
   baseUrl = environment.apiUrl;
   currentmainPhoto: Photo ;

  @Input() photos: Photo[];
  @Output() getmemberPhotoChange = new EventEmitter<string>();
  constructor(private authService: AuthService , private userService: UserService , private alertify: AlertifyService ) { }

  ngOnInit() {
    this.initializeUploader();
  }

   fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;

  }


  initializeUploader() {
    this.uploader = new FileUploader({

      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 *  1024 *  1024

    });

    this.uploader.onAfterAddingFile = (file) => file.withCredentials = false ;

    this.uploader.onSuccessItem = (item, response, status, headers) => {
 if (response) {

  const res: Photo = JSON.parse(response);

            const photo = {
          id: res.id ,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
            };
            this.photos.push(photo);
          }

              };

  }

setMainPhoto(photo: Photo) {

this.userService.setMainPhoto(this.authService.decodedToken.nameid , photo.id).subscribe(() => {
this.currentmainPhoto = this.photos.filter(p => p.isMain === true)[0] ;
this.currentmainPhoto.isMain = false;
photo.isMain = true;
// this.getmemberPhotoChange.emit(photo.url);
this.authService.changeMemberPhoto(photo.url);
this.authService.currentUser.photoUrl = photo.url;
localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
} , err => {
this.alertify.error(err);

});


}

deletePhoto(photoid: number) {
this.alertify.confirm('Are you sure to delete photo ?' , () => {
  this.userService.deletePhoto(this.authService.decodedToken.nameid , photoid).subscribe(
    () => {this.photos.splice(this.photos.findIndex(p => p.id === photoid), 1 );
    this.alertify.success('Photo deleted successfully');
    } ,
     err => this.alertify.error('Failed to delete photo')

    );

});

}

}
