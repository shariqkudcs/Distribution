import { Component, OnDestroy } from '@angular/core';
import { Product } from '../models/product.model';
import { InventoryService } from '../services/inventory.service';
import { Subscription } from 'rxjs';

@Component({

  selector: 'app-add-item',
  templateUrl: './add-item.component.html',
  styleUrls: ['./add-item.component.css']
  
})

export class AddItemComponent implements OnDestroy {

  model: Product;
  private addItemSubscription?: Subscription;

  constructor( private inventoryService: InventoryService){
    this.model = {
      name: '',
      description:''
    }
  }

  ngOnDestroy(): void {
    this.addItemSubscription?.unsubscribe();
  }

  onFormSubmit(){

    this.addItemSubscription = this.inventoryService.addItem(this.model).subscribe(
      {
        next: (response)=>{

          console.log('success')
        },
        error: (error) =>{
          console.log('failed')
        }
      }
    )
  }
}
