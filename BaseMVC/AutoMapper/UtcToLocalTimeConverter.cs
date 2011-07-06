using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseMVC.Extensions;
using AutoMapper;

namespace BaseMVC.AutoMapper
{
    public class UtcToLocalTimeConverter : TypeConverter<DateTime, DateTime>
    {
        protected override DateTime ConvertCore(DateTime source)
        {
            //var timeZones = TimeZoneInfo.GetSystemTimeZones();
            string targetTimeZoneName = "Central European Standard Time";
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(targetTimeZoneName);
            return TimeZoneInfo.ConvertTimeFromUtc(date, targetTimeZone);
        }
    }
}