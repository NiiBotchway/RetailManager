using AutoMapper;
using Caliburn.Micro;
using RetailManager.DesktopUI.Library.Api;
using RetailManager.DesktopUI.Library.Helpers;
using RetailManager.DesktopUI.Library.Models;
using RetailManager.DesktopUI.Models;
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
        private readonly ISaleEndpoint _saleEndpoint;
        private readonly IMapper _mapper;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint, IMapper mapper)
        {
            this._productEndpoint = productEndpoint;
            this._configHelper = configHelper;
            this._saleEndpoint = saleEndpoint;
            this._mapper = mapper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            var productList = await _productEndpoint.GetProducts();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private BindingList<ProductDisplayModel> _products = new BindingList<ProductDisplayModel>();

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductDisplayModel _selectedProduct;

        public ProductDisplayModel SelectedProduct
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

        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private CartItemDisplayModel _selectedCartItem;

        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;

                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }


        public void AddToCart()
        {
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                CartItemDisplayModel Item = new CartItemDisplayModel
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

            //ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanAddToCart
        {
            get
            {
                //check if a product is selected and there is a quantity attached to the selected ProductDisplayModel

                bool result = ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;
                return result;
            }
        }
        public void RemoveFromCart()
        {
            var ExistingProduct = Products.FirstOrDefault(x => x == SelectedCartItem.Product);

            if (ExistingProduct == null)
            {
                Products.Add(SelectedCartItem.Product);
            }


            SelectedCartItem.Product.QuantityInStock += 1;

            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanRemoveFromCart);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                return SelectedCartItem != null;
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

            subTotal = Cart.Sum(x => x.Product.RetailPrice * x.QuantityInCart);

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

            taxAmount = Cart.Where(x => x.Product.IsTaxable).Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

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
                return Cart.Any();
            }
        }

        public async Task CheckOut()
        {
            Sale sale = new Sale();


            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetails()
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }
            await _saleEndpoint.PostSale(sale);
        }
    }
}