using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SE3352Assignment2
{
    public partial class StockByOrder : Form, Model_StockMarketDisplay
    {
        // this class is an observer !!!

        // this class will know which company it is an observer of. the company will be sent to the constructor at instantiation
        private Model_Company company;                          // use a private member variable to store the company associated with this observer

        // company is passed to the observer
        public StockByOrder(Model_Company c)
        {
            InitializeComponent();
            this.company = c;
       }

        public void Update(Model_RealTimeData subject) 
        {
            // this update must reset all the values in the "market depth by order" window!!
            /* The market depth by order window shows:
             *          - top 10 highest BUY prices, and the amount of shares for each
             *          - top 10 loweset SELL prices, and the amount of shares for each
             * */

            // first thing we should do is clear the view of all its data so we can reload new stuff
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Rows.Add(9);

            // first, search through the list of companies to find the company that corresponds to this observer
            foreach (Model_Company comp in subject.listOfCompanies)
            {
                // once we find the correct company
                if (this.company.companyName == comp.companyName)
                {
                    /* Create a temporary list of BuyOrders that orders the elements (using OrderBy) first by the order's price, and then (using ThenBy) by the order's
                     * DateTime. Then use reverse to get the list to be the top 10 prices */ 
                    List<Model_BuyOrder> BUYtemp = new List<Model_BuyOrder>(comp.buyOrdersList.OrderByDescending<Model_BuyOrder, Double>(buy => buy.OrderPrice).ThenBy<Model_BuyOrder, DateTime>(date => date.OrderDateTime));

                    /* Create a temporary list of SellOrders that orders the elements (using OrderBy) first by the order's price, and then (using ThenBy) by the order's
                     * Date Time. We don't need to use reverse here because we want the lowest sells */ 
                    List<Model_SellOrder> SELLtemp = new List<Model_SellOrder>(comp.sellOrdersList.OrderBy<Model_SellOrder, Double>(sell => sell.OrderPrice).ThenBy<Model_SellOrder, DateTime>(date => date.OrderDateTime));

                    // Iterate through the temporary list of buys and print the values of the top 10 highest bids 
                    for (int i = 0; i < BUYtemp.Count ; i++)
                    {
                        //this.dataGridView1.Rows.Add(1);
                        if (i == 10) break;     // break after 10 because we only want the top 10
                        this.dataGridView1.Rows[i].Cells[0].Value = BUYtemp.ElementAt(i).OrderPrice;        // this cell refers to the Buy Price column in the data grid view
                        this.dataGridView1.Rows[i].Cells[1].Value = BUYtemp.ElementAt(i).OrderSize;         // this cell refers to the Buy Volume column in the data grid view
                    }

                    // Next, iterate through all the sells and print the values of the top 10 lowest bids
                    for (int i = 0; i < SELLtemp.Count; i++)
                    {
                        if (i == 10) break;     // again, break after 10 because we only want the top 10
                        this.dataGridView1.Rows[i].Cells[2].Value = SELLtemp.ElementAt(i).OrderPrice;       // this cell refers to the Sell Price column in the data grid view
                        this.dataGridView1.Rows[i].Cells[3].Value = SELLtemp.ElementAt(i).OrderSize;        // this cell refers to the Sell Volume column in the data grid view
                    }   
                }
            }

            // end the update method for the StockByOrder observer with an accurate representation of the top 10 highest bids and lowest sells
        }

        // Call load to get the form
        private void StockByOrder_Load(object sender, EventArgs e)
        {
           
        }
    }
}
