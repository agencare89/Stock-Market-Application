using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE3352Assignment2
{
    public class Model_BuyOrder: Model_Order
    {
        // This class is used to store information for when buy orders are submitted (via the Bid Order Form)
        private DateTime orderDateTime;                             // member variable for the time the bid was submitted
        private Double orderSize;                                   // member variable for the size of the bid order (how many shares) 
        private Double orderPrice;                                  // member variable for the price of the bid order (how much are we buying for)

        // constructor receives the number of shares and the price per share
        public Model_BuyOrder(Double size, Double price)
        {
            this.orderDateTime = DateTime.Now;                      // set the date to be the current time
            this.orderSize = size;
            this.orderPrice = price;
            
        }

        // get and set methods for the private variable orderPrice
        public Double OrderPrice
        {
            get
            {
                return this.orderPrice; 
            }
            set
            {
                this.orderPrice = value;
            }
        }

        // get method for the private variable orderDateTime; note: no set because we will not set the time
        public DateTime OrderDateTime
        {
            get
            {
                return this.orderDateTime;
            }
        }

        // get and set methods for the private variable orderSize 
        public Double OrderSize
        {
            get
            {
                return this.orderSize;
            }
            set
            {
                this.orderSize = value;
            }
        }
        
    }
}
