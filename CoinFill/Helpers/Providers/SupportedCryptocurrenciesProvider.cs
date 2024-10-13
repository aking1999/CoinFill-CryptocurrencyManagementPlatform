using CoinFill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Helpers.Providers
{
    public static class SupportedCryptocurrenciesProvider
    {
        public static List<Cryptocurrencies> SupportedCryptocurrencies { get; } = new List<Cryptocurrencies>
        {
            new Cryptocurrencies { Id = "3408", Name = "USDC", Color = "#0088cc", Icon = "", MinimumTransactionAmount = 118.61, OrderNumber = 1 },
            new Cryptocurrencies { Id = "825", Name = "USDT", Color = "#009393", Icon = "", MinimumTransactionAmount = 118.59, OrderNumber = 2 },
            new Cryptocurrencies { Id = "1", Name = "BTC", Color = "#b56402", Icon = "", MinimumTransactionAmount = 0.0025, OrderNumber = 3 },
            new Cryptocurrencies { Id = "1027", Name = "ETH", Color = "#040425", Icon = "", MinimumTransactionAmount = 0.05, OrderNumber = 4 },
            new Cryptocurrencies { Id = "2", Name = "LTC", Color = "#345d9d", Icon = "", MinimumTransactionAmount = 1.67, OrderNumber = 5 },
            new Cryptocurrencies { Id = "5426", Name = "SOL", Color = "#1af89e", Icon = "", MinimumTransactionAmount = 1.12, OrderNumber = 5 },
            new Cryptocurrencies { Id = "3890", Name = "MATIC", Color = "#8e34d5", Icon = "", MinimumTransactionAmount = 142.65, OrderNumber = 6 },
            new Cryptocurrencies { Id = "1975", Name = "LINK", Color = "#335dd2", Icon = "", MinimumTransactionAmount = 5.92, OrderNumber = 7 },
            new Cryptocurrencies { Id = "6636", Name = "DOT", Color = "#e6007a", Icon = "", MinimumTransactionAmount = 16.72, OrderNumber = 8 },
            new Cryptocurrencies { Id = "21794", Name = "APT", Color = "#030361", Icon = "", MinimumTransactionAmount = 13.01, OrderNumber = 9 },
            new Cryptocurrencies { Id = "20947", Name = "SUI", Color = "#4ca3ff", Icon = "", MinimumTransactionAmount = 68.03, OrderNumber = 10 },
            new Cryptocurrencies { Id = "11841", Name = "ARB", Color = "#2b9cec", Icon = "", MinimumTransactionAmount = 61.38, OrderNumber = 11 },
            new Cryptocurrencies { Id = "14101", Name = "RON", Color = "#1273ea", Icon = "", MinimumTransactionAmount = 49.16, OrderNumber = 11 },
            new Cryptocurrencies { Id = "2563", Name = "TUSD", Color = "#1a5aff", Icon = "", MinimumTransactionAmount = 120.75, OrderNumber = 12 },
            new Cryptocurrencies { Id = "52", Name = "XRP", Color = "#030361", Icon = "", MinimumTransactionAmount = 226.9, OrderNumber = 13 },
            new Cryptocurrencies { Id = "1958", Name = "TRX", Color = "#ff0013", Icon = "", MinimumTransactionAmount = 956.59, OrderNumber = 14 },
            new Cryptocurrencies { Id = "2010", Name = "ADA", Color = "#0133ae", Icon = "", MinimumTransactionAmount = 220.77, OrderNumber = 15 },
            new Cryptocurrencies { Id = "4943", Name = "DAI", Color = "#b56402", Icon = "", MinimumTransactionAmount = 118.62, OrderNumber = 16 },
            new Cryptocurrencies { Id = "22861", Name = "TIA", Color = "#7a2bf9", Icon = "", MinimumTransactionAmount = 6.19, OrderNumber = 17 },
            //new Cryptocurrencies { Id = "1839", Name = "BNB", Color = "#b56402", Icon = "", MinimumTransactionAmount = 0.37, OrderNumber = 18 },
            new Cryptocurrencies { Id = "3794", Name = "ATOM", Color = "#030361", Icon = "", MinimumTransactionAmount = 12.21, OrderNumber = 19 },
            new Cryptocurrencies { Id = "5805", Name = "AVAX", Color = "#e84142", Icon = "", MinimumTransactionAmount = 3.06, OrderNumber = 20 },
            new Cryptocurrencies { Id = "8916", Name = "ICP", Color = "#f57c2d", Icon = "", MinimumTransactionAmount = 9.06, OrderNumber = 21 },
            new Cryptocurrencies { Id = "7226", Name = "INJ", Color = "#00a8fb", Icon = "", MinimumTransactionAmount = 3.42, OrderNumber = 22 },
            new Cryptocurrencies { Id = "29210", Name = "JUP", Color = "#009393", Icon = "", MinimumTransactionAmount = 238.48, OrderNumber = 23 },
            new Cryptocurrencies { Id = "6535", Name = "NEAR", Color = "#00ec97", Icon = "", MinimumTransactionAmount = 37.66, OrderNumber = 24 },
            new Cryptocurrencies { Id = "11840", Name = "OP", Color = "#fe0420", Icon = "", MinimumTransactionAmount = 33.92, OrderNumber = 25 },
            new Cryptocurrencies { Id = "5690", Name = "RNDR", Color = "#cf1011", Icon = "", MinimumTransactionAmount = 26.03, OrderNumber = 26 },
            new Cryptocurrencies { Id = "20396", Name = "KAS", Color = "#009393", Icon = "", MinimumTransactionAmount = 838.44, OrderNumber = 27 },
            new Cryptocurrencies { Id = "11419", Name = "TON", Color = "#0088cc", Icon = "", MinimumTransactionAmount = 57.1, OrderNumber = 28 },
            new Cryptocurrencies { Id = "23149", Name = "SEI", Color = "#c83748", Icon = "", MinimumTransactionAmount = 176, OrderNumber = 29 },
            new Cryptocurrencies { Id = "4847", Name = "STX", Color = "#5546ff", Icon = "", MinimumTransactionAmount = 62.98, OrderNumber = 30 },
            new Cryptocurrencies { Id = "2280", Name = "FIL", Color = "#0090ff", Icon = "", MinimumTransactionAmount = 22.4, OrderNumber = 31 },
            new Cryptocurrencies { Id = "3513", Name = "FTM", Color = "#1969ff", Icon = "", MinimumTransactionAmount = 310.73, OrderNumber = 32 },
            new Cryptocurrencies { Id = "6719", Name = "GRT", Color = "#5042c0", Icon = "", MinimumTransactionAmount = 712.02, OrderNumber = 33 },
            new Cryptocurrencies { Id = "5665", Name = "HNT", Color = "#474dff", Icon = "", MinimumTransactionAmount = 14.78, OrderNumber = 34 },
            new Cryptocurrencies { Id = "4030", Name = "ALGO", Color = "#030361", Icon = "", MinimumTransactionAmount = 673.36, OrderNumber = 35 },
            new Cryptocurrencies { Id = "6210", Name = "SAND", Color = "#00aeef", Icon = "", MinimumTransactionAmount = 252.03, OrderNumber = 36 },
            new Cryptocurrencies { Id = "6783", Name = "AXS", Color = "#0047dc", Icon = "", MinimumTransactionAmount = 15.43, OrderNumber = 37 },
            new Cryptocurrencies { Id = "4157", Name = "RUNE", Color = "#23edbd", Icon = "", MinimumTransactionAmount = 22.92, OrderNumber = 38 },
            new Cryptocurrencies { Id = "74", Name = "DOGE", Color = "#ba9f33", Icon = "", MinimumTransactionAmount = 1475.79, OrderNumber = 39 },
            new Cryptocurrencies { Id = "7431", Name = "AKT", Color = "#e41e13", Icon = "", MinimumTransactionAmount = 39.12, OrderNumber = 40 },
            new Cryptocurrencies { Id = "7186", Name = "CAKE", Color = "#00a8fb", Icon = "", MinimumTransactionAmount = 47.17, OrderNumber = 41 },
            new Cryptocurrencies { Id = "2694", Name = "NEXO", Color = "#1a4199", Icon = "", MinimumTransactionAmount = 125.98, OrderNumber = 42 },
            new Cryptocurrencies { Id = "3635", Name = "CRO", Color = "#345d9d", Icon = "", MinimumTransactionAmount = 944, OrderNumber = 43 },
            new Cryptocurrencies { Id = "3773", Name = "FET", Color = "#030361", Icon = "", MinimumTransactionAmount = 63, OrderNumber = 44 },
            new Cryptocurrencies { Id = "28846", Name = "AI", Color = "#f23a3c", Icon = "", MinimumTransactionAmount = 71, OrderNumber = 45 },
            new Cryptocurrencies { Id = "2416", Name = "THETA", Color = "#27bcfd", Icon = "", MinimumTransactionAmount = 53, OrderNumber = 46 },
            new Cryptocurrencies { Id = "12220", Name = "OSMO", Color = "#8e34d5", Icon = "", MinimumTransactionAmount = 80.53, OrderNumber = 47 },
            new Cryptocurrencies { Id = "11857", Name = "GMX", Color = "#335de1", Icon = "", MinimumTransactionAmount = 2.18, OrderNumber = 48 },
            new Cryptocurrencies { Id = "6836", Name = "GLMR", Color = "#ff679b", Icon = "", MinimumTransactionAmount = 264, OrderNumber = 49 },
            new Cryptocurrencies { Id = "1168", Name = "DCR", Color = "#2970ff", Icon = "", MinimumTransactionAmount = 5.5, OrderNumber = 50 },
            new Cryptocurrencies { Id = "6892", Name = "EGLD", Color = "#23edbd", Icon = "", MinimumTransactionAmount = 1.87, OrderNumber = 51 },
            new Cryptocurrencies { Id = "7278", Name = "AAVE", Color = "#00ec97", Icon = "", MinimumTransactionAmount = 1.14, OrderNumber = 52 },
            new Cryptocurrencies { Id = "2299", Name = "ELF", Color = "#345d9d", Icon = "", MinimumTransactionAmount = 179, OrderNumber = 53 }
        };
    }
}
