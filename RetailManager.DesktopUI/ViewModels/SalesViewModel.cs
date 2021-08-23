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


        private BindingList<ProductDisplayModel> _products = new BindingList<ProductDisplayModel>();

        private ProductDisplayModel _selectedProduct;

        private int _itemQuantity = 1;

        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

        private CartItemDisplayModel _selectedCartItem;




        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetProducts();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private async Task ResetPage()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanRemoveFromCart);
        }




        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }


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


        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


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


        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }
        public string Total
        {
            get
            {
                return CalculateTotal().ToString("C");
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

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;

            subTotal = Cart.Sum(x => x.Product.RetailPrice * x.QuantityInCart);

            return subTotal;
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = Convert.ToDecimal(_configHelper.GetTaxRate() / 100);

            taxAmount = Cart.Where(x => x.Product.IsTaxable).Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            return taxAmount;
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
            await ResetPage();
        }

    }
}