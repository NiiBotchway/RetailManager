using RetailManager.DataAccessLibrary.Internal.DataAccess;
using RetailManager.DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DataAccessLibrary.DataAccess
{
    public class SaleData
    {
        SqlDataAccess dataAccess = new SqlDataAccess();
        ProductData products = new ProductData();



        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            if (saleInfo == null) throw new ArgumentNullException(nameof(saleInfo));
            if (cashierId == null) throw new ArgumentNullException(nameof(cashierId));

            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = PopulateSaleDetailDBModel(item);

                details.Add(detail);
            }

            var sale = PopulateSaleDBModel(details, cashierId);

            var saleId = dataAccess.FetchData<int, SaleDBModel>("spSale_Insert", sale, "RetailManagerDB").FirstOrDefault();


            foreach (var item in details)
            {
                item.SaleId = saleId;

                dataAccess.SaveData("spSaleDetail_Insert", item, "RetailManagerDB");
            }
        }

        private SaleDBModel PopulateSaleDBModel(List<SaleDetailDBModel> details, string cashierId)
        {
            SaleDBModel sale = new SaleDBModel
            {
                CashierId = cashierId,
                Subtotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax)
            };
            sale.Total = sale.Subtotal + sale.Tax;
            return sale;
        }

        private SaleDetailDBModel PopulateSaleDetailDBModel(SaleDetailModel item)
        {
            var productInfo = products.GetProductById(item.ProductId);

            if (productInfo == null)
            {
                throw new Exception($"Product with id {item.ProductId} could not be found!");
            }

            var detail = new SaleDetailDBModel
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PurchasePrice = Decimal.Parse(productInfo.RetailPrice) * item.Quantity,
            };
            detail.Tax = productInfo.IsTaxable ? CalculateTax(detail.PurchasePrice) : 0;
            return detail;

        }


        private decimal CalculateTax(decimal PurchasePrice)
        {

            decimal taxAmount;
            decimal taxRate = Decimal.Parse(ConfigurationManager.AppSettings.Get("taxRate")) / 100;

            taxAmount = PurchasePrice * taxRate;
            return taxAmount;

        }

        //public IEnumerable<ProductModel> GetProducts()
        //{
        //    var output = dataAccess.LoadData<ProductModel, dynamic>("spProduct_GetAll", new { }, "RetailManagerDB");

        //    return output;
        //}GetProductById
    }
}
