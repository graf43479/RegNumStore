using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFMailingSettingRepository : IMailingSettingsRepository
    {
         private RegNumDBContext context;

         public EFMailingSettingRepository(RegNumDBContext context)
        {
            this.context = context;
        }

         public IQueryable<MailSettings> MailSettingses
         {
             get { return context.MailSettingses; } 
            }


        public void SaveDimSetting(MailSettings mailSettings, bool create)
        {
            if (create)
            {
                //context.DimSettings.Add(dimSetting);
                //context.Entry(dimSetting).State = EntityState.Added;
                //var tmp = context.DimSettingsTypes.Where(x => x.SettingTypeID == dimSetting.SettingsTypeID);

                //context.DimSettings.Add(dimSetting);
                context.MailSettingses.Add(mailSettings);
                //context.Entry(dimSetting).State = EntityState.Added;
                context.SaveChanges();
            }
            else
            {
                MailSettings ds = context.MailSettingses.FirstOrDefault(x => x.MailSettingsID == mailSettings.MailSettingsID);
                ds.MailSettingsID = mailSettings.MailSettingsID;
                ds.SettingsDesc = mailSettings.SettingsDesc;
                ds.SettingsValue = mailSettings.SettingsValue;
                context.Entry(ds).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteMailSettings(MailSettings mailSettings)
        {
            context.MailSettingses.Remove(mailSettings);
            context.SaveChanges();
        }
    
    }
}
