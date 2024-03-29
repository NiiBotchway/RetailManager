﻿using System.ComponentModel;

namespace RetailManager.DesktopUI.Models
{
    public class ProductDisplayModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        private int _quantityInStock;

        public int QuantityInStock
        {
            get { return _quantityInStock; }
            set
            {
                _quantityInStock = value;
                InvokePropertyChanged(nameof(QuantityInStock));
            }
        }
        public bool IsTaxable { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        private void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}