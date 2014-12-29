using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE3352Assignment2
{
    public class Model_Company
    {
        // the following class defines a Company. In our application, there will be 3 different instances of companies
        // the realTimeData will use a list of companies to store all the information for all companies

        public String companyName;                                              // store the name of the company
        public Double openPrice;                                                // store the open price of the company
        public String companySymbol;                                            // store the ticker symbol of the company
        public Double netChange;                                                // store the net change of the company (updated through update())
        public Double percentChange;                                            // store the percent change of the company (update through update()) 
        private Double lastPrice;                                               // store the last sale price of the company (updated through update())
        private Double volume;                                                  // store the total volume of the company (updated through update())        

        /* In addition to all the information each company must store, each company must also store a list of their pending buy shares orders and their
         * pending sell shares orders. These will be used in many different other classes, including the StockByOrder, StockByPrice observers and the 
         * Bid and Sell forms to check for potential sales */

        public List<Model_BuyOrder> buyOrdersList = new List<Model_BuyOrder>();
        public List<Model_SellOrder> sellOrdersList = new List<Model_SellOrder>();

        //constructor, receives company name, symbol and open price because these never change
        public Model_Company(String name, String symbol, Double open) 
        {
            this.companyName = name;
            this.companySymbol = symbol;
            this.openPrice = open;
            this.volume = 0.0;                          // in the beginning, let this value be 0
            this.lastPrice = 0.0;                       // in the beginning, let this value be 0
            this.netChange = 0.0;                       // in the beginning, let this value be 0
            this.percentChange = 0.0;                   // in the beginning, let this value be 0
        }

        // get and set methods for the private member variable lastPrice
        public Double LastPrice
        {
            get
            {
                return this.lastPrice;
            }
            set
            {
                this.lastPrice = value;
            }
        }

        // get and set methods for the private member varible volume
        public Double Volume
        {
            get
            {
                return this.volume;
            }

            set
            {
                this.volume = value;
            }
        }
    }
}
