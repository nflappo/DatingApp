import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from '../../../environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { reduce } from 'rxjs/operators';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})

export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() GetMemberPhotoChange = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  baseUrl = environment.apiUrl;
  currentMain: Photo;

  constructor(private authService: AuthService, private userService: UserService,
    private alertify: AlertifyService) { }

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
      // (the size is in bytes, so i transorfm 10 mb into bytes)
      maxFileSize: 10 * 1024 * 1024,
      autoUpload: false
    });
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo);
        if (photo.isMain) {
          this.updateMainPhoto(photo);
        }
      }
    };
  }
  private updateMainPhoto(photo: Photo) {
    this.authService.changeMemberPhoto(photo.url);
    this.authService.currentUser.photoURL = photo.url;
    localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
  }
  setMainPhoto(photo: Photo) {
    this.userService.setMainPhoto(this.authService.decodedToken.nameid, photo.id).subscribe(() => {
      this.alertify.success('Successfully set to main');
      this.currentMain = this.photos.filter(p => p.isMain === true)[0];
      this.currentMain.isMain = false;
      photo.isMain = true;
      // this.GetMemberPhotoChange.emit(photo.url); it's pointless due to the line below, it is just for educational purposes
      this.updateMainPhoto(photo);
    }, error => {
      this.alertify.error(error);
    });
  }
  deletePhoto(id: number) {
    this.alertify.confirm('DatingApp', 'Are you sure to want to delete this photo?', () => {
      this.userService.deletePhoto(this.authService.decodedToken.nameid, id).subscribe(() => {
        this.photos.splice(this.photos.findIndex(p => p.id === id ), 1);
        this.alertify.success('Photo has been deleted');
      }, error => {
        this.alertify.error('Failed to delete photo');
      });
    });
  }

}
