using Caliburn.Micro;
using RetailManager.DesktopUI.Library.Api;
using RetailManager.DesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {

        private BindingList<Product> _cart;
        private readonly IProductEndpoint _productEndpoint;
        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            this._productEndpoint = productEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var products = await _productEndpoint.GetProducts();
            Products = new BindingList<Product>(products);
        }

        private BindingList<Product> _products;

        public BindingList<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private int _itemQuantity;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public void AddToCart()
        {

        }

        public bool CanAddToCart
        {
            get
            {
                // return ((Products?.Length ?? 0) > 0);
                return true;
            }
        }
        public void RemoveFromCart()
        {

        }

        public bool CanRemoveFromCart
        {
            get
            {
                // return ((Products?.Length ?? 0) > 0);
                return true;
            }
        }


        public BindingList<Product> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public string SubTotal
        {
            get
            {
                //calculate here
                return "$0.00";
            }
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