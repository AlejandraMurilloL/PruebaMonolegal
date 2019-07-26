using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaMonolegal.Models
{
    public class Factura
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        public String CodigoFactura { get; set; }
        public String Cliente { get; set; }
        public String Ciudad { get; set; }
        public String Nit { get; set; }
        public String Email { get; set; }
        public double TotalFactura { get; set; }
        public double Subtotal { get; set; }
        public double Iva { get; set; }
        public double Retencion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public String Estado { get; set; }
        public bool Paga { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
