using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaMonolegal.IRepository;
using PruebaMonolegal.Models;

namespace PruebaMonolegal.Controllers
{
    [Route("api/[controller]")]
    public class FacturaController : Controller
    {
        private readonly IFacturaRepository _facturaRepository; 

        public FacturaController(IFacturaRepository facturaRepository)
        {
            _facturaRepository = facturaRepository;
        }

        [HttpGet("[action]")]
        public IEnumerable<Factura> GetAll()
        {
            IEnumerable<Factura> facturas = _facturaRepository.GetAll();
            return facturas;
        }

        [HttpGet("[action]")]
        public IEnumerable<Factura> NotificarTodos()
        {
            _facturaRepository.NotificarTodos();
            return _facturaRepository.GetAll();
        }
    }
}