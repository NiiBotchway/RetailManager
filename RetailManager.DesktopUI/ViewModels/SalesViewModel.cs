using Caliburn.Micro;
using RetailManager.DesktopUI.Library.Api;
using RetailManager.DesktopUI.Library.Helpers;
using RetailManager.DesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {

        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
        {
            this._productEndpoint = productEndpoint;
            this._configHelper = configHelper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var products = await _productEndpoint.GetProducts();
            Products = new BindingList<Product>(products);
        }

        private BindingList<Product> _products = new BindingList<Product>();

        public BindingList<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private Product _selectedProduct;

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;

                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        private int _itemQuantity = 1;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private BindingList<CartItem> _cart = new BindingList<CartItem>();

        public BindingList<CartItem> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        public void AddToCart()
        {
            CartItem existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (Cart.Contains(existingItem))
            {
                Cart.Remove(existingItem);
                existingItem.QuantityInCart += ItemQuantity;

                Cart.Add(existingItem);
            }
            else
            {
                CartItem Item = new CartItem
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };

                Cart.Add(Item);
            }



            SelectedProduct.QuantityInStock -= ItemQuantity;
            if (SelectedProduct.QuantityInStock <= 0)
            {
                Products.Remove(SelectedProduct);
            }

            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanAddToCart
        {
            get
            {
                //check if a product is selected and there is a quantity attached to the selected Product
                return (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity);
                //return false;
            }
        }
        public void RemoveFromCart()
        {

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                // return ((Products?.Length ?? 0) > 0);
                return true;
            }
        }




        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += item.Product.RetailPrice * item.QuantityInCart;
            }
            return subTotal;
        }

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = Convert.ToDecimal(_configHelper.GetTaxRate() / 100);
            foreach (var item in Cart)
            {
                if (item.Product.IsTaxable)
                {

                    taxAmount += item.Product.RetailPrice * item.QuantityInCart * taxRate;
                }

            }
            return taxAmount;
        }


        public string Total
        {
            get
            {
                return CalculateTotal().ToString("C");
            }
        }

        private decimal CalculateTotal()
        {
            //return CalculateSubTotal() + CalculateTax();
            return Decimal.Parse(SubTotal, NumberStyles.Currency) + Decimal.Parse(Tax, NumberStyles.Currency);
        }

        public bool CanCheckOut
        {
            get
            {
                //checkout
                return true;
            }
        }

        public void CheckOut()
        {

        }
    }
}