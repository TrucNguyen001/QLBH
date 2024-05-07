﻿using AutoMapper;
using MISA.AMISDemo.Core.Entities;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using QLBanHang.Core.Resource;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanHang.Core.MISAEnum;
using QLBanHang.Core.MISAAttribute;
using QLBanHang.Core.ValidateException;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Runtime.Caching;

namespace QLBanHang.Core.Service
{
    public class ProductService : IProductService
    {
        IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public string GetProductCodeBiggest()
        {
            var products = _productRepository.SortDecrease();
            Product product = products.FirstOrDefault();
            var productCode = product.ProductCode;
            var numberCode = productCode.Substring(3);
            var numberCodeBiggest = Convert.ToInt64(numberCode) + 1;
            numberCode = PadLeftCustom(Convert.ToString(numberCodeBiggest), numberCode.Length, '0');
            productCode = productCode.Substring(0, 3) + numberCode;
            return productCode;
        }

        public int Insert(ProductDTOs record)
        {
            CheckValidateLength(record);
            CheckValidate(record);
            CheckValidateDate(record);
            //MISAValidate(entity, (int)Status.INSERT);
            return 1;
        }

        public int Update(ProductDTOs entity, Guid id)
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
        private void CheckValidate(ProductDTOs entity)
        {
            var propNoEmpties = typeof(Product).GetProperties().Where(p => Attribute.IsDefined(p, typeof(NotEmpty)));

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
        private void CheckValidateDate(ProductDTOs entity)
        {
            var propValidateDate = typeof(Product).GetProperties().Where(p => Attribute.IsDefined(p, typeof(ValidateDate)));

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
        public void CheckValidateLength(ProductDTOs entity)
        {
            var propValidateDate = typeof(Product).GetProperties().Where(p => Attribute.IsDefined(p, typeof(MaxLength)));

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
        protected virtual void MISAValidate(ProductDTOs entity, int status)
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

        public FileExport ExportExcel()
        {
            var list = _productRepository.GetAll();
            var properties = typeof(ProductDTOs).GetProperties().Where(p => Attribute.IsDefined(p, typeof(ExportFile)));

            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("Danh sách bản ghi");

            int i = 0;
            foreach (var property in properties)
            {
                var propName = (property.GetCustomAttributes(typeof(ProppertyName), true)[0] as ProppertyName).Name;
                workSheet.Column(i + 1).Width = 20;
                workSheet.Column(i + 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[2, i + 1].Value = propName;
                i++;
            }

            using (var row = workSheet.Cells[MISAExportFileExcel.A1AH1])
            {
                row.Merge = true;
                row.Value = MISAExportFileExcel.TITLE;
                row.Style.Font.Bold = true;
                row.Style.Font.Size = 16;
                row.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // style cho excel.
            using (var row = workSheet.Cells[MISAExportFileExcel.A1AH2])
            {
                row.Style.Fill.PatternType = ExcelFillStyle.Solid;
                row.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                row.Style.Font.Bold = true;
                row.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                row.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            workSheet.Cells["E"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

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

            // Auto fit columns
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

            // Lưu và trả về tệp Excel
            package.Save();
            stream.Position = 0;
            string excelName = MISAExportFileExcel.FILENAME;
            string excelType = MISAExportFileExcel.TYPE;
            return new FileExport(stream, excelType, excelName);
        }

        /// <summary>
        /// Kiểmm tra file Import
        /// </summary>
        /// <param name="fileImport">File Import</param>
        /// <exception cref="NotImplementedException"></exception>
        /// CreatedBy: NVTruc(16/1/2024)
        public void CheckFileImport(IFormFile fileImport)
        {
            if (fileImport == null || fileImport.Length <= 0)
            {
                throw new ValidateExceptionError(MISAResourceVN.FileImportNotNull);
            }

            if (!Path.GetExtension(fileImport.FileName).Equals(MISAResourceVN.XLSX, StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidateExceptionError(MISAResourceVN.FileImportMalformed);
            }
        }


        /// <summary>
        /// Hàm import file
        /// </summary>
        /// <param name="fileImport">file import</param>
        ///  <param name="isCommit" Check xem có commit hay không> </param>
        /// <returns>Trả về danh sách nhân viên import</returns>
        /// CreatedBy: NVTruc(9/1/2024)
        public IEnumerable<ProductDTOs> ImportProduct(bool isCommit, IFormFile fileImport)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            CheckFileImport(fileImport);
            var products = new List<ProductDTOs>();

            // Kiểm tra xem dữ liệu lưu trong cache chưa
            var cache = MemoryCache.Default;
            var cachedProducts = cache["CachedProducts"] as List<ProductDTOs>;
            if (cachedProducts != null)
            {
                if (isCommit)
                {
                    List<ProductDTOs> importedProducts = cachedProducts.Where(product => product.IsImported == true).ToList();
                    List<Product> productMap = _mapper.Map<List<ProductDTOs>, List<Product>>(importedProducts);
                    foreach (var product in productMap)
                    {
                        product.CreatedDate = DateTime.Now;
                    }
                    var result = _productRepository.MultiplePost(productMap);
                    return cachedProducts;
                }
            }

            using (var stream = new MemoryStream())
            {
                // Copy tệp vào Stream
                fileImport.CopyToAsync(stream);

                //Thực hiện đọc dữ liệu
                using (var package = new ExcelPackage(stream))
                {
                    // Sheet đọc dữ liệu:
                    var currentSheet = package.Workbook.Worksheets;
                    ExcelWorksheet worksheet = currentSheet.FirstOrDefault();
                    if (worksheet != null)
                    {
                    //    string[] arr = { MISAResourceVN.ProductCode, MISAResourceVN.ProductName, MISAResourceVN.Gender, MISAResourceVN.Birth,
                    //MISAResourceVN.Position, MISAResourceVN.Department, MISAResourceVN.PhoneNumber, MISAResourceVN.CMND,
                    //MISAResourceVN.BankName };
                        // Tổng số dòng dữ liệu:
                        var rowCount = worksheet.Dimension.Rows;
                        var checkHeader = true;
                        int i = 0;
                        //foreach (var item in arr)
                        //{
                        //    if (item != worksheet?.Cells[2, i + 2]?.Value?.ToString()?.Trim())
                        //    {
                        //        checkHeader = false;
                        //    }
                        //    i++;
                        //}

                        //if (!checkHeader)
                        //{
                        //    throw new MISAValidateException(MISAResourceVN.FileNotValid);
                        //}

                        // Bắt đầu đọc dữ liệu 
                        for (int row = 3; row <= rowCount; row++)
                        {
                            var productImport = new ProductDTOs();

                            //Kiểm tra mã nhân viên
                            var productCode = worksheet?.Cells[row, 2]?.Value?.ToString()?.Trim();
                            if (string.IsNullOrEmpty(productCode))
                            {
                                productImport.ImportInvalidErrors.Add("Mã sản ");
                            }
                            else
                            {
                                if (!CheckProductCode(productCode, products))
                                {
                                    productImport.ImportInvalidErrors.Add(MISAExportFileExcel.ProductCodeDuplicate);
                                }
                                if (!CheckProductCodeNotChar(productCode))
                                {
                                    productImport.ImportInvalidErrors.Add(MISAResourceVN.ProductCodeNotChar);
                                }
                            }

                            // Họ tên
                            var fullName = worksheet?.Cells[row, 3]?.Value?.ToString()?.Trim();
                            if (string.IsNullOrEmpty(fullName))
                            {
                                productImport.ImportInvalidErrors.Add(MISAExportFileExcel.ProductNameNotBlank);
                            }

                            // Kiểm tra chức danh
                            var positionName = worksheet.Cells[row, 6]?.Value?.ToString()?.Trim();
                            positionName = (positionName == "") ? null : positionName;
                            if (positionName != null)
                            {
                                var position = _positionRepository.GetByName(positionName);
                                if (position == null)
                                {
                                    productImport.ImportInvalidErrors.Add(MISAResourceVN.PostionNotExist);
                                }
                                else
                                {
                                    productImport.PositionId = position.PositionId;
                                }
                            }
                            else
                            {
                                productImport.PositionId = null;
                                productImport.ImportInvalidErrors.Add(MISAResourceVN.PositionNotEmpty);
                            }

                            // Kiểm tra chức vụ
                            var departmentName = worksheet.Cells[row, 7]?.Value?.ToString()?.Trim();
                            if (departmentName != null)
                            {
                                var department = _departmentRepository.GetByName(departmentName);
                                if (department == null)
                                {
                                    productImport.ImportInvalidErrors.Add(MISAResourceVN.DepartmentExist);
                                }
                                else
                                {
                                    productImport.DepartmentId = department.DepartmentId;
                                }
                            }
                            else
                            {
                                productImport.DepartmentId = null;
                                productImport.ImportInvalidErrors.Add(MISAResourceVN.DepartmentNotEmpty);
                            }

                            // Kiểm tra số điện thoại(File Excel anh gửi để import product không có cột SĐT. Em xin phép thay cột Số tài khoản thành Số điện thoại để test chức năng)
                            var phoneNumber = worksheet.Cells[row, 8]?.Value?.ToString()?.Trim();
                            phoneNumber = phoneNumber == null ? "" : phoneNumber.Trim();
                            if (phoneNumber.Length > 0)
                            {
                                if (!CheckPhoneNumber(phoneNumber, products))
                                {
                                    productImport.ImportInvalidErrors.Add(MISAExportFileExcel.PhoneNumberDuplicate);
                                }
                            }

                            var genderName = worksheet.Cells[row, 4]?.Value?.ToString()?.Trim();
                            Gender gender = (Gender)((genderName == MISAResourceVN.MALE) ? 0 : (genderName == MISAResourceVN.FEMALE) ? 1 : 2);

                            var dob = worksheet.Cells[row, 5]?.Value?.ToString()?.Trim();
                            if (!string.IsNullOrEmpty(dob))
                            {
                                if (ConvertDateTime(dob) == null)
                                {
                                    productImport.ImportInvalidErrors.Add(MISAExportFileExcel.DateOfBirthNotValid);
                                }
                                else if (ConvertDateTime(dob) > DateTime.Now)
                                {
                                    productImport.ImportInvalidErrors.Add(MISAExportFileExcel.DateOfBirthNotCurrentDate);
                                }
                            }

                            productImport.ProductId = Guid.NewGuid();
                            productImport.ProductCode = productCode;
                            productImport.FullName = fullName;
                            productImport.Gender = gender != null ? gender : null;
                            productImport.DateOfBirth = ConvertDateTime(dob);
                            productImport.PositionName = positionName;
                            productImport.DepartmentName = departmentName;
                            productImport.PhoneNumber = phoneNumber;
                            productImport.IdentificationCard = worksheet.Cells[row, 9]?.Value?.ToString()?.Trim();
                            productImport.BankName = worksheet.Cells[row, 10]?.Value?.ToString()?.Trim();




                            // Kiểm tra trùng mã
                            var isAlreadyExistProductCode = _productRepository.CheckDuplicateCode(productImport.ProductCode);

                            // Kiểm tra trùng số điện thoại
                            if (productImport.PhoneNumber.Length > 0)
                            {
                                var isAlreadyExistPhoneNumber = _productRepository.CheckDuplicatePhoneNumber(productImport.PhoneNumber);
                                if (isAlreadyExistPhoneNumber)
                                {
                                    productImport.ImportInvalidErrors.Add(MISAExportFileExcel.PhoneNumberAlreadyExist);
                                }
                            }

                            if (isAlreadyExistProductCode)
                            {
                                productImport.ImportInvalidErrors.Add(MISAExportFileExcel.ProductCodeAlreadyExist);
                            }

                            if (productImport.ImportInvalidErrors.Count() == 0)
                            {
                                productImport.IsImported = true;
                                productImport.ImportInvalidErrors.Add(MISAExportFileExcel.RecordSuccess);
                            }
                            products.Add(productImport);
                        }
                    }
                }

                // Lưu danh sách nhân viên vào memory cache
                cache.Set(MISAResourceVN.CachedProducts, products, DateTimeOffset.UtcNow.AddHours(1));
            }
            return products;
        }

        //public void CheckFileImport(IFr)

        /// <summary>
        /// Hàm chuyển đổi string thành datetime
        /// </summary>
        /// <param name="date">chuỗi chuyển đổi</param>
        /// <returns>Trả về datetinme sau khi chuyển đổi</returns>
        /// CreatedBy: NVTruc(30/1/2024)
        public DateTime? ConvertDateTime(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                // Trả về null nếu chuỗi rỗng
                return null;
            }

            // Kiểm tra nếu chuỗi chứa ngày/tháng/năm
            if (Regex.IsMatch(date, @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$"))
            {
                // Trả về ngày/tháng/năm dưới dạng dd/MM/yyyy
                return DateTime.ParseExact(date, MISAResourceVN.FormatDate, null);
            }

            // Kiểm tra nếu chuỗi chứa tháng/năm
            if (Regex.IsMatch(date, @"^(0[1-9]|1[0-2])/\d{4}$"))
            {
                // Trả về ngày đầu tháng dưới dạng dd/MM/yyyy
                DateTime ngaySinh = DateTime.ParseExact(date, MISAResourceVN.FormatMonthYear, null);
                return new DateTime(ngaySinh.Year, ngaySinh.Month, 1);
            }

            // Kiểm tra nếu chuỗi chỉ chứa năm
            if (Regex.IsMatch(date, @"^\d{4}$") && int.TryParse(date, out int nam))
            {
                // Trả về ngày 01/01/năm dưới dạng dd/MM/yyyy
                return new DateTime(nam, 1, 1);
            }
            return null;
        }

        /// <summary>
        /// Kiểm tra xem mã nhân viên đã có trong danh sách thêm hay chưa
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên</param>
        /// <param name="list">Danh sách thêm</param>
        /// <returns>
        /// false: Đã tồn tại trong danh sách
        /// true: Chưa tồn tại trong danh sách
        /// </returns>
        /// CreatedBy: NVTruc(31/1/2024)
        public bool CheckEmployeeCode(string employeeCode, IEnumerable<Employee> list)
        {
            foreach (var employee in list)
            {
                if (employeeCode == employee.EmployeeCode)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Kiểm tra validate của class cha cho class con bổ sung thêm
        /// </summary>
        /// <param name="entity">Entity muốn kiêm tra</param>
        /// <param name="status">Trạng thái bản ghi</param>
        /// CreatedBy: NVTruc(27/12/2023)
        protected virtual void ValidateCode(Product entity, int status)
        {
        }

        public int GetPagingUser(int pageIndex, string text, int optionSelect, string productTypeId)
        {
            throw new NotImplementedException();
        }
    }
}
