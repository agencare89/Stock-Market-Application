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
    public partial class BidOrderForm : Form
    {
        // this class will need a real time data variable in order to store the companies to select from 
        private Model_RealTimeData realTimeData;

        public BidOrderForm(Model_RealTimeData RTD)
        {
            // create the windows form for placing a bid (Bid Order Form)
            InitializeComponent();       

            // accept the real time data into the bid form
            this.realTimeData = RTD;
        }

        // the following method will be called when the user clicks the button labeled "Submit" on this form
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

            // if the TryParse method returns false for either entry:
            if (!number1 || !number2)
            {
                MessageBox.Show("You have entered an invalid value ", "Invalid Input");
                textBox1.Clear();
                textBox2.Clear();
            }

            // if the TryParse returns true; both inputs are valid so we can proceed
            else
            {   // officially conver the two numbers into double type variables
                Double numShares = Convert.ToDouble(this.textBox1.Text);
                Double bidPrice = Convert.ToDouble(this.textBox2.Text);

                // using the realTimeData that is stored in this form, look through the list of companies for the company name that was selected in the combobox
                foreach (Model_Company comp in this.realTimeData.listOfCompanies)
                {
                    // if we find a match in the combo box 
                    // Note: we should ALWAYS find a match seeing as the combo box is loaded from the companies from the same realTimeData variable
                    if (comp.companyName == comboBox1.Text)
                    {
                        /* Once we find a match, we must perform the necessary steps to update the appropriate observers 
                         * This includes, if there is a sale that can be made to make it. If there is not a sale to be made, 
                         * the requested bid should be sent to the pending list of bid orders, which is stored in this specific company. */    

                        // Make a temporary copy of the list of all sell orders; this will be used to compared the buy order with to check for a sale
                        List<Model_SellOrder> tempSells = new List<Model_SellOrder>(comp.sellOrdersList);        

                        // Initialize a new list that will store a list of potential sales. Potential sales will occur if the sale price is less or equal to the bid price
                        List<Model_SellOrder> potentialSales = new List<Model_SellOrder>();                       

                        // Iterate through the temporary sells list to check each item in the list for a match.
                        foreach (Model_SellOrder sell in tempSells) 
                        {
                            // if the price is correct for a sale, this pending sell order should be added to the list of potential sales
                            if (sell.OrderPrice <= bidPrice) potentialSales.Add(sell);
                            else continue;
                        }

                        // create a copy of the number of shares this bid order is being submitted for
                        Double totalSharesToSell = numShares;

                        // if there are potential sales that can be made, the length of the list of potential sales will be > 0
                        if (potentialSales.Count > 0)
                        {
                            // use the OrderBy method on the list of potential sales to sort the potential sales by their price. This will allow for the 
                            // correct potential sale to be executed 
                            potentialSales.OrderBy<Model_SellOrder, Double>(p => p.OrderPrice);
                            
                            // use a double variable to store the total amount of shares sold. this will be reflected in the company's volume in stock state summary
                            Double totalSharesSOLD = 0.0;
                            
                            // use a double variable to store the sale price of the last sale that occured. this will be reflected in the company's last sale in stock state summary
                            Double finalSalePrice = 0.0;
                            
                            // iterate through the potential sales to make the appropriate sales 
                            foreach (Model_SellOrder potentialSale in potentialSales)
                            {
                                /*if the order size of the sale is less than the total amount of shares there are to buy, then it will not exhaust all of the 
                                 * shares that there are to buy. Therefore, we should sell off these shares (subtract from total shares), adjust the total shares
                                 * that have been sold, and remove the potential by from the pending list */ 

                                if (potentialSale.OrderSize <= totalSharesToSell)
                                {
                                    totalSharesToSell = totalSharesToSell - potentialSale.OrderSize;    // adjust the number of shares that can be sold
                                    totalSharesSOLD += potentialSale.OrderSize;                         // add the sold sales to the shares that have been sold
                                    comp.sellOrdersList.Remove(potentialSale);                          // the sale gets fully executed so remove it from the pending list
                                    if (totalSharesToSell == 0) finalSalePrice = potentialSale.OrderPrice;
                                }

                                /* if the order size is greater than the total amount of shares, then the total amount of available shares is going to be exhausted. This means that
                                 * our bid request will then carry over some, meaning we need to adjust the size of our request and still add it to the pending list */ 

                                else if (potentialSale.OrderSize > totalSharesToSell)
                                {
                                    totalSharesSOLD += totalSharesToSell;                               // add the sold shares to the previous volume
                                    
                                    Double temp = potentialSale.OrderSize - totalSharesToSell;          // store the new order size value of our bid order
                                    Double temp2 = potentialSale.OrderPrice;                            // store the order price 

                                    comp.sellOrdersList.Remove(potentialSale);                          // remove the old potential sale so we can replace it with a new, adjusted one
                                    comp.sellOrdersList.Add(new Model_SellOrder(temp, temp2));          // add the adjusted request to our list of pending bid orders

                                    finalSalePrice = temp2;                                             // set the final sale price for the stock state summary form
                                    totalSharesToSell = 0;                                              // set the shares remaining to 0 for future purposes
                                }
                            
                                else continue;
                            }

                            comp.LastPrice = finalSalePrice;                                            // set the company's last sale price to the final sale price here
                            comp.Volume += totalSharesSOLD;                                             // set the company's total volume to be in addition to what was accumulated in these transactions
                        }

                        // if there are no potential sales that can be made, simply add the bid order to the company's personal list of pending bid orders
                        if (totalSharesToSell > 0)
                        {
                            comp.buyOrdersList.Add(new Model_BuyOrder(totalSharesToSell, bidPrice));
                        }
                    }
                }
                textBox1.Clear();
                textBox2.Clear();

                // at the end of this submit button click we call notify so that the observers can be told to update
                this.realTimeData.Notify();
            }
        }

        // cancel button; executed when the user clicks cancel button on this form
        private void button2_Click(object sender, EventArgs e)
        {
            base.Close();       // simply closes the form
        }

        // this method will be performed when the Orders->Bid option is clicked on
        private void BidOrderForm_Load(object sender, EventArgs e)
        {
            // when the form loads, load the company name information into the combo box
            foreach (Model_Company comp in this.realTimeData.listOfCompanies)
            {
                this.comboBox1.Items.Add(comp.companyName);
            }

            // make the first company the initial selected company 
            this.comboBox1.SelectedIndex = 0;
        }
    }
}
