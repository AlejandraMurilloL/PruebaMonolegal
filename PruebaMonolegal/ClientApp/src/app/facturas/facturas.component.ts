import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Factura } from './Factura';
import { DataService } from '../data.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-facturas',
  templateUrl: './facturas.component.html',
  styleUrls: ['./facturas.component.css']
})
export class FacturasComponent implements OnInit {

  public facturas: Factura[];
  public enviado: boolean;

  constructor(private router: Router, private dataService: DataService) {  }

  getAllFacturas() {
    this.dataService.getAllFacturas().subscribe(result => { this.facturas = result }, error => { console.log(<any>error); });

  }

  notificarTodos() {
    this.dataService.notificarClientes().subscribe(result => { this.facturas = result }, error => { console.log(<any>error); });
    this.enviado = true;
  }

  ngOnInit() {
    localStorage.clear();
    this.getAllFacturas();
  } 
  

}


