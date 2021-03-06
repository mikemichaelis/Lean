﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using QuantConnect.Data;
using QuantConnect.Orders.Fees;
using QuantConnect.Orders.Fills;
using QuantConnect.Orders.Slippage;

namespace QuantConnect.Securities.Cfd
{
    /// <summary>
    /// CFD Security Object Implementation for CFD Assets
    /// </summary>
    /// <seealso cref="Security"/>
    public class Cfd : Security
    {
        /// <summary>
        /// Constructor for the CFD security
        /// </summary>
        /// <param name="exchangeHours">Defines the hours this exchange is open</param>
        /// <param name="quoteCurrency">The cash object that represent the quote currency</param>
        /// <param name="config">The subscription configuration for this security</param>
        /// <param name="symbolProperties">The symbol properties for this security</param>
        public Cfd(SecurityExchangeHours exchangeHours, Cash quoteCurrency, SubscriptionDataConfig config, SymbolProperties symbolProperties)
            : base(config,
                new CfdExchange(exchangeHours),
                new CfdCache(),
                new CfdPortfolioModel(),
                new ImmediateFillModel(),
                new ConstantFeeModel(0),
                new SpreadSlippageModel(),
                new ImmediateSettlementModel(),
                new CfdMarginModel(50m),
                new CfdDataFilter()
                )
        {
            QuoteCurrency = quoteCurrency;
            Holdings = new CfdHolding(this);
            SymbolProperties = symbolProperties;

            if (symbolProperties == null)
                throw new ArgumentException("CFD requires a valid SymbolProperties argument");

            if (symbolProperties.QuoteCurrency != quoteCurrency.Symbol)
                throw new ArgumentException("CFD SymbolProperties.QuoteCurrency and QuoteCurrency.Symbol do not match.");
        }

        /// <summary>
        /// Gets the Cash object used for converting the quote currency to the account currency
        /// </summary>
        public Cash QuoteCurrency { get; private set; }

        /// <summary>
        /// Gets the symbol properties for this security
        /// </summary>
        /// <remarks>Could possibly be moved to the base Security class</remarks>
        public SymbolProperties SymbolProperties { get; private set; }

        /// <summary>
        /// Gets the quote currency for this CFD security
        /// </summary>
        public string QuoteCurrencySymbol
        {
            get { return SymbolProperties.QuoteCurrency; }
        }

        /// <summary>
        /// Gets the contract multiplier for this CFD security
        /// </summary>
        /// <remarks>
        /// PipValue := ContractMultiplier * PipSize
        /// </remarks>
        public decimal ContractMultiplier
        {
            get { return SymbolProperties.ContractMultiplier; }
        }

        /// <summary>
        /// Gets the pip size for this CFD security
        /// </summary>
        public decimal PipSize
        {
            get { return SymbolProperties.PipSize; }
        }
    }
}
