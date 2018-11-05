using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement
{
    public class Portfolio
    {
        public List<Transaction> Ledger;

        public decimal GetPortfolioValue()
        {
            return Ledger.Sum(t => t.Quantity * t.Stock.Value);
        }

    }

    public class Transaction
    {
        public Transaction(Stock stock, int quantity, decimal price)
        {
            Stock = stock;
            Quantity = quantity;
            Price = price;
        }

        public Stock Stock { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }

    public class Stock
    {
        public string Id { get; set; }
        public string Isin { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public decimal Value { get; set; }
        public decimal Pct { get; set; }

    }

}
