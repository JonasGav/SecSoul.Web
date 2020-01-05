import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams, IAfterGuiAttachedParams } from 'ag-grid-community';

@Component({
  selector: 'app-button-renderer',
  template: `
  <span *ngIf="!this.params.node.data.isProcessed"><button disabled="true" style="height: 20px" (click)="invokeParentMethod()" class="btn btn-info">Download</button></span>
  <span *ngIf="this.params.node.data.isProcessed"><button style="height: 20px" (click)="invokeParentMethod()" class="btn btn-info">Download</button></span>
  `,
  styles: [
    `.btn {
        line-height: 0.5
    }`
]
})

export class ChildMessageRenderer implements ICellRendererAngularComp {
    public params: any;

    agInit(params: any): void {
        this.params = params;
        console.log(this.params.node.data.isProcessed);
    }

    public invokeParentMethod() {
        console.log(this.params);
        //this.params.context.componentParent.methodFromParent(`Row: ${this.params.node.rowIndex}, Col: ${this.params.colDef.headerName}`)
        this.params.context.componentParent.methodFromParent(this.params.node.data);
    }

    refresh(): boolean {
        return false;
    }
}