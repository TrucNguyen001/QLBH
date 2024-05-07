﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.MISAAttribute
{
    /// <summary>
    /// Không được phép trống
    /// CreateBy: NVTruc(28/12/2023)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEmpty:Attribute
    {

    }

    /// <summary>
    /// Lấy tên 
    /// CreateBy: NVTruc(28/12/2023)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ProppertyName:Attribute
    {
        public string Name = string.Empty;
        public ProppertyName(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Validate date: Không cho nhập lớn hơn ngày hiện tại
    /// CreateBy: NVTruc(28/12/2023)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateDate:Attribute
    {

    }

    /// <summary>
    /// Dùng để lấy ra các atribute không cho nhập quá kí tự
    /// CreatedBy: Nguyễn Văn Trúc(20/3/2024)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLength:Attribute
    {
        public int Length = 0;
        public MaxLength(int length)
        {
            this.Length = length;
        }
    }

    /// <summary>
    /// Dùng để lấy ra các atribute xuất file
    /// CreatedBy: Nguyễn Văn Trúc(5/5/2024)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportFile : Attribute
    {
    }
}

