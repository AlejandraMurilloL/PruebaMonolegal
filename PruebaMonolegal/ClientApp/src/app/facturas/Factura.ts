export interface Factura {
  codigoFactura: string;
  cliente: string;
  nit: string;
  ciudad: string;
  totalFactura: number;
  subtotal: number;
  iva: number;
  retencion: number;
  fechaCreacion: Date;
  estado: string;
  paga: boolean;
  fechaPago: Date;
}
