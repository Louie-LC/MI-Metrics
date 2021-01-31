using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace MI_Metrics
{
    public class ProductManager
    {
        public List<Product> Products
        {
            get => products;
            set
            {
                products = value;
                productView = new ObservableCollection<Product>(Products);
            }
        }
        private List<Product> products;

        // This acts as a view for pages so that they can have an updated version of the product list.
        public ObservableCollection<Product> ProductView { get => productView; set => productView = value; }
        private ObservableCollection<Product> productView;

        public ProductManager()
        {
            products = new List<Product>();

            this.Products = products;
            BuildProductList();
        }

        public void BuildProductList()
        {
            Products = MainWindow.database.productManager.RetrieveProductList();
            ExpandProductImagePath(Products);
            
            foreach (Product product in Products)
            {
                product.CreateTestList();
            }
        }

        public void ExpandProductImagePath(List<Product> products)
        {
            foreach (Product product in products)
            {
                ExpandProductImagePath(product);
            }
        }

        public void ExpandProductImagePath(Product product)
        {
            String bs = "\\";
            string currentPath = product.ImagePath;

            // If an image logo wasn't set in the database, use the siemens logo.
            if (currentPath == null || currentPath.Equals(""))
            {
                product.ImagePath = "pack://application:,,,/Images/logo_rgb_2x_l.png";
            }
            else
            {
                product.ImagePath = MainWindow.currentDir + "Images" + bs + currentPath;
            }
            // If the image that was set is no longer in the folder, use the siemens logo.
            if (!File.Exists(product.ImagePath))
            {
                product.ImagePath = "pack://application:,,,/Images/logo_rgb_2x_l.png";
            }
        }
    }
}
