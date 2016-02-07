using QuantConnect.Data;
using QuantConnect.Data.Market;
using System;

namespace QuantConnect.Algorithm.HarvestCapital
{
    /// <summary>
    /// Basic template algorithm simply initializes the date range and cash
    /// </summary>
    public class BasicTemplateAlgorithm : QCAlgorithm
    {
        /// <summary>
        /// Initialise the data and resolution required, as well as the cash and start-end dates for your algorithm. All algorithms must initialized.
        /// </summary>
        public override void Initialize()
        {
            SetStartDate(2015, 7, 1);   //Set Start Date
            SetEndDate(2015, 7, 15);    //Set End Date
            SetCash(100000);            //Set Strategy Cash
                       
            // Find more symbols here: http://quantconnect.com/data
            AddSecurity(SecurityType.Equity, "SPY", Resolution.Daily);

            //var s = Securities["SPY"];
            //var s = Portfolio.Securities["SPY"];
            //Transactions
            
        }

        /// <summary>
        /// OnData event is the primary entry point for your algorithm. Each new data point will be pumped in here.
        /// </summary>
        /// <param name="data">Slice object keyed by symbol containing the stock data</param>
        public override void OnData(Slice data)
        {            
            var SPY = data["SPY"];

            Plot("TEST", SPY.Close);

            var orders = this.Transactions.GetOrders(x => x.Symbol == SPY.Symbol);

            

            if (!Portfolio.Invested)
            {
                SetHoldings("SPY", .5);
                Debug("Purchased Stock");                
            }
            else
            {
                MarketOrder("SPY", 10);
                Debug("Purchased Stock");
            }            
        }

        // Second / Minute / Hour / Daily level data
        public void OnData(TradeBars data)
        {

        }

        // Tick level data
        // When Resolution.Tick this is called every second with an array of tick values for that second
        public void OnData(Ticks data)
        {

        }

        public override void OnEndOfDay()
        {
            base.OnEndOfDay();
        }

        public override void OnEndOfAlgorithm()
        {            
            base.OnEndOfAlgorithm();
        }       
    }
}