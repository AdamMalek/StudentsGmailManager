using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMailOrganizer.ViewModels
{
    public class PopupVM
    {
        public string PopupLabel { get; set; }
        public string PopupText { get; set; }
        public TextType TextType { get; set; }
        EmailAddressAttribute validator = new EmailAddressAttribute();

        public bool isValid()
        {
            if (TextType == TextType.Email)
            {
                if (!validator.IsValid(PopupText)) return false;
            }
            return PopupText.Trim().Length > 0;
        }
    }

    public enum TextType
    {
        Email,
        Text
    };
}
