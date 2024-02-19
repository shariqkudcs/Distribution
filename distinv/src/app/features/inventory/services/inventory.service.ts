import { Injectable } from '@angular/core';
import { Product } from '../models/product.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class InventoryService {

  constructor(private http: HttpClient) { }

  addItem(model: Product): Observable<void> {
    return this.http.post<void>('http://localhost:5263/api/inventory', model)
  }
}
