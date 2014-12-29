using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE3352Assignment2
{
    // the following class is extremely important for linking the observer and subject to complete the observer pattern
    public class Model_RealTimeData
    {
        //make a list for the companies in the application
        public List<Model_Company> listOfCompanies = new List<Model_Company>();

        //make a list for the observers in the application
        public List<Model_StockMarketDisplay> listOfObservers = new List<Model_StockMarketDisplay>();

        public Model_RealTimeData ()
        { 
            // in the constructor, hard code the values of the three companies with their starting info
            this.listOfCompanies.Add(new Model_Company("Microsoft Corporation", "MSFT", 46.13));
            this.listOfCompanies.Add(new Model_Company("Apple Inc.", "APPL", 105.22));
            this.listOfCompanies.Add(new Model_Company("Facebook Inc.", "FB", 80.67));
        }

        // the register method allows for an observer to register to the realTimeData so that they can implement the observer pattern and update accordingly
        public void Register(Model_StockMarketDisplay observer)
        {
            // add the observer to the list of observers
            this.listOfObservers.Add(observer);         
            observer.Update(this);                      // call update to ensure everything is up to date
        }

        public void Unregister(Model_StockMarketDisplay observer)
        {
            // remove the observer from list of observers
            this.listOfObservers.Remove(observer);                       
        }

        public void Notify()
        {
            // iterate through each observer and call update upon them 
            // send the real time data to the update function to change all the values
            foreach (Model_StockMarketDisplay obs in this.listOfObservers)
            {
                obs.Update(this);
            }
        }
    }
}
