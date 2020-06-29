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
        private string ebayAddress;
        public MainWindow()
        {
            InitializeComponent();
            ReportsGrid.Visibility = Visibility.Visible;
            ItemsGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Collapsed;
            ChartGrid.Visibility = Visibility.Collapsed;
            vm = new MainViewModel();
            SetupSettings();
            DataContext = vm;

        }

        private void SetupSettings()
        {
            var databasePath = Properties.Settings.Default.databasePath;
            var lastHitDate = Properties.Settings.Default.LastHitDateEbay;

            if(!string.IsNullOrWhiteSpace(databasePath))
            {
                vm.DatabaseLocation = databasePath;
                vm.OpenDatabase();
            }

            if(lastHitDate != DateTime.MinValue)
            {
                vm.LastHitDate = lastHitDate;
            }
        }

        private void ProfitSearchFolders_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                vm.ProfitFileLocation = fileDialog.FileName;
            }
        }

        private void DatabaseSearchFolders_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                vm.DatabaseLocation = fileDialog.FileName;
                Properties.Settings.Default.databasePath = fileDialog.FileName;
            }
        }

        private void SearchFolders_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                vm.PayPalFileLocation = fileDialog.FileName;
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
            SaveProfit_Click(sender, e);
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
            SettingsGrid.Visibility = Visibility.Collapsed;
            ChartGrid.Visibility = Visibility.Collapsed;


        }

        private void Items_Click(object sender, RoutedEventArgs e)
        {
            ResetDataGrid();
            ReportsGrid.Visibility = Visibility.Collapsed;
            ItemsGrid.Visibility = Visibility.Visible;
            SettingsGrid.Visibility = Visibility.Collapsed;
            ChartGrid.Visibility = Visibility.Collapsed;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            ReportsGrid.Visibility = Visibility.Collapsed;
            ItemsGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Visible;
            ChartGrid.Visibility = Visibility.Collapsed;
        }

        private void ChartTab_Click(object sender, RoutedEventArgs e)
        {
            ReportsGrid.Visibility = Visibility.Collapsed;
            ItemsGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Collapsed;
            ChartGrid.Visibility = Visibility.Visible;
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
                //ResetDataGrid();
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

        private void CreateItemReport_Click(object sender, RoutedEventArgs e)
        {
            vm.CreateItemReport();
            ResetDataGrid();
        }

        private void SaveProfit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(vm.DatabaseLocation))
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "sqlite3 files (*.sqlite3)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == true)
                {
                    if(!string.IsNullOrWhiteSpace(saveFileDialog1.FileName))
                    {
                        vm.DatabaseLocation = saveFileDialog1.FileName;
                        vm.SaveOrderItemListToFile();
                    }
                }
            }
            else
            {
                vm.SaveOrderItemListToFile();

            }

        }

        private void DataGridProfit_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Cancel)
            {
                return;
            }
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var item = e.Row.Item as OrderItem;
                var textbox = e.EditingElement as TextBox;
                vm.EditOrderListItem(item, e.Column.Header.ToString(), textbox.Text);
            }
        }

        private void OpenDatabase_Click(object sender, RoutedEventArgs e)
        {
            vm.OpenDatabase();
            Properties.Settings.Default.Save();
        }

        private void ImportEbayTransaction_Click(object sender, RoutedEventArgs e)
        {
            EbayWebBrowser.Visibility = Visibility.Visible;
            ebayAddress = vm.GetAuthSource().AbsoluteUri;
            EbayWebBrowser.Address = ebayAddress;
        }

        private void EbayWebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EbayWebBrowser.Address))
            {
                vm.GetEbayBrowserVisiblity(new Uri(EbayWebBrowser.Address));
            }
        }

        private void EbayWebBrowser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                ebayAddress = EbayWebBrowser.Address;
            });
            if (!e.IsLoading)
            {
                if (!string.IsNullOrWhiteSpace(ebayAddress))
                {
                    vm.GetEbayBrowserVisiblity(new Uri(ebayAddress));
                }
            }
            if(vm.HasEbayToken())
            {
                vm.GetEbayTransaction();
                Properties.Settings.Default.LastHitDateEbay = DateTime.Now;
                Properties.Settings.Default.Save();
            }
        }

        private void ChartGraph_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fromDate = ChartFromComboBox.SelectedIndex+1;
                var ToDate = ChartToComboBox.SelectedIndex+1;
                var graphData = vm.ChartGraph(new DateTime(DateTime.Now.Year, fromDate, 1), new DateTime(DateTime.Now.Year, ToDate+1, 1));
                ChartCanvas.Children.Clear();
                var maxX = ChartCanvas.ActualWidth;
                var maxY = ChartCanvas.ActualHeight + 1;
                var maxValueX = graphData.XTicks.Count;
                var maxValueY = graphData.YTicks;
                var YTicks = Math.Round(maxY / maxValueY);
                var points = graphData.Points;

                for (int i = 0; i < YTicks; i++)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = Math.Round(((maxValueY / (YTicks - i)))).ToString();
                    textBlock.Foreground = Brushes.Black;
                    ChartCanvas.Children.Add(textBlock);
                    var xPos = /*(((double)i + 1) / maxValueX) * */ -20;
                    var yPos = (((double)(i + 1) * 10) / maxValueY) * maxY;
                    Canvas.SetLeft(textBlock, xPos);
                    var topPos = (maxY - (maxY * (double)((i + 1)/YTicks))) - 1;
                    Canvas.SetTop(textBlock, topPos);
                }

                for (int i = 0; i < maxValueX; i++)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = graphData.XTicks[i];
                    textBlock.Foreground = Brushes.Black;
                    ChartCanvas.Children.Add(textBlock);
                    var xPos = (((double)i + 1) / maxValueX) * maxX;
                    var yPos = /*((double)1 / maxValueY) * */ maxY;
                    Canvas.SetLeft(textBlock, xPos);
                    Canvas.SetTop(textBlock, yPos);
                }

                PointCollection myPointCollection2 = new PointCollection();
                var myPolyline = new Polyline();

                foreach (var point in points)
                {
                    var circlePoint = new Ellipse
                    {
                        Width = 5,
                        Height = 5,
                        Fill = Brushes.Black,
                    };

                    ChartCanvas.Children.Add(circlePoint);
                    var xPos = (point.X / maxValueX) * maxX;
                    var yPos = (point.Y / maxValueY) * maxY;
                    Canvas.SetLeft(circlePoint, xPos);
                    Canvas.SetTop(circlePoint, maxY - yPos);

                    myPolyline.Stroke = Brushes.SlateGray;
                    myPolyline.StrokeThickness = 2;
                    myPolyline.FillRule = FillRule.EvenOdd;
                    System.Windows.Point Point4 = new System.Windows.Point(xPos, maxY - yPos);
                    myPointCollection2.Add(Point4);
                    myPolyline.Points = myPointCollection2;
                }
                ChartCanvas.Children.Add(myPolyline);
            }
            catch
            {

            }
        }

        private void ChartInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.ChartInfoSelectionChanged(e.AddedItems[0].ToString());
        }

        private void ImportReportButton_Click(object sender, RoutedEventArgs e)
        {        
            var reportfilelocation = ebayOrderReportTextbox.Text;
            vm.ImportEbayOrderReport(reportfilelocation);
        }

        private void SearchReportButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                ebayOrderReportTextbox.Text = fileDialog.FileName;
            }
        }
    }
}
