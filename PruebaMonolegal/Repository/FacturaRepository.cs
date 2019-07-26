using Datos.MailServices;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using PruebaMonolegal.DbModels;
using PruebaMonolegal.IRepository;
using PruebaMonolegal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaMonolegal.Repository
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly ObjectContext _context = null;

        public FacturaRepository(IOptions<Settings> settings)
        {
            _context = new ObjectContext(settings);
        }

        public IEnumerable<Factura> GetAll()
        {
            return  _context.Facturas.Find(x => true).ToList();
        }

        public void Update(string CodigoFactura, Factura factura)
        {
            _context.Facturas.ReplaceOne(zz => zz.CodigoFactura == CodigoFactura, factura);
        }

        public void NotificarTodos()
        {
            Dictionary<string, string> listaFacturasPrimer = new Dictionary<string, string>();
            Dictionary<string, string> listaFacturasSegundo = new Dictionary<string, string>();
            Dictionary<string, string> listaFacturasAcum = new Dictionary<string, string>();

            string cliente = "",facturas="";
            IEnumerable<Factura> facturasPrimerRecordatorio = _context.Facturas.Find(x => x.Estado == "primerrecordatorio").ToList();
            foreach (var factura in facturasPrimerRecordatorio)
            {
                cliente = factura.Cliente;
                if (factura.Cliente == cliente)
                {
                    if (facturas == "") facturas += factura.CodigoFactura; else facturas += ","+ factura.CodigoFactura;

                    if (factura == facturasPrimerRecordatorio.LastOrDefault())
                    {
                        listaFacturasPrimer.Add(factura.Email, facturas);
                    }
                }else
                {
                    if (facturas != "") listaFacturasPrimer.Add(factura.Email, facturas);
                }
            }
            cliente = "";
            facturas = "";
            IEnumerable<Factura> facturasSegundoRecordatorio = _context.Facturas.Find(x => x.Estado == "segundorecordatorio").ToList();
            foreach (var factura in facturasSegundoRecordatorio)
            {
                cliente = factura.Cliente;
                if (factura.Cliente == cliente)
                {
                    if (facturas == "") facturas += factura.CodigoFactura; else facturas += "," + factura.CodigoFactura;
                    if (factura == facturasPrimerRecordatorio.LastOrDefault())
                    {
                        listaFacturasSegundo.Add(factura.Email, facturas);
                    }
                }
                else
                {
                    if (facturas != "") listaFacturasSegundo.Add(factura.Email, facturas);
                }
            }

            cliente = "";
            facturas = "";
            
            var facturasAcumuladas = _context.Facturas.Aggregate().Group(x => new { Cliente = x.Cliente }, x => new { Email = x.First().Email, Client = x.First().Cliente, Count = x.Sum(s => 1), Sum = x.Sum(y => y.TotalFactura), FacturasSinPago = x.Count(z=> z.Paga == false) }).ToList();
            


            foreach (var factura in facturasAcumuladas)
            {
                if (factura.Count > 3 && factura.Sum >=10000 && factura.FacturasSinPago > 3)
                {
                    listaFacturasAcum.Add(factura.Email, factura.Client);
                }
            }

            EnviarEmails(listaFacturasPrimer, listaFacturasSegundo, listaFacturasAcum);
            ActualizarEstadoBD(facturasPrimerRecordatorio, facturasSegundoRecordatorio, listaFacturasAcum);

        }


        public void EnviarEmails(Dictionary<string, string> listaPrimer, Dictionary<string, string> listaSegundo, Dictionary<string, string> listaAcumulado)
        {
            string msg = "";
            MasterMailServer mailServer = new MasterMailServer();

            foreach (var item in listaPrimer)
            {

                msg = "Estimado Cliente: \n Le informamos que su(s) factura(s) No: " + item.Value + " acaba(n) de pasar de estado: Primer Recordatorio a estado: Segundo Recordatorio. ";
                mailServer.SendMail("Cambio de estado factura(s)", msg, item.Key);
            }

            foreach (var item in listaSegundo)
            {

                msg = "Estimado Cliente: \n Le informamos que su(s) factura(s) No: " + item.Value + " acaba(n) de pasar de estado: Segundo Recordatorio a estado: Desactivado. ";
                mailServer.SendMail("Cambio de estado factura(s)", msg, item.Key);
            }

            foreach (var item in listaAcumulado)
            {

                msg = "Estimado Cliente: \n Le informamos que debido a su deuda de más de $10000 y adicional la acumulación de 3 facturas más pendientes de pago, acaba de pasar a estado desactivado. ";
                mailServer.SendMail("Desactivación", msg, item.Key);
            }
        }

        public void ActualizarEstadoBD(IEnumerable<Factura> facturasPrimerRecordatorio, IEnumerable<Factura> facturasSegundoRecordatorio, Dictionary<string, string> listaAcumulado)
        {
            foreach (var item in facturasPrimerRecordatorio)
            {
                item.Estado = "segundorecordatorio";
                Update(item.CodigoFactura, item);
            }

            foreach (var item in facturasSegundoRecordatorio)
            {
                item.Estado = "desactivado";
                Update(item.CodigoFactura, item);
            }

            foreach (var item in listaAcumulado)
            {
                _context.Facturas.UpdateMany(x => x.Cliente == item.Value, Builders<Factura>.Update.Set(p => p.Estado, "desactivado"), new UpdateOptions { IsUpsert = false });
            }
        }

        
    }
}
