using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE3352Assignment2
{
    // the following interface defines the necessary structure for the observers
    public interface Model_StockMarketDisplay
    {
        // the observers must have an update method that can be invoked so that their data updates automatically
        void Update(Model_RealTimeData subject);
    }
}
