import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../shared/user.service';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  userDetails;
  userLoggedIn;


  LoginStatus$ : Observable<boolean>;

  UserName$ : Observable<string>;

  constructor(private router: Router, private service: UserService, private toastr: ToastrService) {
   }

  ngOnInit() {
    this.updateUserInfo();
  }

  updateUserInfo(){
    this.service.getUserProfile().subscribe(
      res => {
        this.userDetails = res;
      },
      err => {
        console.log(err);
      },
    );
  }

  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
    this.updateUserInfo();
  }

}
