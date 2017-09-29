using AutoMapper;
using Img.Config.config;
using Img.Model.Models;
using Img.Model.PointNix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.DataService.Mappers
{
	class PointNixMapper:Profile
	{private int DefaultCategory = ConfigManager.Instance.GetConfig("PointNix").ImageCategory;

        public PointNixMapper()
        {
            CreateMap<ImageInfo, MedicalImage>()
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.PatientName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.CheckTime, opt => opt.MapFrom(src => src.CheckTime))
                .ForMember(dest => dest.VideoNum, opt => opt.MapFrom(src => src.VideoNum));
        }

        private Img.Model.Models.ImageCategory ToImageCategory(int imgType)
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
            return (Img.Model.Models.ImageCategory)temp;
        }
	}
}
