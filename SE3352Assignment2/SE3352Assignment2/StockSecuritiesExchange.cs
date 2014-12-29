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
    public partial class StockSecuritiesExchange : Form
    {
        // the following class defines the actions for the MDI parent that will store all the displays for the forms and how to get to these options
        private Model_RealTimeData realTimeData; 

        public StockSecuritiesExchange()
        {
            InitializeComponent();

            // create the intial instance of real time data, this will be passed to many other classes for use
            realTimeData = new Model_RealTimeData();
        }

        // this tool strip menu relates to the "Market" option on the first tool bar
        private void helloToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        // this tool strip menu relates to the "Window" option on the first tool bar
        private void windowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        // this tool strip menu relates to the "Watch" option on the first tool bar
        private void watchToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        // this tool strip menu relates to the "Order" option on the first tool bar
        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        // window load; when the window initially loads
        private void StockSecuritiesExchange_Load(object sender, EventArgs e)
        {
            helloToolStripMenuItem.Text = "Market <<Closed>>";      // set the text of the first option to be "Market <<Closed>>"

            // iterate through each company and populate the options for the Market By Order and Market By Price options
            foreach (Model_Company comp in this.realTimeData.listOfCompanies)
            {
                // we must create the options dynamically based on the companies that exist in the application
                ToolStripMenuItem orderMenuItem = new ToolStripMenuItem();                  // create a new tool strip menu item 
                orderMenuItem.Text = comp.companyName;                                      // set the text to be the company name (so you know what company youre selecting an observer for)
                orderMenuItem.Click += new EventHandler(this.orderClick);                   // create an event handler for this option
                this.marketByOrderToolStripMenuItem.DropDownItems.Add(orderMenuItem);       // add the menu item to the appropriate parent menu item

                ToolStripMenuItem priceMenuItem = new ToolStripMenuItem();                  // create a new tool strip menu item 
                priceMenuItem.Text = comp.companyName;                                      // set the text to be the company name (so you know what company youre selecting an observer for)
                priceMenuItem.Click += new EventHandler(this.priceClick);                   // create an event handler for this option
                this.marketByPriceToolStripMenuItem.DropDownItems.Add(priceMenuItem);       // add the menu item to the appropriate parent menu item
            }
        }

        // this is the event handler for when the user clicks on one of the dynamically created menu options under Market By Price
        private void priceClick(object sender, EventArgs e)
        {
            // this is called when one of the options in the market by price menu is clicked
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            // iterate through the companies to check which one they actually clicked on
            foreach (Model_Company comp in this.realTimeData.listOfCompanies)
            {
                // create a new market by price form based on the company selected
                if (comp.companyName == item.Text)
                {
                    StockByPrice MBPobserver = new StockByPrice(comp);                      // create a stock by price observer for the specified company
                    MBPobserver.MdiParent = this;                                           // specify the parent to be the mdi window
                    MBPobserver.Text = "Market Depth by Price for " + comp.companyName;     // specify the title for the observer form
                    MBPobserver.Show();                                                     // reveal this observer
                    
                    //** register the market by price observer with the realTimeData!!
                    this.realTimeData.Register(MBPobserver);
                }
            }
        }

        // this is the event handler for when the user clicks on one of the dynamically created menu options under Market By Order
        private void orderClick(object sender, EventArgs e)
        {
            // this is called when one of the options in the market by order menu is clicked
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            // iterate through the companies to check which one they actually clicked on
            foreach (Model_Company comp in this.realTimeData.listOfCompanies)
            {
                // create a new market by order form based on the company selected
                if (comp.companyName == item.Text)
                {
                    StockByOrder MBOobserver = new StockByOrder(comp);                      // create a stock by order observer for the specified company
                    MBOobserver.MdiParent = this;                                           // specify the parent to be the mdi window
                    MBOobserver.Text = "Market Depth by Order for " + comp.companyName;     // specify the title for the observer form
                    MBOobserver.Show();                                                     // reveal this observer
                    
                    // register the "market by order" observer 
                    this.realTimeData.Register(MBOobserver);
                }
            }
        }

        // market-> end trading
        private void endTradingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // when we end the trading
            endTradingToolStripMenuItem.Enabled = false;            // disable the end trading option 
            beginTradingToolStripMenuItem.Enabled = true;           // enable the begin trading option 
            helloToolStripMenuItem.Text = "Market <<Closed>>";      // set the appropriate text
            watchToolStripMenuItem.Visible = false;                 // hide the watch tools bar
            ordersToolStripMenuItem.Visible = false;                // hide the orders tools bar
        }

        // market-> begin trading
        private void beginTradingToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // when we begin the trading
            endTradingToolStripMenuItem.Enabled = true;             // enable the end trading option
            beginTradingToolStripMenuItem.Enabled = false;          // disable the begin trading option 
            helloToolStripMenuItem.Text = "Market <<Open>>";        // set the appropriate text
            watchToolStripMenuItem.Visible = true;                  // show the watch tools bar
            ordersToolStripMenuItem.Visible = true;                 // show the orders tools bar

        }

        //window-> cascade
        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);  // order all the open windows inside this window as a cascaded layout
        }

        //window-> horizontal tile
        private void horizontalTileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);  // order all the open windows inside this window as a horizontal tiled layout
        }

        //window-> vertical tile
        private void verticalTileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);     // order all the open windows inside this window as a vertical tiled layout
        }

        //watch -> stock stats summary
        private void stockStatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // create a new StockStats observer when the corresponding option is selected from the watch options
            StockStatsSummary summaryObserver = new StockStatsSummary();
            summaryObserver.MdiParent = this;                               // set the parent window to be this MDI window
            summaryObserver.Show();                                         // show the observer

            // ** register the observer with realTimeData so it can update accordingly!
            this.realTimeData.Register(summaryObserver);            
        }

        //---------------------------------------------------------------
        //orders -> bid
        private void bidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // send realTimeData to the bid order form for it to be created
            BidOrderForm bid = new BidOrderForm(this.realTimeData);
            bid.MdiParent = this;                   // set this window to be the MDI parent
            bid.Text = "Place Bid Order Form";      // set the text on the form
            bid.Show();                             // show the form
        }

        //orders -> sell
        private void sellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // send realTimeData to the sell order form for it to be created
            SellOrderForm sell = new SellOrderForm(this.realTimeData);
            sell.MdiParent = this;                   // set this window to be the MDI parent
            sell.Text = "Place Sell Order Form";     // set the text on the form
            sell.Show();                             // show the form
        }

        // the exit option should close the entire application
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}
