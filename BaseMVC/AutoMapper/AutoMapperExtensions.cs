using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace BaseMVC.AutoMapper
{
    public static class AutoMapperExtensions
    {
        public static TDestination Map<TDestination>(this object source)
        {
            if (source == null)
            {
                return default(TDestination);
            }

            return (TDestination)Mapper.Map(source, source.GetType(), typeof(TDestination));
        }
    }
}