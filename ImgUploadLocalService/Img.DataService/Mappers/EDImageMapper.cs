using AutoMapper;
using Img.Config.config;
using Img.Model.EasyDent;
using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.DataService.Mappers
{
    public class EDImageMapper : Profile
    {
        private int DefaultCategory = ConfigManager.Instance.GetConfig("EasyDent").ImageCategory;

        public EDImageMapper()
        {
            CreateMap<EDImage, MedicalImage>()
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Injek.Chart))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Injek.PName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => ToImageCategory(src.C_Type)))
                .ForMember(dest => dest.CheckTime, opt => opt.MapFrom(src => src.C_Date))
                .ForMember(dest => dest.VideoNum, opt => opt.MapFrom(src => src.Seq));
        }

        private ImageCategory ToImageCategory(int imgType)
        {
            int temp = Convert.ToInt32(DefaultCategory);
            if (imgType == 3)
            {
                temp = 4;
            }
            else if (imgType == 4)
            {
                temp = 1;
            }
            return (ImageCategory)temp;
        }
    }
}
