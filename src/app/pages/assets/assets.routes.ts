import {Routes} from "@angular/router";
import {AssetsComponent} from "./assets.component";
import {ListAssetComponent} from "./list-asset/list-asset.component";

export const routes: Routes = [
  {
    path: '',
    component: AssetsComponent,
    children: [
      {
        path: '',
        component: ListAssetComponent
      },
      {
        path: 'inventory',
        loadComponent: () => import('./inventory/inventory.component').then(x => x.InventoryComponent)
      },
      {
        path: 'technologies',
        loadComponent: () => import('./technology/technology.component').then(x => x.TechnologyComponent)
      }
    ]
  }
]
