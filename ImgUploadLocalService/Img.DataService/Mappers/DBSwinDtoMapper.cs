using AutoMapper;
using Img.Model.DBSwin;
using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Img.DataService.Mappers
{
	public class DBSwinDtoMapper:Profile
	{
		public DBSwinDtoMapper()
        {
			CreateMap<DBSwinInfo, MedicalImage>()
				.ForMember(s => s.VideoNum, opt => opt.MapFrom(dest => dest.Vorgnr))
				.ForMember(s => s.Category, opt => opt.MapFrom(dest => dest.Category))
				.ForMember(s => s.CheckTime, opt => opt.MapFrom(dest => dest.CheckTime))
				.ForMember(s => s.PatientId, opt => opt.MapFrom(dest => dest.PrivateId))
				.ForMember(s => s.Status, opt => opt.MapFrom(dest => dest.Status))
				.ForMember(s => s.PatientName, opt => opt.MapFrom(dest => dest.PatientName))
				.ForMember(s => s.FilePath, opt => opt.MapFrom(dest => dest.ImgFile));
        }
	}
}
