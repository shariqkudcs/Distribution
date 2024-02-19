import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})

export class SignalRService {

  public data: Product[] = [];

  private hubConnection!: signalR.HubConnection;

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:5263/data')
    .build();

    this.hubConnection.start().then(()=>console.log('connection started')).catch(err=> console.log('error connecting ' +  err))
  }

  public addInventoryDataListener = () => {
    this.hubConnection.on('inventorydata', (data) => {
        this.data.push(data);
      console.log(data);
    })
  }
  constructor() { 

  }
}
