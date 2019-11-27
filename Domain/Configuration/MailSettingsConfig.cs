using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Configuration
{
    public class MailSettingsConfig: EntityTypeConfiguration<MailSettings>
    {
        public MailSettingsConfig()
        {
            HasKey(p => p.MailSettingsID);
            Property(p => p.MailSettingsID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
