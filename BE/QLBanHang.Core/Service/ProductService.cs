using AutoMapper;
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
using Microsoft.IdentityModel.Tokens;

namespace QLBanHang.Core.Service
{
    public class ProductService : IProductService
    {
        IProductRepository _productRepository;
        IProductTypeRepository _productTypeRepository;
        ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper, IProductTypeRepository productTypeRepository, ISupplierRepository supplierRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productTypeRepository = productTypeRepository;
            _supplierRepository = supplierRepository;
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
        /// <param name = "fileImport" > file import</param>
        ///  <param name = "isCommit" Check xem có commit hay không> </param>
        /// <returns>Trả về danh sách nhân viên import</returns>
        /// CreatedBy: NVTruc(9/1/2024)
        public List<ProductDTOs> ImportExcel(bool isCommit, IFormFile fileImport)
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
                        product.ProductId = Guid.NewGuid();
                        product.CreatedDate = DateTime.Now;
                        product.Status = 1;
                        product.PriceBuy = product.Price - product.PriceReduced;
                        product.CreatedBy = "NVTruc";
                        product.QuantityBuy = 0;
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
                        string[] arr = { "Mã sản phẩm", "Tên sản phẩm", "Avatar", "Số lượng", "Giá", "Giá giảm", "Mã nhà cung cấp", "Mã loại sản phẩm", "Độ hot" };
                        // Tổng số dòng dữ liệu:
                        var rowCount = worksheet.Dimension.Rows;
                        var checkHeader = true;
                        int i = 0;
                        foreach (var item in arr)
                        {
                            if (item != worksheet?.Cells[2, i + 2]?.Value?.ToString()?.Trim())
                            {
                                checkHeader = false;
                            }
                            i++;
                        }

                        if (!checkHeader)
                        {
                            throw new ValidateExceptionError(MISAResourceVN.FileNotValid);
                        }

