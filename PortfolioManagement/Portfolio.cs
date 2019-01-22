using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement
{
    public partial class Portfolio
    {
        public List<Transaction> Ledger;

        public Portfolio()
        {
            Ledger = new List<Transaction>();
        }

        public decimal GetPortfolioValue()
        {
            return Ledger.Sum(t => t.Stock?.Value ?? 0);
        }

        public void AddTransaction(Stock stock, int quantity, decimal price)
        {
            Ledger.Add(new Transaction(stock, quantity, price));
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
        public string Info { get; set; }

    }

}
