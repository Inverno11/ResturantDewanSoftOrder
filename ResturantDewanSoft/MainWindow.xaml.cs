using AutoMapper;
using ResturantDewanSoft.Models.DataBaseModels;
using ResturantDewanSoft.Repository;
using ResturantDewanSoft.StartupClasses;
using ResturantDewanSoft.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Printing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace ResturantDewanSoft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        UnitOfWork unitOfWork;

        public MainWindow()
        {
            InitializeComponent();
            AutoMapperConfig.Init();
            initializeProductList();
            Disable();
            DateLabel.Content = System.DateTime.Now;
        }



        public void initializeProductList() 
        {

            IMapper mapper = AutoMapperConfig.Mapper; 
            unitOfWork = new UnitOfWork();
            var list = unitOfWork.GetRepositoryInstance<Item>().GetAllRecordes();
            var listModel = mapper.Map<List<ItemModel>>(list);
            ProductDataGrid.ItemsSource= listModel;



        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(QuantityTextBox.Text) && ProductDataGrid.SelectedItems.Count!=0)

            {

                ItemModel productItem = (ItemModel)ProductDataGrid.SelectedItem;

                OrderModel orderModel = new OrderModel
                {
                    Name = productItem.Name,
                    Price = productItem.Price.Value,
                    Quantity = Int32.Parse(QuantityTextBox.Text)
                };
                orderModel.Total = orderModel.Quantity * orderModel.Price;
                OrderDataGrid.Items.Add(orderModel);

                var list = OrderDataGrid.Items.Cast<OrderModel>();
                receiptTotalLabel.Content = list.Sum(it => it.Quantity * it.Price);
                QuantityTextBox.Clear();


            }
            
        }

        private static readonly Regex _regex = new Regex("[^0 - 9]+"); //regex that matches disallowed text
       

       


        private void PrintFuction(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.Items != null)
            {


                XpsDocument doc = new XpsDocument("D:\\print_previw.xps", FileAccess.Write);

                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);

                SerializerWriterCollator preview_Document = writer.CreateVisualsCollator();

                preview_Document.BeginBatchWrite();

                preview_Document.Write(WholeOrderDataGrid);  //*this or wpf xaml control

                preview_Document.EndBatchWrite();

                //--</ create xps document >--



                var doc2 = new XpsDocument("D:\\print_previw.xps", FileAccess.Read);



                FixedDocumentSequence preview = doc2.GetFixedDocumentSequence();



                var window = new Window();

                window.Content = new DocumentViewer { Document = preview };

                window.ShowDialog();



                doc.Close();

                ClearContet();
                    Disable();

                

           



                
            }

        }

        public void Disable()
        {
            AddBtn.IsEnabled = true;
            CustomerNameTextBox.IsEnabled = false;
            QuantityTextBox.IsEnabled = false;
            AddItem.IsEnabled = false;
            PrintBtn.IsEnabled = false;
        }

        public void Enable()
        {
            CustomerNameTextBox.IsEnabled = true;
            AddBtn.IsEnabled = true;
            QuantityTextBox.IsEnabled = true;
            DateLabel.Content = System.DateTime.Now;
            PrintBtn.IsEnabled = true;
            AddItem.IsEnabled = true;



        }


        public void ClearContet() 
        {
            QuantityTextBox.Clear();
            OrderDataGrid.Items.Clear();
            receiptTotalLabel.Content = "0";
            CustomerNameTextBox.Clear();
            
        }

        public void GetNumberOfOrder()
        {
            int numberOfOrder = unitOfWork.GetRepositoryInstance<Order>().LastId();
            numberOfOrder++;
            LabelofOrderNumber.Content = numberOfOrder;




        }
        public void SubmitOrder()
        {
            if(!String.IsNullOrEmpty(CustomerNameTextBox.Text) && OrderDataGrid.Items.Count != 0)
            {

                var orders = OrderDataGrid.Items.Cast<OrderModel>();
                Order order = new Order();

                Customer customer = new Customer();
                customer.Name = CustomerNameTextBox.Text;
                customer.DateOrder =(DateTime)DateLabel.Content;
                customer.TotalPrice = Decimal.Parse(receiptTotalLabel.Content.ToString());

                var CustomerUnitOfWork = unitOfWork.GetRepositoryInstance<Customer>();
                CustomerUnitOfWork.Add(customer);

                var OrderUnitOfWork = unitOfWork.GetRepositoryInstance<Order>();


                foreach (var orderModel in orders)
                {

                    var item= unitOfWork.GetRepositoryInstance<Item>().FindIdByName(orderModel.Name);
                    order.ItemId = item.Id;
                    order.UserId = customer.Id;
                    order.quantity = orderModel.Quantity;
                    OrderUnitOfWork.Add(order);

                }



            }
        
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Enable();
            GetNumberOfOrder();

        }

        private void SubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            SubmitOrder();
        }
    }
}
