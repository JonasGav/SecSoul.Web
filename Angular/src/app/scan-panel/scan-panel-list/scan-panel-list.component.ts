import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { HttpClient } from '@angular/common/http';
import { Module, GridOptions } from "ag-grid-community/";
import { Router } from '@angular/router';
import { UserService } from 'src/app/shared/user.service';
import { ToastrService } from 'ngx-toastr';
import {ChildMessageRenderer} from '../renderer/child-message-renderer.component'

@Component({
  selector: 'app-scan-panel-list',
  templateUrl: './scan-panel-list.component.html',
  styleUrls: ['./scan-panel-list.component.css']
})
export class ScanPanelListComponent implements OnInit {

  private gridApi;
  private gridColumnApi;

  rowDataClicked = {};
  userDetails;
  rowData = null;

  private columnDefs;
  private context;
  private gridOptions:GridOptions;
  frameworkComponents: any;

  constructor(private router: Router, private service: UserService, private toastr: ToastrService) { 


    this.context = { componentParent: this };
    this.frameworkComponents = {
      childMessageRenderer: ChildMessageRenderer
    };
    

    this.columnDefs = [
      { headerName: 'Id', field: 'id' },
      { headerName: 'Website Url', field: 'websiteUrl' },
      { headerName: 'Website Ftp', field: 'websiteFtp' },
      { headerName: 'Request Date', field: 'requestDate' },
      { headerName: 'Is finished', field: 'isProcessed' },
      { headerName: 'Nmap scanned', field: 'nmapScanned' },
      { headerName: 'Page enumeration completed', field: 'dirbScanned' },
      { headerName: 'Virus total Api Scanned', field: 'virusTotalScanned' },
      {
        headerName: "Child/Parent",
        field: "value",
        cellRenderer: "childMessageRenderer",
        colId: "params",
        width: 180
      }
    ];
  }

  
  ngOnInit() {
    this.service.getUserProfile().subscribe(
      res => {
        this.userDetails = res;
      },
      err => {
        console.log(err);
      },
    );
    this.service.GetScanWebsiteList().subscribe(
      res => {
        this.rowData = res;
        console.log(this.rowData);
      },
      err => {
        console.log(err);
      },
    );
  }

  onGridReady(params) {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;

    params.api.sizeColumnsToFit();
  }

  onBtnClick(e) {
    this.rowDataClicked = e.rowData;
    console.log(this.rowDataClicked);
  }

  methodFromParent(cell) {
    // alert("Parent Component Method from " + cell + "!");
    this.toastr.info("It works")
  }
}
