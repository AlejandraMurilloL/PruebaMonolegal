import { Injectable, Inject} from '@angular/core';
import { HttpHeaders, HttpClient } from "@angular/common/http";
import { Factura } from '../app/facturas/Factura';
import { Observable } from 'rxjs';

@Injectable() 
export class DataService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  notificarClientes(): Observable<Factura[]> {
    return this.http.get<Factura[]>(this.baseUrl + 'api/Factura/NotificarTodos');
  }

  getAllFacturas(): Observable<Factura[]>{
    return this.http.get<Factura[]>(this.baseUrl + 'api/Factura/GetAll');
  }
}
