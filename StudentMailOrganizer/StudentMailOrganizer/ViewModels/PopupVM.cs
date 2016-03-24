using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentMailOrganizer.ViewModels
{
    public class PopupVM: INotifyPropertyChanged
    {
        public string PopupLabel { get; set; }
        public string PopupText { get; set; }
        public TextType TextType { get; set; }
        EmailAddressAttribute validator = new EmailAddressAttribute();
        RegularExpressionAttribute timeValidator = new RegularExpressionAttribute("([01]?[0-9]|2[0-3]):[0-5][0-9]");
        
        public event PropertyChangedEventHandler PropertyChanged;
        void RaiseChange(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool isValid()
        {
            if (TextType == TextType.Email)
            {
                if (!validator.IsValid(PopupText))
                {
                    PopupText = "";
                    RaiseChange("PopupText");
                    return false;
                }
            }
            else if (TextType == TextType.Time)
            {
                if (!timeValidator.IsValid(PopupText))
                {
                    PopupText = "";
                    RaiseChange("PopupText");
                    return false;
                }
            }
            return PopupText!=null && PopupText.Trim().Length > 0;
        }
    }

    public enum TextType
    {
        Email,
        Text,
        Time
    };
}
