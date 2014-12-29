using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE3352Assignment2
{
    // the following interface defines the structure for the realTimeData concrete subject
    public interface Model_StockMarket
    {
        void Register(Model_StockMarketDisplay obser);          // observers must have the ability to register to the subject
        void Unregister(Model_StockMarketDisplay obser);        // observers must have the ability to unregister from the subject
        void Notify();                                          // realTimeDate should be able to notify the observers when there is information they must update
    }
}
