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
    public partial class SellOrderForm : Form
    {
        // this class will need a real time data variable in order to store the companies to select from 
        private Model_RealTimeData realTimeData;

        public SellOrderForm(Model_RealTimeData RTD)
        {
            // create the windows form for placing a sell (Sell Order Form)
            InitializeComponent();

            // accept the real time data into the sell form
            this.realTimeData = RTD;    
        }

        // on load, we have to load the combo boxes for the sell order form with the list of companies
        private void SellOrderForm_Load(object sender, EventArgs e)
        {
            foreach (Model_Company comp in this.realTimeData.listOfCompanies)
            {
                this.comboBox1.Items.Add(comp.companyName);
            }

            this.comboBox1.SelectedIndex = 0;
        }

        // this is the cancel button, simply closes the window
        private void button2_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        // the following method will be called when the user clicks te button labeled "Submit" on the form
        private void button1_Click(object sender, EventArgs e)
        {
            // the following execution is to validate that the user has entered two double type values into the spaces on the form

            /* The validation is performed by attempting to parse the values that the user has entered. The method Double.TryParse 
             * will return true into the respective boolean variable if the parse was successful. Otherwise, it will return false 
             * and the user will be promted to enter new values */

            Double outputValue1 = 0, outputValue2 = 0;
            bool number1, number2;

            number1 = Double.TryParse(textBox1.Text, out outputValue1);
            number2 = Double.TryParse(textBox2.Text, out outputValue2);

            // if the TryParse method retusn false for either entry
            if (!number1 || !number2)
            {
                MessageBox.Show("You have entered an invalid value ", "Invalid Input");
                textBox1.Clear();
                textBox2.Clear();
            }
            
            // if the TryParse returns true; both inputs are valid so we can proceed
            else
            {
                // oficially convert the two numbers into double type variables 
                Double numShares = Convert.ToDouble(this.textBox1.Text);
                Double sellPrice = Convert.ToDouble(this.textBox2.Text);

                // using the real time data stored in this form (from constructor) look through the list of companies for the company name that was selected in the combobox
                foreach (Model_Company comp in this.realTimeData.listOfCompanies)
                {
                    // if we find a match in the combo box
                    // Note: we should always find a match seeing as the combo box is loaded from the companies from the same realTimeData variable
                    if (comp.companyName == comboBox1.Text)
                    {
                        /* Once we find a match, we must perform the necessary steps to update the appropriate ovservers. This includes, if there is
                         * a sale that can be made, to make a sale. If there is not a sale that can be made, the requested sell sould be sent to the 
                         * pending list of sell orders, which is stored in the specific company */

                        // Make a temporary copy of the list of all buy orders; this will be used to compare the sell order with to check for a sale
                        List<Model_BuyOrder> tempBuyList = new List<Model_BuyOrder>(comp.buyOrdersList);

                        // Initialize a new list that will store a list of potential sales. Potential sales will occur if the buy price is greater than or equal to a sell price
                        List<Model_BuyOrder> potentialBuys = new List<Model_BuyOrder>();

                        // Iterate through the temporary buys list to check each item in list for a potential buy match
                        foreach (Model_BuyOrder buy in tempBuyList)
                        {
                            // if the price is correct for a sale, this pending buy order should be added to the list of potential buys 
                            if (buy.OrderPrice >= sellPrice) potentialBuys.Add(buy);
                            else continue; 
                        }

                        // create a copy of the number of shares this sell order is being submitted for 
                        Double totalSharesToBuy = numShares;

                        // if there are potenital buys that can be made, the length of this list will be > 0
                        if (potentialBuys.Count > 0)
                        {
                            /// use the OrderBy method on the list of potential buys to sort the potential buys by their price. This will allow for the 
                            // correct potential buy to be executed 
                            potentialBuys.OrderBy<Model_BuyOrder, Double>(p => p.OrderPrice);
                            potentialBuys.Reverse();

                            // use a double variable to store the total amount of shares sold. this will be reflected in the company's volume in stock state summary
                            Double totalSharesBOUGHT = 0.0;

                            // use a double variable to store the sale price of the last sale that occured. this will be reflected in the company's last sale in stock state summary
                            Double finalBuyPrice = 0.0;

                            // iterate through the potential buys to make the appropriate sales 
                            foreach (Model_BuyOrder potentialBuy in potentialBuys)
                            {
                                /*if the order size of the buy is less than the total amount of shares there are to sell, then it will not exhaust all of the 
                                 * shares that there are to sell. Therefore, we should sell off these shares (subtract from total shares), adjust the total shares
                                 * that have been sold, and remove the potential by from the pending list */ 

                                if (potentialBuy.OrderSize <= totalSharesToBuy)
                                {
                                    totalSharesToBuy = totalSharesToBuy - potentialBuy.OrderSize;           // adjust the number of shares that can be sold
                                    totalSharesBOUGHT += potentialBuy.OrderSize;                            // add the sold bids to she shares that have been sold
                                    comp.buyOrdersList.Remove(potentialBuy);                                // the sale gets fully executed so remove it from the pending list

                                    finalBuyPrice = potentialBuy.OrderPrice;
                                    if (totalSharesToBuy == 0) finalBuyPrice = potentialBuy.OrderPrice;
                                }

                                /* if the sell order size is greater than the total amount of shares, then the total amount of available shares is going to be exhausted. This means that
                                 * our sell request will then carry over some, meaning we need to adjust the size of our request and still add it to the pending list */ 

                                else if (potentialBuy.OrderSize > totalSharesToBuy)
                                {
                                    totalSharesBOUGHT += totalSharesToBuy;                                  // add the sold shares to the previous volume
                                    
                                    Double temp = potentialBuy.OrderSize - totalSharesToBuy;                // store the new order size value of our sell order
                                    Double temp2 = potentialBuy.OrderPrice;                                 // store the order price

                                    comp.buyOrdersList.Remove(potentialBuy);                                // remove the old potential bid so we can replace it with a new adjusted order
                                    comp.buyOrdersList.Add(new Model_BuyOrder(temp, temp2));                // add the adjusted request to our list of pending sell orders

                                    finalBuyPrice = temp2;                                                  // set the final sale price for the stock state summary observer
                                    totalSharesToBuy = 0;                                                   // set the shares remaining to 0 for future purposes
                                }
                       
                                else continue;
                            }

                            comp.LastPrice = finalBuyPrice;                                                 // set the company's last sale price to the final sale price here
                            comp.Volume += totalSharesBOUGHT;                                               // set the company's total volume to be in addition to what was accumulated

                        }

                        // if there are no potential sales that can be made, simply add the bid order to the company's personal list of pending bid orders
                        if (totalSharesToBuy > 0)
                        {
                            comp.sellOrdersList.Add(new Model_SellOrder(totalSharesToBuy, sellPrice));
                        }
                    } 
                }

               
                textBox1.Clear();
                textBox2.Clear();

                // at the end of this submit button, we call notify so that the observers can be told to update
                this.realTimeData.Notify();
            }
        }
    }
}
