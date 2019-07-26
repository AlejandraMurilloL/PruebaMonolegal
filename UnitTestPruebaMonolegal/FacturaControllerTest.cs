using Microsoft.AspNetCore.Mvc;
using PruebaMonolegal.Controllers;
using PruebaMonolegal.IRepository;
using PruebaMonolegal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTestPruebaMonolegal
{
    public class FacturaControllerTest
    {

        FacturaController _controller;
        IFacturaRepository _service;

        public FacturaControllerTest()
        {
            _service = new FacturaRepositoryFake();
            _controller = new FacturaController(_service);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.GetAll();

            // Assert
            var items = Assert.IsType<List<Factura>>(okResult);
            Assert.Equal(8, items.Count);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllUpdatedItems()
        {
            // Act
            var okResult = _controller.NotificarTodos();

            // Assert
            var items = Assert.IsType<List<Factura>>(okResult);
            Assert.Equal(8, items.Count);
        }
    }
}
