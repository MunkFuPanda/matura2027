import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Lander } from './lander';

describe('Lander', () => {
  let component: Lander;
  let fixture: ComponentFixture<Lander>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Lander]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Lander);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
