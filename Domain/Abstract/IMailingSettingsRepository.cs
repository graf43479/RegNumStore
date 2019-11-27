using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IMailingSettingsRepository
    {
        IQueryable<MailSettings> MailSettingses { get;  }

        void SaveDimSetting(MailSettings mailSettings, bool create);

        void DeleteMailSettings(MailSettings mailSettings);
    }
}
