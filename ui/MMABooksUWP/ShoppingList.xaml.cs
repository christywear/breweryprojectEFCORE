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
using Microsoft.Toolkit.Uwp.UI.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
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
    public sealed partial class ShoppingList : Page
    {
       
        // for easy navigation:)
        Frame frame = Window.Current.Content as Frame;
        //nulling obj Customer
        private IngredientInventoryAddition selected = null;
        // creating an HttpDataService obj named service
        private HttpDataService service;

        //For the current customer this is selected.
        public IngredientInventoryAddition Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        //making a collection I suspect it's like a list .. or accessing the model.
        public ObservableCollection<IngredientInventoryAddition> IngredientsObs { get; private set; } = new ObservableCollection<IngredientInventoryAddition>();

        public ShoppingList()
        {
            this.InitializeComponent();
            
        }

        //loads after page is done loading.
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // instantiates service, and defines default url
            service = new HttpDataService("http://localhost:5000/api");
            // creates a new list for states, and requests them, and puts them in it.
            List<IngredientInventoryAddition> IngredientsAdding = await service.GetAsync<List<IngredientInventoryAddition>>("IngredientInventoryAdditions");
            foreach (IngredientInventoryAddition I in IngredientsAdding)
                this.IngredientsObs.Add(I);
            FillDataBox();
        }

        private void HomeNavButton_Click(object sender, RoutedEventArgs e)
        {
            if (frame == null)
            {
                frame = new Frame();
            }
            this.frame.Navigate(typeof(MainPage));
        }



        private void searchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchForThisRecipe_txt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FillDataBox()
        {
            DataGrid DG = new DataGrid();
            System.Data.DataTable DT = new System.Data.DataTable();
            DT.Columns.Clear();
  
            //
            //DT.Columns.Add(IngredientsObs.ToDictionary(IngredientsObs).

            //DG.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            //for(int i = 0; i < )

        }
    }
}
