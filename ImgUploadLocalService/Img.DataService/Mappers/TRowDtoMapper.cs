using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.Model.Models;
using Img.Model.Sidexis;

namespace Img.DataService.Mappers
{
    public class TRowDtoMapper:Profile
    {
        public TRowDtoMapper()
        {
            CreateMap<MedicalImage, TRawDto>()
                .ForMember(s => s.ImageId, opt => opt.MapFrom(dest => dest.VideoNum))
                .ForMember(s=>s.Category,opt=>opt.MapFrom(dest=>dest.Category))
                .ForMember(s => s.ImgCreateTime, opt => opt.MapFrom(dest => dest.CheckTime))
                .ForMember(s => s.PrivateId, opt => opt.MapFrom(dest => dest.PatientId))
                .ForMember(s => s.UploadStatus, opt => opt.MapFrom(dest => dest.Status));
        }
    }
}
