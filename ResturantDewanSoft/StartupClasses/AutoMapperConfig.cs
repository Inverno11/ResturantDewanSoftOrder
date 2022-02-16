using AutoMapper;
using ResturantDewanSoft.Models.DataBaseModels;
using ResturantDewanSoft.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResturantDewanSoft.StartupClasses
{
    public static class AutoMapperConfig
    {

        public static IMapper Mapper { set; get; }
            
        public static void Init()
        {

            MapperConfiguration config = new MapperConfiguration( conf=>conf.CreateMap<Item,ItemModel>().ForMember(des=>des.No,src=>src.MapFrom(i=>i.Id)).ReverseMap());

            Mapper = config.CreateMapper();

        }




    }
}
