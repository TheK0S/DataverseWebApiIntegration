using System.Collections.Generic;
using System.Windows.Documents;

namespace DataverseWebApiIntegration.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Telephone1 { get; set; }
        public string Websiteurl { get; set; }
        public string Address1_line1 { get; set; }
        public string Address1_line2 { get; set; }
        public string Address1_city { get; set; }
        public string Address1_postalcode { get; set; }
        public string Address1_stateorprovince { get; set; }
        public List<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
