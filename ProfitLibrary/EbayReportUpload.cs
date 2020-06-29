using System;
using System.Collections.Generic;
using System.IO;

namespace ProfitLibrary
{
    public class EbayReportUpload
    {
        private const int order_number = 1;
        private const int item_number= 20;
        private const int title_name = 21;
        private const int custom_label = 22;
        private const int qauntity = 24;
        private const int sold_for = 25;
        private const int tax = 28;
        private const int shipping=26;
        private const int sale_date = 35;
        private const int trans_id = 47;

        public static List<OrderItem> GetOrderReport(string file)
        {
            var deliminator = ',';
            var orderItems = new List<OrderItem>();
            using (var reader = new StreamReader(file))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                var line = reader.ReadLine();
                line = reader.ReadLine();
                line = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var newItem = true;
                    var chararray = reader.ReadLine().ToCharArray();
                    var skip = false;
                    for (int i = 0; i < chararray.Length; i++)
                    {
                        if (chararray[i] == '"')
                        {
                            skip = !skip;
                        }
                        if (skip)
                        {
                            continue;
                        }
                        if (deliminator == chararray[i])
                        {
                            chararray[i] = ';';
                        }
                    }
                    line = new string(chararray);
                    var values = line.Split(';');
                    OrderItem orderItem = new OrderItem();
                    if (string.IsNullOrEmpty(values[order_number].Replace($@"""", "")) || values[order_number].Replace($@"""", "") == "record(s) downloaded")
                    {
                        continue;
                    }
                    orderItem.BoughtFrom = "Ebay";
                    orderItem.DateSold = ExtractDate(values);
                    orderItem.SoldFor = PaymentDetail.ConvertDollarstoPennies(values[sold_for].Replace($@"""", "")) + PaymentDetail.ConvertDollarstoPennies(values[shipping].Replace($@"""", "")) + PaymentDetail.ConvertDollarstoPennies(values[tax].Replace($@"""", ""));
                    orderItem.ItemName = values[title_name].Replace($@"""", "");
                    orderItem.OrderID = values[order_number].Replace($@"""", "");
                    orderItem.QuantitySold = int.Parse(values[qauntity].Replace($@"""", ""));
                    orderItem.SalesTax = PaymentDetail.ConvertDollarstoPennies(values[tax].Replace($@"""", ""));
                    orderItem.SKU = values[item_number].Replace($@"""", "");
                    orderItem.TransID = values[trans_id].Replace($@"""", "");

                    orderItems.Add(orderItem);
                }
            }
            return orderItems;
        }

        private static string ExtractDate(string[] values)
        {
            var date = DateTime.MinValue;
            if (DateTime.TryParseExact(values[sale_date].Replace($@"""", ""), "MMM-dd-yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
            {
                return date.ToString("MM/dd/yyyy");
            };

            return "";
        }
    }
}
