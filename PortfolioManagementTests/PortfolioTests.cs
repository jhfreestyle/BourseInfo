using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioManagement;

namespace PortfolioManagementTests
{
    [TestClass]
    public class PortfolioTests
    {
        [TestMethod]
        public void GetPortfolioValue_return_null_if_stock_is_null()
        {
            Portfolio p = new Portfolio();
            p.AddTransaction(null,1,1);

            Assert.AreEqual(0, p.GetPortfolioValue());
        }
    }
}
