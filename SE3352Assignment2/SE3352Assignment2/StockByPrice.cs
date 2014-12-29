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
    public partial class StockByPrice : Form, Model_StockMarketDisplay
    {
        // this class is an observer !!!

        // this class will know which company it is an observer of. the company will be sent to the constructor at instantiation
        private Model_Company company;                  // use a private member variable to store the company associated with this observer

        // company is passed to the observer
        public StockByPrice(Model_Company c)
        {
            InitializeComponent();
            this.company = c; 
        }

        public void Update(Model_RealTimeData subject)
        {
            // this update must rest all the values in the market depth by price window!!
            /* The market depth by price window shows: 
             *          - the top 10 GROUPS of bids (i.e if i have 3 bids of 40, group them all
             *              together) 
             *          - the total volume of the group
             *          - the total number of bids that make up that volume 
             *          - same for sells, but sorted by lowest 10 values
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
                    // First, create a temporay copy of the bid orders list so that we can group them by price 
                    List<Model_BuyOrder> tempGroupedOrders = new List<Model_BuyOrder>(comp.buyOrdersList);

                    /* Use an IEnumerable type object with a template of IGrouping to create a list of groups, grouped by the order price Key. This operation
                     * also uses the order by method that orders the groups by their price (key) and reverses it to get the max orders */
                    IEnumerable<IGrouping<Double, Model_BuyOrder>> orders = tempGroupedOrders.GroupBy(buy => buy.OrderPrice).OrderBy(b => b.Key).Reverse();

                    // Create a temporary copy of the sell orders list so that we can group them by price
                    List<Model_SellOrder> tempGroupedSells = new List<Model_SellOrder>(comp.sellOrdersList);

                    /* Use an IEnumerable type object with a template of IGrouping to create a list of groups, grouped once again by the order price (becomes the Key). This
                     * also uses order by method to order the groups by their key. */ 
                    IEnumerable<IGrouping<Double, Model_SellOrder>> sells = tempGroupedSells.GroupBy(sell => sell.OrderPrice).OrderBy(a => a.Key);

                    int count = 0;  // counts which row we are adding to in the observer

                    // Iterate through all the groups in the orders list
                    foreach (IGrouping<Double, Model_BuyOrder> group in orders)
                    {
                        //this.dataGridView1.Rows.Add(1);
                        if (count == 10) break; // only show the top 10

                        this.dataGridView1.Rows[count].Cells[0].Value = group.Count();  // Count() returns the number of elements that make up the group 
                        this.dataGridView1.Rows[count].Cells[1].Value = group.Key;      // Key is the value that the group is grouped by (in this case, the order price)

                        Double totalVolume = 0.0;   // set a variable to keep track of the total volume in the group

                        // Iterate through each element of the grouping, adding up each element's volume to get the total volume of the group
                        foreach (Model_BuyOrder buy in group)
                        {
                            totalVolume += buy.OrderSize;
                        }
                        this.dataGridView1.Rows[count].Cells[2].Value = totalVolume;    // write the total volume to the observer

                        count++;    // go to the next row
                    }

                    int count2 = 0;     // another variable to count which row we are adding to in the observer

                    // Again, iterate throuh all the groups in the sells list
                    foreach (IGrouping<Double, Model_SellOrder> group in sells)
                    {
                        if (count2 == 10) break; // only show the top 10 

                        this.dataGridView1.Rows[count2].Cells[3].Value = group.Count();     // Count() returns the number of elements that make up the group
                        this.dataGridView1.Rows[count2].Cells[4].Value = group.Key;         // Key is the value that the group is grouped by (in this case, the order price)

                        Double totalVolume = 0.0;           // set a variable to keep track of the total volume in the group

                        // Iterate through each element of the grouping, adding up each element's volume to get the total volume of the group
                        foreach (Model_SellOrder sell in group)
                        {
                            totalVolume += sell.OrderSize; 
                        }
                        this.dataGridView1.Rows[count2].Cells[5].Value = totalVolume;   // write the total volume to the observer

                        count2++;
                    }

                }
            }

            /* End the update method with an observer that shows the top bid prices grouped by bid price with the total volume of each group and the 
             * number of elements that make up that group. The same information will the shown for sell prices */
        }

        // Call load to get the form
        private void StockByPrice_Load(object sender, EventArgs e)
        {

        }
    }
}
