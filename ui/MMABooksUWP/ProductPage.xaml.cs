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
    public sealed partial class ProductPage : Page
    {
        Frame frame = Window.Current.Content as Frame;

        private Products selected = null;
        private HttpDataService service;

        public ObservableCollection<Products> Products { get; private set; } = new ObservableCollection<Products>();
        public ProductPage()
        {
            this.InitializeComponent();
        }

        public Products Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            service = new HttpDataService("http://localhost:5000/api");
            List<Products> products = await service.GetAsync<List<Products>>("products");
            foreach (Products p in Products)
                this.Products.Add(p);
            ClearProductsDetails();
            EnableFields(false);
            EnableButtons("pageLoad");
        }

        private async void findBtn_Click(object sender, RoutedEventArgs e)
        {
            string ProductCode = this.ProductCodeTxt.Text;
            try
            {
                Selected = await service.GetAsync<Products>("Products\\" + ProductCode, null, true);
                DisplayProductDetails();
                EnableButtons("found");

            }
            catch
            {
                var messageDialog = new MessageDialog("A Product with that Product Code cannot be found.");
                await messageDialog.ShowAsync();
                Selected = null;
                ClearProductsDetails();
            }
        }

        private async void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Selected != null)
            {
                string ProductCode = Selected.ProductCode;
                if (await service.DeleteAsync("Products\\" + ProductCode))
                {
                    Selected = null;
                    this.ProductCodeTxt.Text = "";
                    ClearProductsDetails();
                    EnableButtons("pageLoad");
                    var messageDialog = new MessageDialog("Product was deleted.");
                    await messageDialog.ShowAsync();
                }
                else
                {
                    var messageDialog = new MessageDialog("There was a problem deleting that product.");
                    await messageDialog.ShowAsync();
                }
            }
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            this.ProductCodeTxt.IsEnabled = false;
            EnableFields(true);
            EnableButtons("editing");
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            Selected = null;
            this.ProductCodeTxt.IsEnabled = false;
            ClearProductsDetails();
            EnableFields(true);
            EnableButtons("adding");
        }

        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            // adding
            if (Selected == null)
            {
                Products newProduct = new Products();
                newProduct.ProductCode = this.ProductCodeTxt.Text;
                newProduct.Description = this.DescriptionTxt.Text;
                newProduct.UnitPrice = Decimal.Parse(this.UnitPriceTxt.Text);
                newProduct.OnHandQuantity = int.Parse(this.OnHandQuantityTxt.Text);
                HttpResponseMessage response = await service.PostAsJsonAsync<Products>("Products", newProduct, true);
                if (response.IsSuccessStatusCode)
                {
                    // the customer id is at the end of response.Headers.Location.AbsolutePath
                    string url = response.Headers.Location.AbsolutePath;
                    int index = url.LastIndexOf("/");
                    string ProductCode = url.Substring(index + 1);
                    newProduct.ProductCode = ProductCode;
                    Selected = newProduct;
                    this.ProductCodeTxt.Text = Selected.ProductCode.ToString();
                    this.ProductCodeTxt.IsEnabled = true;
                    DisplayProductDetails();
                    EnableButtons("found");
                    var messageDialog = new MessageDialog("Product was added.");
                    await messageDialog.ShowAsync();
                }
                else
                {
                    var messageDialog = new MessageDialog("There was a problem adding that Product.");
                    await messageDialog.ShowAsync();
                }
            }
            // editing
            else
            {
                Products updatedProduct = new Products();
                updatedProduct.ProductCode = Selected.ProductCode;
                updatedProduct.Description = this.DescriptionTxt.Text;
                updatedProduct.UnitPrice = Decimal.Parse(this.UnitPriceTxt.Text);
                updatedProduct.OnHandQuantity = int.Parse(this.OnHandQuantityTxt.Text);
                if (await service.PutAsJsonAsync<Products>("products\\" + updatedProduct.ProductCode, updatedProduct))
                {
                    Selected = updatedProduct;
                    DisplayProductDetails();
                    this.ProductCodeTxt.IsEnabled = true;
                    EnableButtons("found");
                    var messageDialog = new MessageDialog("Product was updated.");
                    await messageDialog.ShowAsync();
                }
                else
                {
                    var messageDialog = new MessageDialog("There was a problem updating that Product.");
                    await messageDialog.ShowAsync();
                }
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            // adding
            if (Selected == null)
            {
                this.ProductCodeTxt.IsEnabled = true;
                ClearProductsDetails();
                EnableFields(false);
                EnableButtons("pageLoad");
            }
            // editing
            else
            {
                DisplayProductDetails();
                this.ProductCodeTxt.IsEnabled = true;
                EnableButtons("found");
            }

        }

        private void DisplayProductDetails()
        {
            this.ProductCodeTxt.Text = Selected.ProductCode;
            this.DescriptionTxt.Text = Selected.Description;
            this.UnitPriceTxt.Text = Selected.UnitPrice.ToString();
            this.OnHandQuantityTxt.Text = Selected.OnHandQuantity.ToString();
            EnableFields(false);
        }

        private void ClearProductsDetails()
        {
            this.DescriptionTxt.Text = "";
            this.UnitPriceTxt.Text = "";
            this.OnHandQuantityTxt.Text = "";
            EnableFields(false);
        }


        private void UpdateProductDetails()
        {
            Selected.ProductCode = this.ProductCodeTxt.Text;
            Selected.Description = this.DescriptionTxt.Text;
            Selected.UnitPrice = Decimal.Parse(this.UnitPriceTxt.Text);
            Selected.OnHandQuantity = int.Parse(this.OnHandQuantityTxt.Text);
        }

        private void EnableFields(bool enabled = true)
        {
            
            this.DescriptionTxt.IsEnabled = enabled;
            this.UnitPriceTxt.IsEnabled = enabled;
            this.OnHandQuantityTxt.IsEnabled = enabled;
        }

        private void EnableButtons(string Products)
        {
            switch (Products)
            {
                case "pageLoad":
                    this.deleteBtn.IsEnabled = false;
                    this.editBtn.IsEnabled = false;
                    this.saveBtn.IsEnabled = false;
                    this.cancelBtn.IsEnabled = false;
                    this.findBtn.IsEnabled = true;
                    this.addBtn.IsEnabled = true;
                    break;
                case "editing":
                case "adding":
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
