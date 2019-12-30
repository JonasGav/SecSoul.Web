import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScanPanelListComponent } from './scan-panel-list.component';

describe('ScanPanelListComponent', () => {
  let component: ScanPanelListComponent;
  let fixture: ComponentFixture<ScanPanelListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScanPanelListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScanPanelListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
