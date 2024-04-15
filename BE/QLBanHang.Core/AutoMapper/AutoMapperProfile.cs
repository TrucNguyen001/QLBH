using AutoMapper;
using MISA.AMISDemo.Core.Entities;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        #region Constructor
        public AutoMapperProfile() {
            CreateMap<ProductDTOs, Product>();
            CreateMap<ProductDTOs, Configuration>();
        }
        #endregion
    }
}
