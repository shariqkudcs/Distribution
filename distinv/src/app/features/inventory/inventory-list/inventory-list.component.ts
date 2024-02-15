import { Component } from '@angular/core';
import { SignalRService } from '../services/signal-r.service';

@Component({
  selector: 'app-inventory-list',
  templateUrl: './inventory-list.component.html',
  styleUrls: ['./inventory-list.component.css']
})

export class InventoryListComponent {
  getItems() {
    return this.signalRService.data;
  }
  constructor(private signalRService: SignalRService){}

}
