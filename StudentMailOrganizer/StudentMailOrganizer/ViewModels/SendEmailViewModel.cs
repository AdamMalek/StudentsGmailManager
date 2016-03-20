using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMailOrganizer.ViewModels
{
    public class SendEmailViewModel: INotifyPropertyChanged
    {

        string _receiver;
        public string Receiver
        {
            get
            {
                return _receiver;
            }
            set
            {
                _receiver = value;
                RaiseChange("IsValid");
            }
        }

        string _topic;
        public string Topic
        {
            get
            {
                return _topic;
            }
            set
            {
                _topic = value;
                RaiseChange("IsValid");
            }
        }
        string _body;
        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
                RaiseChange("IsValid");
            }
        }
        public bool IsValid
        {
            get
            {
                return isValid();
            }
        }

        EmailAddressAttribute email;
        RequiredAttribute required;
        StringLengthAttribute length;


        public SendEmailViewModel()
        {
            email = new EmailAddressAttribute();
            required = new RequiredAttribute();
            length = new StringLengthAttribute(30);
            length.MinimumLength = 5;
        }

        public bool isValid()
        {
            var isEmail = email.IsValid(Receiver);

            var isReqEmail = required.IsValid(email);
            var isReqTopic = required.IsValid(Topic);
            var isReqBody = required.IsValid(Body);

            var isLengTopic = length.IsValid(Topic);

            bool retval = isEmail && isReqEmail && isReqTopic && isReqBody && isLengTopic;

            return retval;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void RaiseChange(string propname)
        {
            var x = PropertyChanged;
            if (x != null) x(this, new PropertyChangedEventArgs(propname));
        }
    }
}
