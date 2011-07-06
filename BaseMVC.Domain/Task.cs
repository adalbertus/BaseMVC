using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMVC.Domain
{
    public class Task : Entity
    {
        public virtual string Title { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }
        public virtual User Owner { get; set; }
        public virtual Project Project { get; set; }

        public virtual int GetTotalSpendHours()
        {
            DateTime endTime = DateTime.Now;
            if (EndTime.HasValue)
            {
                endTime = EndTime.Value;
            }

            return (int)(endTime - StartTime).TotalHours;
        }
    }
}
