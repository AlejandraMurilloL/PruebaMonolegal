using PruebaMonolegal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaMonolegal.IRepository
{
    public interface IFacturaRepository
    {
        IEnumerable<Factura> GetAll();
        void Update(string CodigoFactura, Factura factura);
        void NotificarTodos();
        void EnviarEmails(Dictionary<string, string> listaPrimer, Dictionary<string, string> listaSegundo, Dictionary<string, string> listaAcumulado);
        void ActualizarEstadoBD(IEnumerable<Factura> facturasPrimerRecordatorio, IEnumerable<Factura> facturasSegundoRecordatorio, Dictionary<string, string> listaAcumulado);
    }
}
