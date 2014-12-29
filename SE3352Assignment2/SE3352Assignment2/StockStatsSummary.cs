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
    public partial class StockStatsSummary : Form, Model_StockMarketDisplay
    {
        public StockStatsSummary()
        {
            InitializeComponent();
        } 

        // window load
        private void StockStatsSummary_Load(object sender, EventArgs e)
        {

            Bitmap nochange = new Bitmap(@"C:\Users\Adam\Documents\Visual Studio 2013\Projects\SE3352Assignment2\SE3352Assignment2\Resources\noChange.bmp", true);
            this.dataGridView1.Rows.Add("Microsoft Corporation", "MSFT", "46.13", "0", "0", nochange, "0", "0");
            this.dataGridView1.Rows.Add("Apple Inc.", "AAPL", "105.22", "0", "0", nochange, "0", "0");
            this.dataGridView1.Rows.Add("Facebook Inc.", "FB", "80.67", "0", "0", nochange, "0", "0");

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        public void Update(Model_RealTimeData subject)
        {
            // this update must reset all the values in the Stocks Stats Summary view!!
            /*Values to be printed include: 
             *          - company name (always same)
             *          - company symbol (always same)
             *          - open price (i believe will always be the same)
             *          - last sale price 
             *          - net change
             *          - image (either up arrow, down arrow or even arrow) 
             *          - percent change 
             *          - volume of shares */

            
            // first thing we should do is clear all the rows so that we can reprint the new data
            this.dataGridView1.Rows.Clear();

            Bitmap up = new Bitmap(@"C:\Users\Adam\Documents\Visual Studio 2013\Projects\SE3352Assignment2\SE3352Assignment2\Resources\up.bmp", true);
            Bitmap down = new Bitmap(@"C:\Users\Adam\Documents\Visual Studio 2013\Projects\SE3352Assignment2\SE3352Assignment2\Resources\down.bmp", true);
            Bitmap nochange = new Bitmap(@"C:\Users\Adam\Documents\Visual Studio 2013\Projects\SE3352Assignment2\SE3352Assignment2\Resources\noChange.bmp", true);

            // then iterate through all the companies and print their data
            foreach (Model_Company comp in subject.listOfCompanies)
            {
                // compute the net change
                Bitmap temp;
                // if the net change is currently 0 (this applies to the first time we update)
                if (comp.LastPrice > 0) comp.netChange = comp.LastPrice - comp.openPrice;
                else comp.netChange = 0; 

                // determine which icon should be display 
                if (comp.netChange > 0)
                {
                    // display the green up arrow
                    temp = new Bitmap(up);
                }

                else if (comp.netChange < 0)
                {
                    // display the red down arrow
                    temp = new Bitmap(down);
                }

                else
                {
                    // display the no change arrow
                    temp = new Bitmap(nochange);
                }

                comp.netChange = Math.Round(comp.netChange, 2);
                comp.percentChange = Math.Round((comp.netChange / comp.openPrice * 100), 2);

                this.dataGridView1.Rows.Add(comp.companyName, comp.companyName, comp.openPrice, comp.LastPrice, comp.netChange,
                    temp, comp.percentChange, comp.Volume); 
                
            } 
        } 
    }
}
