using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Img.Model.Models;

namespace Img.DataService.Infrastructure
{
    public class DtoService<TSource, TDestination>
    {
        public TDestination ToDto(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public TSource ToDto(TDestination dest)
        {
            return Mapper.Map<TDestination,TSource>(dest);
        }

        public IEnumerable<TDestination> ToCollection(IEnumerable<TSource> source)
        {
            return Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
        }

        public IEnumerable<TSource> ToCollection(IEnumerable<TDestination> dest)
        {
            return Mapper.Map<IEnumerable<TDestination>, IEnumerable<TSource>>(dest);
        }
    }
}
