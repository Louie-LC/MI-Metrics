using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public class EditProductUtil
    {
        HomePageViewModel viewModel;

        public const string TempFileName = "NewSystemImage";
        private string currentTempFile;

        private ObservableCollection<EditProductError> errorMessages;

        public ObservableCollection<EditProductError> ErrorMessages { get => errorMessages; set => errorMessages = value; }

        public EditProductUtil(HomePageViewModel viewModel)
        {
            this.viewModel = viewModel;
            errorMessages = new ObservableCollection<EditProductError>();
            currentTempFile = "";
            AddErrorMessage(EditErrorCode.SelectionRequired);
        }

        public void NameChagned()
        {
            AddErrorMessage(EditErrorCode.NameChanged);
        }


        public SelectFileResult SelectAndStoreImageFile(string currentFile)
        {
            // CurrentFile is needed to check against the file being selected
            SelectFileResult fileResult = DialogManager.ShowDialog(DialogType.Image);
            // If fileResult.validFileSelected is false at this point, then no file was selected when the dilog was displayed.
            if (fileResult.validFileSelected)
            {
                if (ValidateFile(fileResult.filePath, currentFile))
                {
                    
                    StoreTemporaryImageFile(fileResult.filePath);
                }
                else
                {
                    fileResult.validFileSelected = false;
                }
            }
            return fileResult;
        }

        public void StoreTemporaryImageFile(string filePath)
        {
            currentTempFile = null;
            DeleteAllTempImageFiles();
            if (File.Exists(filePath))
            {
                string extension = Path.GetExtension(filePath);
                currentTempFile = MainWindow.currentDir + TempFileName + extension;
                File.Copy(filePath, currentTempFile);
                SetTemporaryFileAttributes(currentTempFile);
            }
        }
        private static void SetTemporaryFileAttributes(string filePath)
        {
            FileAttributes attributes = File.GetAttributes(filePath);
            File.SetAttributes(filePath, attributes | FileAttributes.Hidden | FileAttributes.Temporary);
        }
        private void RemoveTemporaryFileAttributes(string filePath)
        {
            FileAttributes attributes = File.GetAttributes(filePath);
            File.SetAttributes(filePath, attributes ^ (FileAttributes.Hidden | FileAttributes.Temporary));
        }

        public void SaveImageFile(string destinationFileName)
        {
            string destinationFile = MainWindow.currentDir + "\\Images\\" + destinationFileName;
            if (!File.Exists(destinationFile))
            {
                File.Copy(currentTempFile, destinationFile);
                RemoveTemporaryFileAttributes(destinationFile);
            }
        }

        public void DeleteCurrentImageFile(string currentFilePath)
        {
            if(File.Exists(currentFilePath))
            {
                File.Delete(currentFilePath);
            }
        }

        public bool ValidateFile(string filePath, string currentFile)
        {
            // Checks if the file is valid to use.
            RemoveAllFileBasedErrors();
            bool validFileFound = true;
            // Check if the file selected exists
            if (!File.Exists(filePath))
            {
                AddErrorMessage(EditErrorCode.FileNotFound);
                validFileFound = false;
            }
            // Check if the selected file uses an image extension that is supported by WPF
            string fileExtension = Path.GetExtension(filePath).ToLower();
            bool foundExtensionMatch = false;
            foreach (string extension in AddProductUtil.ValidImageExtensions)
            {
                if (fileExtension.Equals(extension)) foundExtensionMatch = true;
            }
            if (!foundExtensionMatch)
            {
                AddErrorMessage(EditErrorCode.InvalidFileExtension);
                validFileFound = false;
            }
            // Check if a file with the same name already exists in the images directory
            string fileName = Path.GetFileName(filePath);
            
            /*
             * The file being selected can not have the same name as a file that already exists within the image directory.
             * The only exception to this is if the selected file share the same name as the file being edited.
             * In this case, the file being edited will first be deleted, and then the slected file will be copied over
             * so duplicate files existing shouldn't be an issue
             */
            if (!currentFile.Equals(fileName) && ImageWithSameNameExists(fileName))
            {
                AddErrorMessage(EditErrorCode.DuplicateFileFound);
                validFileFound = false;
            }
            // Check if the file name is too long to be stored in the database
            if(fileName.Length > 100)
            {
                AddErrorMessage(EditErrorCode.ImageNameTooLong);
                validFileFound = false;
            }
            AddErrorMessage(EditErrorCode.ImageChanged);
            return validFileFound;
        }

        public bool ImageWithSameNameExists(string imageName)
        {
            string potentialFilePath = MainWindow.currentDir + "\\Images\\" + imageName;
            bool doesExist = File.Exists(potentialFilePath);
            return File.Exists(potentialFilePath);
        }

        public bool ValidateName(string name)
        {
            RemoveErrorMessage(EditErrorCode.NameRequired);
            if (name.Trim().Equals("") || name == null)
            {
                AddErrorMessage(EditErrorCode.NameRequired);
                return false;
            }
            
            return true;
        }
        public bool ValidateCharactersInName(string name)
        {
            RemoveErrorMessage(EditErrorCode.InvalidCharacterInName);
            bool charactersAreValid = DatabaseHelper.ValidateCharacters(name);
            if (!charactersAreValid)
            {
                AddErrorMessage(EditErrorCode.InvalidCharacterInName);
            }
            return charactersAreValid;
        }
        
        public void RemoveErrorMessage(EditErrorCode errorCode)
        {
            foreach (EditProductError error in errorMessages)
            {
                if (errorCode == error.Code)
                {
                    errorMessages.Remove(error);
                    break;
                }
            }
        }

        public void AddErrorMessage(EditErrorCode errorCode)
        {
            bool errorAlreadyExists = false;
            foreach (EditProductError error in errorMessages)
            {
                if (errorCode == error.Code) errorAlreadyExists = true;
            }
            if (!errorAlreadyExists) errorMessages.Add(new EditProductError(errorCode));
        }

        public void RemoveAllFileBasedErrors()
        {
            RemoveErrorMessage(EditErrorCode.InvalidFileExtension);
            RemoveErrorMessage(EditErrorCode.FileNotFound);
            RemoveErrorMessage(EditErrorCode.DuplicateFileFound);
            RemoveErrorMessage(EditErrorCode.ImageChanged);
        }

        public void RemoveAllNameBasedErrors()
        {
            RemoveErrorMessage(EditErrorCode.NameRequired);
            RemoveErrorMessage(EditErrorCode.InvalidCharacterInName);
            RemoveErrorMessage(EditErrorCode.NameChanged);
        }

        public void InformEditImageRemoved()
        {
            RemoveAllFileBasedErrors();
            AddErrorMessage(EditErrorCode.ImageChanged);
        }

        public void InformSystemNotRequired()
        {
            RemoveErrorMessage(EditErrorCode.SelectionRequired);
        }
        public void InformSystemRequired()
        {
            AddErrorMessage(EditErrorCode.SelectionRequired);
        }

        public static void DeleteAllTempImageFiles()
        {
            // This will remove all temporary file that may still be around due to the file transfer process not completing.
            string[] filePaths = Directory.GetFiles(MainWindow.currentDir, TempFileName + ".*");
            foreach (string filePath in filePaths)
            {
                File.Delete(filePath);
            }
        }

        public void UpdateProductName(Product product, string name)
        {
            Product currentProduct = MainWindow.database.productManager.GetProduct(product.ProductID);
            MainWindow.database.productManager.UpdateProduct(product.ProductID, name, currentProduct.ImagePath);
            product.Name = name;
            viewModel.productManager.ExpandProductImagePath(product);
        }

        public void UpdateProductImage(Product product, string imageName)
        {
            Product currentProduct = MainWindow.database.productManager.GetProduct(product.ProductID);
            MainWindow.database.productManager.UpdateProduct(product.ProductID, currentProduct.Name, imageName);

            DeleteCurrentImageFile(product.ImagePath);

            if (imageName != null && !imageName.Equals(""))
            {
                SaveImageFile(imageName);
            }
            product.ImagePath = imageName;
            viewModel.productManager.ExpandProductImagePath(product);
            viewModel.UpdateProductImage(product);
        }

        public void UpdateProduct(Product product, string name, string imageName)
        {
            MainWindow.database.productManager.UpdateProduct(product.ProductID, name, imageName);
            // If the image currently being used is the default image, then DeleteCurrentImageFile 
            // will not be able to find it so it will skip trying to delete it.
            DeleteCurrentImageFile(product.ImagePath);
            if (imageName != null && !imageName.Equals(""))
            {
                SaveImageFile(imageName);
            }
            product.Name = name;
            product.ImagePath = imageName;
            viewModel.productManager.ExpandProductImagePath(product);
            viewModel.UpdateProductImage(product);
        }


    }
}