                        // Bắt đầu đọc dữ liệu 
                        for (int row = 3; row <= rowCount; row++)
                        {
                            var productImport = new ProductDTOs();

                            //----------Kiểm tra mã sản phẩm------------
                            var productCode = worksheet?.Cells[row, 2]?.Value?.ToString()?.Trim();
                            if (string.IsNullOrEmpty(productCode))
                            {
                                productImport.ImportInvalidErrors.Add("Mã sản không được phép bỏ trống!");
                            }
                            else
                            {
                                if (!CheckProductCode(productCode, products))
                                {
                                    productImport.ImportInvalidErrors.Add("Mã sản phẩm đã trùng với mã trong file");
                                }
                                if (!CheckProductCodePrefix(productCode))
                                {
                                    productImport.ImportInvalidErrors.Add("Mã sản phẩm không đúng định dạng");
                                }
                            }

                            // Tên sản phẩm
                            var productName = worksheet?.Cells[row, 3]?.Value?.ToString()?.Trim();
                            if (string.IsNullOrEmpty(productName))
                            {
                                productImport.ImportInvalidErrors.Add("Tên sản phẩm không được phép bỏ trống");
                            }

                            // ------------Avatar-----------------
                            var avatar = worksheet.Cells[row, 4]?.Value?.ToString()?.Trim();
                            if (string.IsNullOrEmpty(avatar))
                            {
                                productImport.ImportInvalidErrors.Add("Avatar không được phép bỏ trống");
                                avatar = "error_img.png";
                            }
                            else
                            {
                                if(!IsImageFileExtension(avatar))
                                {
                                    productImport.ImportInvalidErrors.Add("File ảnh không đúng định dạng");
                                    avatar = "error_img.png";
                                }
                            }

                            //  -----------------Quanntity-------------------
                            var quantity = Convert.ToInt32(worksheet.Cells[row, 5]?.Value?.ToString()?.Trim());
                            if (string.IsNullOrEmpty(quantity.ToString()))
                            {
                                productImport.ImportInvalidErrors.Add("Số lượng không được phép bỏ trống");
                            }
                            else
                            {
                                if (quantity <= 0)
                                {
                                    productImport.ImportInvalidErrors.Add("Số lượng không đúng định dạng");
                                }
                            }

                            // ------------ Price---------------
                            var price = Convert.ToDecimal(worksheet.Cells[row, 6]?.Value?.ToString()?.Trim());

                            if (string.IsNullOrEmpty(price.ToString())) {
                                productImport.ImportInvalidErrors.Add("Giá bán không được phép bỏ trống");
                            }
                            else
                            {
                                if (price <= 0)
                                {
                                    productImport.ImportInvalidErrors.Add("Giá bán không đúng định dạng");
                                }
                            }

                            // ------------------PriceReduced------------------
                            var priceReduced = Convert.ToDecimal(worksheet.Cells[row, 7]?.Value?.ToString()?.Trim());
                            if (!string.IsNullOrEmpty(priceReduced.ToString())) {
                                if (priceReduced <= 0)
                                {
                                    productImport.ImportInvalidErrors.Add("Giá giảm không đúng định dạng");
                                }
                                else if (priceReduced > price && price > 0)
                                {
                                    productImport.ImportInvalidErrors.Add("Giá giảm không được lớn hơn giá bán");
                                }

                            }

                            // ----------------Kiểm tra Loại sản phẩm--------------------
                            var productTypeId = worksheet.Cells[row, 9]?.Value?.ToString()?.Trim();
                            productTypeId = (productTypeId == "") ? null : productTypeId;
                            if (productTypeId != null)
                            {
                                var productType = _productTypeRepository.GetById(Guid.Parse(productTypeId));
                                if (productType == null)
                                {
                                    productImport.ImportInvalidErrors.Add("Không tim thấy loại sản phẩm");
                                }
                                else
                                {
                                    productImport.ProductTypeName = productType.ProductTypeName;
                                    productImport.ProductTypeId = productType.ProductTypeId;
                                }
                            }
                            else
                            {
                                productImport.ImportInvalidErrors.Add("Loại sản phẩm không được phép bỏ trống");
                            }

                            // ----------------Kiểm tra nhà cung cấp sản phẩm--------------------
                            var supplierId = worksheet.Cells[row, 8]?.Value?.ToString()?.Trim();
                            supplierId = (supplierId == "") ? null : supplierId;
                            if (supplierId != null)
                            {
                                var supplier = _supplierRepository.GetById(Guid.Parse(supplierId));
                                if (supplier == null)
                                {
                                    productImport.ImportInvalidErrors.Add("Không tim thấy nhà cung cấp");
                                }
                                else
                                {
                                    productImport.SupplierName = supplier.SupplierName;
                                    productImport.SupplierId = supplier.SupplierId;
                                }
                            }
                            else
                            {
                                productImport.ImportInvalidErrors.Add("Nhà cung cấp không được phép bỏ trống");
                            }

                            //-----------------Hot---------------------
                            var hot = Convert.ToInt32(worksheet.Cells[row, 10]?.Value?.ToString()?.Trim());

                            if (string.IsNullOrEmpty(hot.ToString()))
                            {
                                productImport.ImportInvalidErrors.Add("Độ không được phép bỏ trống");
                            }
                            else
                            {
                                if (hot <= 0)
                                {
                                    productImport.ImportInvalidErrors.Add("Độ hot không đúng định dạng");
                                }
                            }

                            productImport.ProductId = Guid.NewGuid();
                            productImport.ProductCode = productCode;
                            productImport.ProductName = productName;
                            productImport.Avatar = avatar;
                            productImport.Quantity = quantity;
                            productImport.Price = price;
                            productImport.PriceReduced = priceReduced;
                            productImport.Hot = hot;




                            // Kiểm tra trùng mã
                            var isAlreadyExistProductCode = _productRepository.CheckDuplicateCode(productImport.ProductCode);

                            if (isAlreadyExistProductCode)
                            {
                                productImport.ImportInvalidErrors.Add("Mã sản phẩm đã tồn tại");
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
                cache.Set("CachedProducts", products, DateTimeOffset.UtcNow.AddHours(1));
            }
            return products;
        }


        /// <summary>
        /// Kiểm tra xem mã sản phẩm đã có trong danh sách thêm hay chưa
        /// </summary>
        /// <param name="employeeCode">Mã sản phẩm</param>
        /// <param name="list">Danh sách thêm</param>
        /// <returns>
        /// false: Đã tồn tại trong danh sách
        /// true: Chưa tồn tại trong danh sách
        /// </returns>
        public bool CheckProductCode(string productCode, IEnumerable<ProductDTOs> list)
        {
            foreach (var produc in list)
            {
                if (productCode == produc.ProductCode)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra tiền tố của mã sản phẩm
        /// </summary>
        /// <param name="productCode">Mã sản phẩm</param>
        /// <returns>
        /// false: Mã sản phẩm < 4 ký tự hoặc không bắt đầu bằng SP-
        /// true: Đúng định dạng yêu cầu
        /// </returns>
        public bool CheckProductCodePrefix(string productCode)
        {
            if (productCode.Length <= 3 || productCode.Substring(0, 3) != "SP-")
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra có phải file ảnh không
        /// </summary>
        /// <param name="filePath">ảnh</param>
        /// <returns>
        /// true: đúng
        /// false: sai
        /// </returns>
        static bool IsImageFileExtension(string filePath)
        {
            // Lấy phần mở rộng của file
            string extension = Path.GetExtension(filePath)?.ToLower();

            // Danh sách các đuôi file ảnh hợp lệ
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".ico" };

            // Kiểm tra xem phần mở rộng có nằm trong danh sách các đuôi file ảnh hợp lệ không
            return Array.Exists(validExtensions, ext => ext == extension);
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
