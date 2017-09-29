using Img.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Img.DataService.Infrastructure;
using Img.Model.Dtos;
using Img.Model.Search;

namespace Img.DataService
{
    public interface IDataService
    {
        string Name { get;}

        void Upload(IEnumerable<MedicalImage> imageList);
        Page<MedicalImage> GenerateImageList(ImageSearch search);
        IEnumerable<MedicalImage> GenerateImageList();
    }
}
