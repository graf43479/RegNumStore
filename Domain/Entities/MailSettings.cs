using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MailSettings
    {
        public string MailSettingsID { get; set; }

        public string SettingsType { get; set; }

        public string SettingsDesc { get; set; }

        public string SettingsValue { get; set; }
    }
}
