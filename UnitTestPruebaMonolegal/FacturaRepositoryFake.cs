using Datos.MailServices;
using PruebaMonolegal.IRepository;
using PruebaMonolegal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestPruebaMonolegal
{
    class FacturaRepositoryFake: IFacturaRepository
    {
        private List<Factura> _facturas;

        public FacturaRepositoryFake()
        {
            _facturas = new List<Factura>()
            {
                new Factura() { CodigoFactura = "P-00001", Cliente = "Cliente 1",Nit = "11111111",Email = "malemurillo15@gmail.com",Ciudad = "Ciudad 1",TotalFactura = 19278,Subtotal = 16200,Iva = 3078,Retencion = 0, FechaCreacion = new DateTime(2019,07,20),Estado =  "primerrecordatorio", Paga = false },
                new Factura() { CodigoFactura = "P-00002", Cliente = "Cliente 1",Nit = "11111111",Email = "malemurillo15@gmail.com",Ciudad = "Ciudad 1",TotalFactura = 21539,Subtotal = 18100,Iva = 3439,Retencion = 0, FechaCreacion = new DateTime(2019,07,20),Estado =  "primerrecordatorio", Paga = false },                    
                new Factura() { CodigoFactura = "P-00003", Cliente = "Cliente 1",Nit = "11111111",Email = "malemurillo15@gmail.com",Ciudad = "Ciudad 1",TotalFactura = 9520,Subtotal = 8000,Iva = 1520,Retencion = 0, FechaCreacion = new DateTime(2019,07,20),Estado =  "primerrecordatorio", Paga = false },
                new Factura() { CodigoFactura = "P-00004", Cliente = "Cliente 2",Nit = "22222222",Email = "alejandramurillo1510@outlook.es",Ciudad = "Ciudad 2",TotalFactura = 17850,Subtotal = 15000,Iva = 2850,Retencion = 0, FechaCreacion = new DateTime(2019,07,21),Estado =  "segundorecordatorio", Paga = false },
                new Factura() { CodigoFactura = "P-00005", Cliente = "Cliente 3",Nit = "33333333",Email = "mamurillo54@misena.edu.co",Ciudad = "Ciudad 3",TotalFactura = 11067,Subtotal = 9300,Iva = 1767,Retencion = 0, FechaCreacion = new DateTime(2019,07,22),Estado =  "", Paga = false },
                new Factura() { CodigoFactura = "P-00006", Cliente = "Cliente 3",Nit = "33333333",Email = "mamurillo54@misena.edu.co",Ciudad = "Ciudad 3",TotalFactura = 5950,Subtotal = 5000,Iva = 950,Retencion = 0, FechaCreacion = new DateTime(2019,07,22),Estado =  "", Paga = false },
                new Factura() { CodigoFactura = "P-00007", Cliente = "Cliente 3",Nit = "33333333",Email = "mamurillo54@misena.edu.co",Ciudad = "Ciudad 3",TotalFactura = 7140,Subtotal = 6000,Iva = 1140,Retencion = 0, FechaCreacion = new DateTime(2019,07,22),Estado =  "", Paga = false },
                new Factura() { CodigoFactura = "P-00008", Cliente = "Cliente 3",Nit = "33333333",Email = "mamurillo54@misena.edu.co",Ciudad = "Ciudad 3",TotalFactura = 17850,Subtotal = 15000,Iva = 2850,Retencion = 0, FechaCreacion = new DateTime(2019,07,22),Estado =  "", Paga = false },

             };
         }


        public IEnumerable<Factura> GetAll()
        {
             return _facturas;
        }

        public void NotificarTodos()
        {
            Dictionary<string, string> listaPrimer = new Dictionary<string, string>();
            listaPrimer.Add("malemurillo15@gmail.com", "P-0001,P-00002,P-0003");

            Dictionary<string, string> listaSegun = new Dictionary<string, string>();
            listaSegun.Add("alejandramurillo1510@outlook.es", "P-0003");

            Dictionary<string, string> listaAcum = new Dictionary<string, string>();
            listaAcum.Add("mamurillo54@misena.edu.co", "Cliente 3");

            EnviarEmails(listaPrimer, listaSegun, listaAcum);

            List<Factura> listaPActualizacion = new List<Factura>();
            List<Factura> listaSActualizacion = new List<Factura>();
            foreach (var item in _facturas)
            {
                if (item.Estado == "primerrecordatorio") { listaPActualizacion.Add(item); }
                if (item.Estado == "segundorecordatorio") { listaSActualizacion.Add(item); }
            }

            ActualizarEstadoBD(listaPActualizacion, listaSActualizacion, new Dictionary<string, string>());

        }
                         

        public void Update(string CodigoFactura, Factura factura)
        {
            var antiguo = _facturas.Find(x => x.CodigoFactura == CodigoFactura);

            _facturas.Remove(antiguo);
            _facturas.Add(factura);
           
            
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


    }
    
}
