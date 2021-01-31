using System.Collections.ObjectModel;
using System.IO;


namespace MI_Metrics
{
    public class AddProductUtil
    {
        private HomePageViewModel viewModel;

        public const string TempFileName = "TemporaryButNecessaryImage";
        private string currentTempFile;
        public bool validImageSelected = false;

        private ObservableCollection<AddProductError> errorMessages;
        private static string[] validImageExtensions = { ".png", ".jpg", ".gif" };
        internal ObservableCollection<AddProductError> ErrorMessages { get => errorMessages; set => errorMessages = value; }
        public static string[] ValidImageExtensions { get => validImageExtensions; set => validImageExtensions = value; }

        public AddProductUtil(HomePageViewModel viewModel)
        {
            this.viewModel = viewModel;
            errorMessages = new ObservableCollection<AddProductError>();
            AddErrorMessage(ErrorCode.NameRequired);
        }

        public void AddProduct(string name, string imageName)
        {
            Product product = MainWindow.database.productManager.AddProduct(name, imageName);
            if (imageName != null && !imageName.Equals(""))
            {
                SaveImageFile(imageName);
            }
            viewModel.productManager.ExpandProductImagePath(product);
            viewModel.productManager.Products.Add(product);
            viewModel.productManager.ProductView.Add(product);
        }

        public SelectFileResult SelectAndStoreImageFile()
        {
            SelectFileResult fileResult = DialogManager.ShowDialog(DialogType.Image);
            // If fileResult.validFileSelected is false at this point, then no file was selected when the dilog was displayed.
            if(fileResult.validFileSelected)
            {
                // If a file was selected, validate it.
                if (ValidateFile(fileResult.filePath))
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
        public bool ValidateFile(string filePath)
        {
            // Checks if the file is valid to use.
            RemoveAllFileBasedErrors();
            // Check if the file selected exists
            if (!File.Exists(filePath))
            {
                AddErrorMessage(ErrorCode.FileNotFound);
                return false;
            }
            // Check if the selected file uses an image extension that is supported by WPF
            string fileExtension = Path.GetExtension(filePath).ToLower();
            
            bool validFileFound = true;

            // Check if the file extension is valid
            bool foundExtensionMatch = false;
            foreach (string extension in ValidImageExtensions)
            {
                if (fileExtension.Equals(extension)) foundExtensionMatch = true;
            }
            if (!foundExtensionMatch)
            {
                AddErrorMessage(ErrorCode.InvalidFileExtension);
                validFileFound = false;
            }
            // Check if a file with the same name already exists in the images directory
            string fileName = Path.GetFileName(filePath);
            if (ImageWithSameNameExists(fileName))
            {
                AddErrorMessage(ErrorCode.DuplicateFileFound);
                validFileFound = false;
            }
            // Check if the file name is too long to be contained in the database
            if(fileName.Length > 100)
            {
                AddErrorMessage(ErrorCode.ImageNameTooLong);
                validFileFound = false;
            }
            return validFileFound;
        }

        public bool ValidateName(string name)
        {
            RemoveAllNameBasedErrors();
            if (name.Trim().Equals("") || name == null)
            {   
                AddErrorMessage(ErrorCode.NameRequired);
                return false;
            }
            return true;
        }

        public bool ValidateCharactersInName(string name)
        {
            RemoveErrorMessage(ErrorCode.InvalidCharacterInName);
            bool validCharactersInName = DatabaseHelper.ValidateCharacters(name);
            if (!validCharactersInName)
            {
                AddErrorMessage(ErrorCode.InvalidCharacterInName);
            }
            return validCharactersInName;
        }

        public bool ImageWithSameNameExists(string imageName)
        {
            string potentialFilePath = MainWindow.currentDir + "\\Images\\" + imageName;
            bool doesExist = File.Exists(potentialFilePath);
            return File.Exists(potentialFilePath);
        }
        public void AddErrorMessage(ErrorCode errorCode)
        {
            bool errorAlreadyExists = false;
            foreach (AddProductError error in errorMessages)
            {
                if (errorCode == error.Code) errorAlreadyExists = true;
            }
            if (!errorAlreadyExists) errorMessages.Add(new AddProductError(errorCode));
        }
        public void RemoveErrorMessage(ErrorCode errorCode)
        {
            foreach (AddProductError error in errorMessages)
            {
                if (errorCode == error.Code)
                {
                    errorMessages.Remove(error);
                    break;
                }
            }
        }
        public void RemoveAllFileBasedErrors()
        {
            RemoveErrorMessage(ErrorCode.InvalidFileExtension);
            RemoveErrorMessage(ErrorCode.FileNotFound);
            RemoveErrorMessage(ErrorCode.DuplicateFileFound);
            RemoveErrorMessage(ErrorCode.ImageNameTooLong);
        }

        public void RemoveAllNameBasedErrors()
        {
            RemoveErrorMessage(ErrorCode.NameRequired);
            RemoveErrorMessage(ErrorCode.InvalidCharacterInName);
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

    }
}
