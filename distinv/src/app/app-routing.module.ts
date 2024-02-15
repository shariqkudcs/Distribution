import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InventoryListComponent } from './features/inventory/inventory-list/inventory-list.component';
import { AddItemComponent } from './features/inventory/add-item/add-item.component';

const routes: Routes = [
{
  path: 'admin/inventory',
  component: InventoryListComponent
},
{
  path: 'admin/inventory/add',
  component: AddItemComponent
}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
