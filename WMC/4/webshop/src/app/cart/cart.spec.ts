import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Cart } from './cart';
import { CartService } from '../service/cart.service';
import { Product } from '../model/product.model';

describe('Cart', () => {
  let component: Cart;
  let fixture: ComponentFixture<Cart>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Cart]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Cart);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

describe('CartService', () => {
  let service: CartService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should add product to cart', () => {
    const product: Product = { id: 1, name: 'Test Product', price: 10 };
    service.addProduct(product);
    expect(service.items().length).toBe(1);
    expect(service.itemCount()).toBe(1);
  });

  it('should increase quantity when adding same product', () => {
    const product: Product = { id: 1, name: 'Test Product', price: 10 };
    service.addProduct(product);
    service.addProduct(product);
    expect(service.items().length).toBe(1);
    expect(service.itemCount()).toBe(2);
  });

  it('should remove product from cart', () => {
    const product: Product = { id: 1, name: 'Test Product', price: 10 };
    service.addProduct(product);
    service.removeProduct(1);
    expect(service.items().length).toBe(0);
  });

  it('should calculate total price correctly', () => {
    const product1: Product = { id: 1, name: 'Product 1', price: 10 };
    const product2: Product = { id: 2, name: 'Product 2', price: 20 };
    service.addProduct(product1);
    service.addProduct(product2);
    expect(service.totalPrice()).toBe(30);
  });
});

