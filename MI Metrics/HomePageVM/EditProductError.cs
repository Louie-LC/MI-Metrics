using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public enum EditErrorCode
    {
        NameRequired = 0,
        InvalidFileExtension = 1,
        DuplicateFileFound = 2,
        FileNotFound = 3,
        InvalidCharacterInName = 4,
        NameChanged = 5,
        ImageChanged = 6,
        ImageNameTooLong = 7,
        SelectionRequired = 8
    }

    public class EditProductError
    {
        private EditErrorCode code;
        private string message;
        private IconColor iconColor;

        public EditErrorCode Code { get => code; set => code = value; }
        public string Message { get => message; set => message = value; }
        public IconColor IconColor { get => iconColor; set => iconColor = value; }

        public EditProductError(EditErrorCode code)
        {
            this.Code = code;
            switch (code)
            {
                case EditErrorCode.FileNotFound:
                    message = "The selected file does not exist or no file was selected.";
                    IconColor = IconColor.Red;
                    break;
                case EditErrorCode.InvalidFileExtension:
                    message = "The chosen file has an unsupported file extension.";
                    IconColor = IconColor.Red;
                    break;
                case EditErrorCode.DuplicateFileFound:
                    message = "An image file with this name already exists in the image directory.";
                    IconColor = IconColor.Red;
                    break;
                case EditErrorCode.NameRequired:
                    message = "The System Name field must be filled out.";
                    IconColor = IconColor.Red;
                    break;
                case EditErrorCode.InvalidCharacterInName:
                    message = "The System Name field contains an invalid character.";
                    IconColor = IconColor.Red;
                    break;
                case EditErrorCode.NameChanged:
                    message = "The system's name has been changed.";
                    IconColor = IconColor.Yellow;
                    break;
                case EditErrorCode.ImageChanged:
                    message = "The image for the system has been changed.";
                    IconColor = IconColor.Yellow;
                    break;
                case EditErrorCode.ImageNameTooLong:
                    message = "The image file name should not be more than 100 characters long.";
                    IconColor = IconColor.Red;
                    break;
                case EditErrorCode.SelectionRequired:
                    message = "Select a system for editing.";
                    IconColor = IconColor.Yellow;
                    break;
            }
        }


    }
}
