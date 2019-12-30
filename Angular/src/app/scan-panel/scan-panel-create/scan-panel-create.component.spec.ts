import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScanPanelCreateComponent } from './scan-panel-create.component';

describe('ScanPanelCreateComponent', () => {
  let component: ScanPanelCreateComponent;
  let fixture: ComponentFixture<ScanPanelCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScanPanelCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScanPanelCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
