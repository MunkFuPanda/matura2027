import { ProductDto } from './product-dto.model';

export class Order {
  id?: number;
  salutation?: string;
  firstName?: string;
  lastName?: string;
  street?: string;
  city?: string;
  zipCode?: string;
  canceled?: string;
  finished?: string;
  totalPrice?: number;
  productList?: ProductDto[];
}
