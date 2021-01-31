using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace MI_Metrics
{
    public enum IconColor
    {
        Yellow,
        Red
    }
    
    public class HomePageViewModel : INotifyPropertyChanged
    {
        private HomePage homePage;        
        public ProductManager productManager;
        public AddProductUtil addProductUtil;
        public EditProductUtil editProductUtil;

        public List<Image> images;
        private string selectedImageFile;      
        private string editedImageFile;

        public string ValidImageExtensions
        {
            get
            {
                string extensions = "";
                foreach (string extension in AddProductUtil.ValidImageExtensions)
                {
                    if (!extensions.Equals("")) extensions += ", ";
                    extensions += extension;
                }
                return extensions;
            }
        }

        private ObservableCollection<Product> productView;
        public ObservableCollection<Product> ProductView { get => productView; set => productView = value; }

        /*
         * Several properties are kept here but are managed in a different class because this class (HomePageViewModel.cs) acts
         * as the data context for the home page. The ProductView property for example is used in a binding on the home page
         * but ProductManager is what is actually changing and updating the ObservableCollection.
         */
        public ObservableCollection<AddProductError> AddProductErrorMessages { get => addProductUtil.ErrorMessages; set => addProductUtil.ErrorMessages = value; }
        public ObservableCollection<EditProductError> EditProductErrorMessages { get => editProductUtil.ErrorMessages; set => editProductUtil.ErrorMessages = value; }


        public string SelectedImageFile
        {
            get => selectedImageFile;
            set
            {
                if (!value.Equals(selectedImageFile))
                {
                    selectedImageFile = value;
                    OnPropertyChanged();
                }
            }
        }
        public string EditedImageFile
        {
            get => editedImageFile;
            set
            {
                if (!value.Equals(editedImageFile))
                {
                    editedImageFile = value;
                    OnPropertyChanged();
                }
            }
        }

        public HomePageViewModel(HomePage homePage)
        {
            this.homePage = homePage;
            this.homePage.DataContext = this;

            selectedImageFile = "";
            EditedImageFile = "";

            images = new List<Image>();
            this.productManager = MainWindow.productManager;
            BuildProductView();

            addProductUtil = new AddProductUtil(this);
            editProductUtil = new EditProductUtil(this);
        }

        public void BuildProductView()
        {
            // The ProductView starts off a copy of Products since there isn't any filtering being done when the program starts.
            productView = new ObservableCollection<Product>(productManager.Products);
        }

        public void FilterProductView(string filter)
        {
            ProductView.Clear();
            string searchText = filter.ToUpper();

            foreach (Product product in productManager.Products)
            {
                if (product.Name.ToUpper().Contains(searchText))
                {
                    ProductView.Add(product);
                }

            }
        }
        
        public SelectFileResult SelectFile()
        {
            SelectFileResult result = addProductUtil.SelectAndStoreImageFile();
            string resultName = Path.GetFileName(result.filePath);
            if (!(resultName.Equals("") || resultName == null))
                SelectedImageFile = Path.GetFileName(result.filePath);
            return result;
        }

        public SelectFileResult SelectEditFile()
        {
            // The AbbreviatedProductList.SelectedItem is needed to see if a duplicate image error needs to be added
            Product selectedProduct = (Product)homePage.AbbreviatedProductList.SelectedItem;
            string fileName = Path.GetFileName(selectedProduct.ImagePath);
            SelectFileResult result = editProductUtil.SelectAndStoreImageFile(fileName);
            string resultName = Path.GetFileName(result.filePath);
            
            if (!(resultName.Equals("") || resultName == null))
            {
                EditedImageFile = resultName;
            }
                
            return result;
        }

        public void ClearSelectedFile()
        {
            SelectedImageFile = "";
            AddProductUtil.DeleteAllTempImageFiles();
        }

        public void ClearEditImageFile()
        {
            EditedImageFile = "";
            EditProductUtil.DeleteAllTempImageFiles();
        }

        public void UpdateProductImage(Product product)
        {
            foreach(Image image in images)
            {
                Product currentProduct = (Product)image.Tag;
                if(product.ProductID == currentProduct.ProductID)
                {
                    currentProduct.ImagePath = product.ImagePath;
                    homePage.StreamImageFromSource(currentProduct.ImagePath, image);
                    break;
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
