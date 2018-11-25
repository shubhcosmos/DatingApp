import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';


@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  user: User;
  galOpts: NgxGalleryOptions[];
  galImgs: NgxGalleryImage[];
  constructor(private userService: UserService, private alertify: AlertifyService , private actroute: ActivatedRoute) { }



  ngOnInit(): void {
    this.actroute.data.subscribe((data) => {
      this.user = data['user'];
    } ) ;
    this.galOpts  = [
      {
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
      }
    ];
    this.galImgs = this.getImages();
  }

getImages() {

  const imageUrls = [];

  for ( let i = 0 ; i < this.user.photos.length ; i++) {

  imageUrls.push({
    small: this.user.photos[i].url,
    medium: this.user.photos[i].url,
    big: this.user.photos[i].url,
    description: this.user.photos[i].description

  }
  );
  }

  return imageUrls;
}

  // tslint:disable-next-line:eofline
  }