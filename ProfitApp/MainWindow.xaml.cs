using Microsoft.Win32;
using ProfitLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProfitApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new MainViewModel();
            DataContext = vm;
        }

        private void SearchFolders_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                vm.AmazonFileLocation = fileDialog.FileName;
            }
        }

        private void SearchItemFolders_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                vm.ItemListLocation = fileDialog.FileName;
            }
        }

        private void UploadReport_Click(object sender, RoutedEventArgs e)
        {
            vm.GetReport();
            ResetDataGrid();
        }

        private void AutoCreateItems_Click(object sender, RoutedEventArgs e)
        {
            vm.AutoCreateItems();                    
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
           
            ReportsGrid.Visibility = Visibility.Visible;
            ItemsGrid.Visibility = Visibility.Collapsed;
        }

        private void Items_Click(object sender, RoutedEventArgs e)
        {
            ResetDataGrid();
            ReportsGrid.Visibility = Visibility.Collapsed;
            ItemsGrid.Visibility = Visibility.Visible;

        }

        private void ResetDataGrid()
        {
            ItemListDataGrid.ItemsSource = null;
            ItemListDataGrid.ItemsSource = vm.ItemList;
            dataGridProfit.ItemsSource = null;
            dataGridProfit.ItemsSource = vm.OrderItems;
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if(e.Cancel)
            {
                return;
            }
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var item = e.Row.Item as Item;
                var textbox = e.EditingElement as TextBox;
               vm.EditItemListItem(item, e.Column.Header.ToString(), textbox.Text);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    vm.SaveItemListToFile(myStream);
                    myStream.Close();
                }
            }
           
        }

        private void GetItem_Click(object sender, RoutedEventArgs e)
        {
            vm.GetItemListFromFile();
            ResetDataGrid();
        }

        private void AutoAssign_Click(object sender, RoutedEventArgs e)
        {
            vm.AutoAssign();
            ResetDataGrid();
        } 
    }
}
