using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManagement
{
    using Newtonsoft.Json;

    public class Portfolio
    {
        public List<Transaction> Ledger;

        public Portfolio()
        {
            Ledger = new List<Transaction>();
        }

        public decimal GetPortfolioValue()
        {
            return Ledger.Sum(t => (t.Stock?.Value ?? 0) * t.Quantity);
        }

        public decimal GetPortfolioInvestedValue()
        {
            return Ledger.Sum(t => t.Price * t.Quantity);
        }

        public decimal GetPortfolioGainLoss()
        {
            return this.GetPortfolioValue() - this.GetPortfolioInvestedValue();
        }

        public decimal GetPortfolioGainLossPct()
        {
            var previousValue = this.GetPortfolioInvestedValue();
            var currentValue = this.GetPortfolioValue();

            if (previousValue > 0 && currentValue >= 0)
            {
                return (currentValue / previousValue) - 1;
            }

            return -2;
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

        [JsonConstructor]
        public Transaction(string isin, int quantity, decimal price)
        {
            Isin = isin;
            Quantity = quantity;
            Price = price;
        }

        [JsonIgnore]
        public Stock Stock { get; set; }

        private string _isin;
        public string Isin
        {
            get => this.Stock?.Isin ?? this._isin;
            set => this._isin = value;
        }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }

    public class Stock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Stock"/> class. 
        /// Create a Stock from a Json object
        /// </summary>
        /// <param name="item">
        /// Json Object (returned by JObject.Parse() method) used to build the <see cref="Stock"/>
        /// </param>
        public Stock(dynamic item)
        {
            if (item?.values != null)
            {
                this.Id = GetStockId(item.urn.ToString());
                this.Isin = item.isinCode;
                this.Name = item.fullName._default;
                this.Ticker = item.ticker;
                this.Value = item.values.lastPrice == null ? -1 : item.values.lastPrice;
                this.Pct = item.values.dayChangePercentage == null ? -1 : item.values.dayChangePercentage;
                this.LastUpdate = item.values.updatedOn == null ? DateTime.MinValue : item.values.updatedOn; // item.values.lastTime;
                this.Info = item.ToString();
            }
        }
        
        public string Id { get; set; }

        public string Isin { get; set; }

        public string Name { get; set; }

        public string Ticker { get; set; }

        public decimal Value { get; set; }

        public decimal Pct { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool Highlight { get; set; }

        [JsonIgnore]
        public string Info { get; set; }

        public static string GetStockId(string id)
        {
            return id.Replace("urn:issue:", string.Empty);            
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

}
