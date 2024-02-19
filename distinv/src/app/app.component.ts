import { Component, OnInit } from '@angular/core';
import { SignalRService } from './features/inventory/services/signal-r.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'distinv';
  constructor(public signalRService: SignalRService,private http:HttpClient){

  }

  ngOnInit(){
    this.signalRService.startConnection();
    this.signalRService.addInventoryDataListener();
    this.startHttpRequest();
  }

  private startHttpRequest = () => {
    this.http.get('http://localhost:5263/api/inventory')
    .subscribe(res => {
      console.log(res);
    })
  }

}
