using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Mappers;
using Img.Model.Models;
using Img.Model.Sidexis;

namespace Img.DataService.Mappers
{
    public class ImageListMapper : Profile
    {
        public ImageListMapper()
        {
            CreateMap<TRawDto, MedicalImage>()
                .ForMember(dest => dest.CheckTime, opt => opt.MapFrom(src => src.ImgCreateTime))
                .ForMember(dest=>dest.Category,opt=>opt.MapFrom(src=>src.Category))
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PrivateId))
                .ForMember(dest => dest.VideoNum, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.UploadStatus));
        }
    }
}
