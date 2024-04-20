using QLBanHang.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Resource;
using MISA.AMISDemo.Core.Entities;
using System.ComponentModel;
using System.Drawing;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using OfficeOpenXml.Style;
using QLBanHang.Core.MISAEnum;
using QLBanHang.Core.MISAAttribute;
using QLBanHang.Core.ValidateException;

namespace QLBanHang.Core.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        IBaseRepository<T> _baseRepository;

        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public int Insert(T entity)
        {
            T record = RecordDTOs(entity);
            CheckValidateLength(record);
            CheckValidate(record);
            CheckValidateDate(record);
            //MISAValidate(entity, (int)Status.INSERT);
            return 1;
        }

        public int Update(T entity, Guid id)
        {
            CheckValidateLength(entity);
            CheckValidate(entity);
            CheckValidateDate(entity);
            MISAValidate(entity, (int)Status.UPDATE);
            return 1;
        }

        /// <summary>
        /// Validate những dữ liệu bắt buộc nhập
        /// </summary>
        /// <param name="entity">Entity kiểm tra</param>
        /// <exception cref="ValidateException.ValidateExceptionError"></exception>
        /// CreatedBy: NVTruc(31/3/2024)
        private void CheckValidate(T entity)
        {
            var propNoEmpties = typeof(T).GetProperties().Where(p => Attribute.IsDefined(p, typeof(NotEmpty)));

            foreach (var prop in propNoEmpties)
            {
                var propValue = prop.GetValue(entity);
                var propName = (prop.GetCustomAttributes(typeof(ProppertyName), true)[0] as ProppertyName).Name;

                var nameDisplay = (propName.Length == 0) ? prop.Name : propName;
                if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                {
                    throw new ValidateException.ValidateExceptionError($"{nameDisplay} {MISAResourceVN.InformationBlank}");
                }
            }
        }

        /// <summary>
        /// Kiểm tra validate date: Không nhập lớn hơn ngày hiện tại
        /// </summary>
        /// <param name="entity">Entity muốn kiểm tra</param>
        /// <exception cref="ValidateException.ValidateExceptionError"></exception>
        /// CreatedBy: NVTruc(31/3/2024)
        private void CheckValidateDate(T entity)
        {
            var propValidateDate = typeof(T).GetProperties().Where(p => Attribute.IsDefined(p, typeof(ValidateDate)));

            foreach (var prop in propValidateDate)
            {
                var propValue = prop.GetValue(entity);

                var propName = (prop.GetCustomAttributes(typeof(ProppertyName), true)[0] as ProppertyName).Name;

                var nameDisplay = (propName.Length == 0) ? prop.Name : propName;

                if (propValue != null)
                {
                    DateTime dateTime = (DateTime)propValue;
                    if (dateTime > DateTime.Now)
                    {
                        throw new ValidateException.ValidateExceptionError($"{nameDisplay} {MISAResourceVN.InformatonDate}");
                    }
                }
            }
        }

        /// <summary>
        /// Hàm kiểm tra chuỗi
        /// </summary>
        /// <param name="str">chuỗi truyền vào</param>
        /// <param name="totalWidth">Độ dài ban đầu</param>
        /// <param name="paddingChar">Kí tự thêm vào</param>
        /// <returns>
        /// Trả về chuỗi ban đầu nếu có độ dài >= độ dài ban đầu
        /// Trả về chuỗi thêm kí tự đằng trước nếu độ dài < độ dài ban đầu
        /// </returns>
        /// CreatedBy: NVTruc(1/4/2024)
        public string PadLeftCustom(string str, int totalWidth, char paddingChar)
        {
            if (str.Length >= totalWidth)
            {
                return str;
            }
            else
            {
                return new string(paddingChar, totalWidth - str.Length) + str;
            }
        }

        /// <summary>
        /// Hàm kiểm tra độ dài của của các thuộc tính
        /// </summary>
        /// <param name="entity">Bản ghi muốn kiểm tra</param>
        /// CreatedBy: NVTruc(20/3/2024)
        public void CheckValidateLength(T entity)
        {
            var propValidateDate = typeof(T).GetProperties().Where(p => Attribute.IsDefined(p, typeof(MaxLength)));

            foreach (var prop in propValidateDate)
            {
                var propValue = prop.GetValue(entity);

                var propLength = (prop.GetCustomAttributes(typeof(MaxLength), true)[0] as MaxLength).Length;

                if (propValue != null && propValue.ToString().Length > propLength)
                {
                    throw new ValidateException.ValidateExceptionError($"{prop} {MISAResourceVN.MaxLength} {propLength} {MISAResourceVN.Char}");
                }
            }
        }

        /// <summary>
        /// Kiểm tra validate của class cha cho class con bổ sung thêm
        /// </summary>
        /// <param name="entity">Entity muốn kiêm tra</param>
        /// <param name="status">Trạng thái bản ghi</param>
        /// CreatedBy: NVTruc(31/3/2024)
        protected virtual void MISAValidate(T entity, int status)
        {
        }

        public int GetPaging(int pageSize, int pageIndex, string? text)
        {
            if (pageSize < 1)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.PageSizeNotLessThan1);
            }
            if (pageIndex < 1)
            {
                throw new ValidateException.ValidateExceptionError(MISAResourceVN.PageIndexNotLessThan1);
            }
            return 1;
        }

        public FileExport ExportExcel<T>()
        {
            var list = _baseRepository.GetAll(1);
            var properties = typeof(T).GetProperties();

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Danh sách bản ghi");

            // Đặt các cấu hình cho các cột
            for (int i = 0; i < properties.Length; i++)
            {
                workSheet.Column(i + 1).Width = 20;
                workSheet.Column(i + 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[2, i + 1].Value = properties[i].Name;
            }

            using (var row = workSheet.Cells[MISAExportFileExcel.A1Q1])
            {
                row.Merge = true;
                row.Value = MISAExportFileExcel.TITLE;
                row.Style.Font.Bold = true;
                row.Style.Font.Size = 16;
                row.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // style cho excel.
            using (var row = workSheet.Cells[MISAExportFileExcel.A2Q2])
            {
                row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                row.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                row.Style.Font.Bold = true;
                row.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                row.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            int rowIndex = 3;
            // đổ dữ liệu từ list vào.
            foreach (var item in list)
            {
                int columnIndex = 1;
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item);
                    workSheet.Cells[rowIndex, columnIndex].Value = value != null ? value.ToString() : "";
                    columnIndex++;
                }
                rowIndex++;
            }

            // Lưu và trả về tệp Excel
            package.Save();
            stream.Position = 0;
            string excelName = MISAExportFileExcel.FILENAME;
            string excelType = MISAExportFileExcel.TYPE;
            return new FileExport(stream, excelType, excelName);
        }
        /// <summary>
        /// Kiểm tra validate của class cha cho class con bổ sung thêm
        /// </summary>
        /// <param name="entity">Entity muốn kiêm tra</param>
        /// <param name="status">Trạng thái bản ghi</param>
        /// CreatedBy: NVTruc(27/12/2023)
        protected virtual void ValidateCode(T entity, int status)
        {
        }

        /// <summary>
        /// Kiểm tra validate của class cha cho class con bổ sung thêm
        /// </summary>
        /// <param name="entity">Entity muốn kiêm tra</param>
        /// <param name="status">Trạng thái bản ghi</param>
        /// CreatedBy: NVTruc(27/12/2023)
        protected virtual T RecordDTOs(T entity)
        {
            return entity;
        }

    }
}
