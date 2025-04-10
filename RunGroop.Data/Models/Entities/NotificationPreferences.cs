﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunGroop.Data.Models.Entities
{
    public class NotificationPreferences
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool NotifyMention { get; set; }
        public bool NotifyRequest { get; set; }
        public bool NotifyShare { get; set; }
        public bool NotifyMessage { get; set; }
        public bool NotifyAdds { get; set; }
        public bool NotifySales { get; set; }
    }

}
