import { Routes } from '@angular/router';
import { Lander } from './lander/lander';
import { Shop } from './shop/shop';
// import { ShopComponent } from './components/shop/shop.component';
// import { ImpressumComponent } from './components/impressum/impressum.component';
// import { WarenkorbComponent } from './components/warenkorb/warenkorb.component';

export const routes: Routes = [
  { path: '', component: Lander },
  { path: 'shop', component: Shop },
  // { path: 'impressum', component: ImpressumComponent },
  // { path: 'warenkorb', component: WarenkorbComponent },
];
