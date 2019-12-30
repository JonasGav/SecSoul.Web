import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScanPanelCreateComponent } from './scan-panel-create/scan-panel-create.component';
import { ScanPanelListComponent } from './scan-panel-list/scan-panel-list.component';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AgGridModule } from 'ag-grid-angular';

import { ChildMessageRenderer } from "./renderer/child-message-renderer.component";



@NgModule({
  declarations: [ScanPanelCreateComponent, ScanPanelListComponent, ChildMessageRenderer],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
    AgGridModule.withComponents([ChildMessageRenderer])
  ]
})
export class ScanPanelModule { }
