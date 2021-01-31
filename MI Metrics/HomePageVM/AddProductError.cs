using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI_Metrics
{
    public enum ErrorCode
    {
        NameRequired = 0,
        InvalidFileExtension = 1,
        DuplicateFileFound = 2,
        FileNotFound = 3,
        InvalidCharacterInName = 4,
        ImageNameTooLong = 5
    }

    public class AddProductError
    {
        private ErrorCode code;
        private string message;
        private IconColor iconColor;

        public ErrorCode Code { get => code; set => code = value; }
        public string Message { get => message; set => message = value; }
        public IconColor IconColor { get => iconColor; set => iconColor = value; }

        public AddProductError(ErrorCode code)
        {
            this.Code = code;
            switch (code)
            {
                case ErrorCode.FileNotFound:
                    message = "The selected file does not exist or no file was selected.";
                    IconColor = IconColor.Red;
                    break;
                case ErrorCode.InvalidFileExtension:
                    message = "The chosen file has an unsupported file extension.";
                    IconColor = IconColor.Red;
                    break;
                case ErrorCode.DuplicateFileFound:
                    message = "An image file with this name already exists in the image directory.";
                    IconColor = IconColor.Red;
                    break;
                case ErrorCode.NameRequired:
                    message = "The System Name field must be filled out.";
                    IconColor = IconColor.Red;
                    break;
                case ErrorCode.InvalidCharacterInName:
                    message = "The System Name field contains an invalid character.";
                    IconColor = IconColor.Red;
                    break;
                case ErrorCode.ImageNameTooLong:
                    message = "The image file name should not be more than 100 characters long.";
                    IconColor = IconColor.Red;
                    break;
            }
        }

        
    }
}
