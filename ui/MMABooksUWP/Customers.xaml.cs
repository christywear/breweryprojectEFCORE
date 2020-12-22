using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

using MMABooksUWP.Models;
using MMABooksUWP.Services;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using System.Net.Http;

namespace MMABooksUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Customers : Page  //making a page
    {
        // for easy navigation:)
        Frame frame = Window.Current.Content as Frame;
        //nulling obj Customer
        private Customer selected = null;
        // creating an HttpDataService obj named service
        private HttpDataService service;

        //For the current customer this is selected.
        public Customer Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        //making a collection I suspect it's like a list .. or accessing the model.
        public ObservableCollection<State> States { get; private set; } = new ObservableCollection<State>();

        public Customers()
        {
            this.InitializeComponent();
        }

            //loads after page is done loading.
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // instantiates service, and defines default url
            service = new HttpDataService("http://localhost:5000/api");
            // creates a new list for states, and requests them, and puts them in it.
            List<State> states = await service.GetAsync<List<State>>("states");
            foreach (State s in states)
                this.States.Add(s);
            // clears default customer details on the form.
            ClearCustomerDetails();
            // disables the fields
            EnableFields(false);
            // disables buttons
            EnableButtons("pageLoad");
        }

        //Search string method .. finds information (record requested from db)
        private async void findBtn_Click(object sender, RoutedEventArgs e)
        {
            // instantiates a new string type customerID and obtains data from the field Textbox customerIdTxt
            string customerId = this.customerIdTxt.Text;
            // attempting to obtain information via the service. http query for customers.
            try 
            {
                //sets selected, the form of which one is selected. via the customer ID. requests from server
                Selected = await service.GetAsync<Customer>("customers\\" + customerId, null, true);
                //displays customer details
                DisplayCustomerDetails();
                //if customer is found, it will disable save button, and cancel button. Enable rest.
                EnableButtons("found");

            }
            catch   // oops .. didnt find it.
            {
                // write a message for a dialog. that says this .. 
                var messageDialog = new MessageDialog("A customer with that customer id cannot be found.");
                // await user to click something / see it
                await messageDialog.ShowAsync();
                // sets current user info in the form selected to null.
                Selected = null;
                // clears all customer details inside the form.
                ClearCustomerDetails();
            }
        }

        //method deletes a record.
        private async void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            //check to see if the selected form is null, not present or not fileld out.
            if (Selected != null)
            {
                // sets the customerid to the selected customer id. resolves to what's in the form's txt field.
                int customerId = Selected.CustomerId;
                // wait for delete to get result from server.. via url/api/customerid
                if (await service.DeleteAsync("customers\\" + customerId))
                {
                    // clears the form. via nulling the form obj
                    Selected = null;
                    //removes the customerid field seems a little redundant (shouldn't this go in clear?) but ok.
                    this.customerIdTxt.Text = "";
                    // clears customer details all fields.
                    ClearCustomerDetails();
                    // disables things while page reloads.
                    EnableButtons("pageLoad");
                    // creates a new dialog for the user.
                    var messageDialog = new MessageDialog("Customer was deleted.");
                    //wait for user to respond.
                    await messageDialog.ShowAsync();
                }
                else
                {
                    // If this process failed. display this message.
                    var messageDialog = new MessageDialog("There was a problem deleting that customer.");
                    // wait until user notices
                    await messageDialog.ShowAsync();
                }
            }
        }

        //method for editing a record.
        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            //Are you still reading these? 
            //disables customerid field in the form because .. why make things blow up, when you can just not change it.
            this.customerIdTxt.IsEnabled = false;
            //enable the rest of the fields for more chaos
            EnableFields(true);
            // disables pretty much all the shiny buttons, except find and add
            EnableButtons("editing");
        }

        // this method adds a record
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            //nulling the obj again.. yaaay?
            Selected = null;
            //clearing id field.. 
            this.customerIdTxt.Text = "";
            //nope no clickly no use the id field. 
            this.customerIdTxt.IsEnabled = false;
            // clear all the things!
            ClearCustomerDetails();
            // enable all the things!
            EnableFields(true);
            // enable find, and add.. The rest has been redacted.
            EnableButtons("adding");
        }

        // Save, .. always save before fighting the boss.. I mean erm. send the changes to the server plz.
        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            //check if the obj is null.. because we don't want to update the database with information from a non existant source
            if (Selected == null)
            {
                // make a new customer obj!
                Customer newCustomer = new Customer();
                // sET all THE fields EQUAL to THE textBOXes 
                // Are you one of these people who hear what they read in their head?:P
                newCustomer.Name = this.customerNameTxt.Text;
                newCustomer.Address = this.customerAddressTxt.Text;
                newCustomer.City = this.customerCityTxt.Text;
                newCustomer.ZipCode = this.customerZipcodeTxt.Text;
                // state, .. state.. stake.. steak mmmm steak.. Sets a new state object equal to the drop down box.
                State selectedState = (State)this.customerStateCBox.SelectedItem;
                // plops selected state into statecode.
                newCustomer.StateCode = selectedState.StateCode;
                // Makes a brand new shiny response message, .. After these messages. and when done waiting, sent to server.
                // get response
                HttpResponseMessage response = await service.PostAsJsonAsync<Customer>("customers", newCustomer, true);
                // get ready to celebrate!
                if (response.IsSuccessStatusCode) 
                {
                    // the customer id is at the end of response.Headers.Location.AbsolutePath
                    string url = response.Headers.Location.AbsolutePath;
                    // sets index / , Or .. finds the index of the last slash in the url.
                    int index = url.LastIndexOf("/");
                    // uses that index adds one.. then prepares to add id.
                    string customerId = url.Substring(index + 1);
                    // parses the number from the id field.. puts it into a new customer obj
                    newCustomer.CustomerId = int.Parse(customerId);
                    // says ohh hey our form obj? yeah .. that one.. it's now a new customer.
                    Selected = newCustomer;
                    // gimme the id
                    this.customerIdTxt.Text = Selected.CustomerId.ToString();
                    // enable the field for my new id.
                    this.customerIdTxt.IsEnabled = true;
                    // spit out all the things to the display.
                    DisplayCustomerDetails();
                    // why don't they call this enable findadd or such.. 
                    EnableButtons("found");
                    // annoys the user with another dialog.
                    var messageDialog = new MessageDialog("Customer was added.");
                    //waits for user to care
                    await messageDialog.ShowAsync();
                }
                else
                {
                    // PANIC all the things didnt do the things!
                    var messageDialog = new MessageDialog("There was a problem adding that customer.");
                    //wait for user to panic and call support after cursing this hard work.
                    await messageDialog.ShowAsync();
                }
            }
            // editing
            else
            {    
                //well where here update time.. is this v3? Can't remember anymore.
                // make new customer obj
                Customer updatedCustomer = new Customer();
                // set customer obj fields equal to fields.
                updatedCustomer.CustomerId = Selected.CustomerId;
                updatedCustomer.Name = this.customerNameTxt.Text;
                updatedCustomer.Address = this.customerAddressTxt.Text;
                updatedCustomer.City = this.customerCityTxt.Text;
                updatedCustomer.ZipCode = this.customerZipcodeTxt.Text;
                State selectedState = (State)this.customerStateCBox.SelectedItem;
                updatedCustomer.StateCode = selectedState.StateCode;
                //If .. WAIT!!! ohh I mean it's waiting for sesrvice to get back with the info from update.
                if (await service.PutAsJsonAsync<Customer>("customers\\" + updatedCustomer.CustomerId, updatedCustomer))
                {
                    // hah.. I swear we have done this one before.. setting selected form/record = to our obj we created
                    Selected = updatedCustomer;
                    // spit out all the coff.. I mean record info
                    DisplayCustomerDetails();
                    // enable the id field. so we can put things in it. or display it.
                    this.customerIdTxt.IsEnabled = true;
                    // betcha it's addandfind again.. yep..
                    EnableButtons("found");
                    //makes YetAnotherDialog(tm) Spam(c)2020
                    var messageDialog = new MessageDialog("Customer was updated.");
                    // waits for the idle grunt from disgruntled employee..
                    await messageDialog.ShowAsync();
                }
                else
                {
                    // makes a new dialog... heh never get tired of these do we?
                    var messageDialog = new MessageDialog("There was a problem updating that customer.");
                    // waits for user to pretend to read it.
                    await messageDialog.ShowAsync();
                }
            }
        }

        // Cancel..???.. what.. are we canceling? wouldn't that imply .. we are doing something to cancel?
        // I mean erm.. this is the cancel method, we are prob just clearing things.
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            // adding
            if (Selected == null)
            {
                // enables the id field.
                this.customerIdTxt.IsEnabled = true;
                // clearing I knew it!
                ClearCustomerDetails();
                // disables the fields 
                EnableFields(false);
                // and buttons...
                EnableButtons("pageLoad");
            }
            // editing
            else
            {
                // failed.! .. how do we fail canceling the nothing we was doing before? Dunno yet.. lets find out.
                // displays all the things(record)
                DisplayCustomerDetails();
                //enables the id field
                this.customerIdTxt.IsEnabled = true;
                // yay record found.. enables.. wait.. we.. wern't finding... anything?
                // I mean yeah.. enabling findandadd
                EnableButtons("found");
            }

        }

        //Woo its time we are finally here! lets display ALL THE THINGS!!
        private void DisplayCustomerDetails()
        {
            // well.. ok then, if you look to the left you'll see a rare site indeed.. 
            // This method atm is dumping all the things from selected obj, to each field.
            //honestly a bit suprised we didnt check for null again, or clear fields first..
            //then set page load.. then add and find.. then do this.. then renable things.
            this.customerNameTxt.Text = Selected.Name;
            this.customerAddressTxt.Text = Selected.Address;
            this.customerCityTxt.Text = Selected.City;
            this.customerZipcodeTxt.Text = Selected.ZipCode;
            int stateIndex = this.FindStateIndex(Selected.StateCode);
            this.customerStateCBox.SelectedIndex = stateIndex;
            //this is that thing I was talking about earlier..  disable the things.
            EnableFields(false);
        }

        // Wooosh clean slate! Your free to go past Start and collect.. err wrong app.
        private void ClearCustomerDetails()
        {
            // clears all the fields , except id for some reason.
            this.customerNameTxt.Text = "";
            this.customerAddressTxt.Text = "";
            this.customerCityTxt.Text = "";
            this.customerZipcodeTxt.Text = "";
            this.customerStateCBox.SelectedIndex = -1;
            //and disables the fields how nice.
            EnableFields(false);
        }

        //wish I could find my mental state index. I mean uhh Who said that?
        // this thing finds your state from the state code, and tells ya what state your in.
        // my guess your in "Ore" <3
        private int FindStateIndex(string stateCode)
        {
            // declares index.
            int index = 0;
            // .. erm.. I guess, .. we like O(n) .. ;p .. it's iterating through each one looking for your statecode.
            foreach (State s in this.States)
            {
                if (s.StateCode == stateCode)
                    return index;
                index++;
            }
            //if your here, something went wrong.. send back -1.
            return -1;
        }

        // this one upd.. yeah if you can read this.. lol nvm.. i'll be good.
        // Updates customer details.
        private void UpdateCustomerDetails()
        {
            // sets the selected obj which looks very much like a customer obj's fields to all of the form's field boxes.
            Selected.Name = this.customerNameTxt.Text;
            Selected.Address = this.customerAddressTxt.Text;
            Selected.City = this.customerCityTxt.Text;
            Selected.ZipCode = this.customerZipcodeTxt.Text;
            State selectedState = (State)this.customerStateCBox.SelectedItem;
            Selected.StateCode = selectedState.StateCode;
            // .. then does .. nothing with them?
        }

        //Woooo this one allows the user to enter data again. .. except .. y'know that pesky id find thing.
        private void EnableFields(bool enabled = true)
        {
            this.customerNameTxt.IsEnabled = enabled;
            this.customerAddressTxt.IsEnabled = enabled;
            this.customerCityTxt.IsEnabled = enabled;
            this.customerZipcodeTxt.IsEnabled = enabled;
            this.customerStateCBox.IsEnabled = enabled;
        }

        // this one enables all the shiny buttons ... well ok not really.. it enables find/add mostly.. sometimes 
        // it gives delete and edit too though.
        private void EnableButtons(string state)
        {
            switch (state)
            {
                case "pageLoad":
                    this.deleteBtn.IsEnabled = false;
                    this.editBtn.IsEnabled = false;
                    this.saveBtn.IsEnabled = false;
                    this.cancelBtn.IsEnabled = false;
                    this.findBtn.IsEnabled = true;
                    this.addBtn.IsEnabled = true;
                    break;
                case "editing": case "adding":
                    this.deleteBtn.IsEnabled = false;
                    this.editBtn.IsEnabled = false;
                    this.addBtn.IsEnabled = false;
                    this.findBtn.IsEnabled = false;
                    this.saveBtn.IsEnabled = true;
                    this.cancelBtn.IsEnabled = true;
                    break;
                case "found":
                    this.saveBtn.IsEnabled = false;
                    this.cancelBtn.IsEnabled = false;
                    this.deleteBtn.IsEnabled = true;
                    this.editBtn.IsEnabled = true;
                    this.addBtn.IsEnabled = true;
                    this.findBtn.IsEnabled = true;
                    break;
            }

        }
        private void ReturnToMain(object sender, RoutedEventArgs e)
        {
            if (frame == null)
            {
                frame = new Frame();
            }
            this.frame.Navigate(typeof(MainPage));
        }
    }
}
