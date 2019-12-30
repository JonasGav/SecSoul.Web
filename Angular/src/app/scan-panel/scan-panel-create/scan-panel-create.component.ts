import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from 'src/app/shared/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-scan-panel-create',
  templateUrl: './scan-panel-create.component.html',
  styleUrls: ['./scan-panel-create.component.css']
})
export class ScanPanelCreateComponent implements OnInit {

  userDetails;
  formModel = {
    WebsiteUrl: '',
    WebsiteFtp: ''
  }

  constructor(private router: Router, private service: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.service.getUserProfile().subscribe(
      res => {
        this.userDetails = res;
      },
      err => {
        console.log(err);
      },
    );
  }

  onSubmit(form: NgForm) {
    this.service.ScanWebsite(form.value).subscribe( (res: any) => {
      this.toastr.info('Scan was successfully registered', 'Scan Request');
      this.formModel = {
        WebsiteUrl: '',
        WebsiteFtp: ''
      }
    },
    err => {
      if (err.status == 400)
        this.toastr.error('Failed to register scan request', 'Scan Request');
      else
        console.log(err);
      this.formModel = {
        WebsiteUrl: '',
        WebsiteFtp: ''
      }
    });
  }


  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }
}
