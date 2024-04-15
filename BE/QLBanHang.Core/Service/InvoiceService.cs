using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using QLBanHang.Core.ValidateException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Service
{
    public class InvoiceService : BaseService<Invoice>, IInvoiceService
    {
        IInvoiceRepository _invoiceRepository;
        IDiscountRepository _discountRepository;
        public InvoiceService(IInvoiceRepository invoiceRepository, IDiscountRepository discountRepository):base(invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
            _discountRepository = discountRepository;
        }

        public string GetInvoiceCodeBiggest()
        {
            var invoices = _invoiceRepository.SortDecrease();
            var invoiceCode = "";
            if (invoices == null)
            {
                invoiceCode = "HD-00001";
            }
            else
            {
                Invoice invoice = invoices.FirstOrDefault();
                invoiceCode = invoice.InvoiceCode;
            }
            var numberCode = invoiceCode.Substring(3);
            var numberCodeBiggest = Convert.ToInt64(numberCode) + 1;
            numberCode = PadLeftCustom(Convert.ToString(numberCodeBiggest), numberCode.Length, '0');
            invoiceCode = invoiceCode.Substring(0, 3) + numberCode;
            return invoiceCode;
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

        public InvoiceDiscountDTOs Update(Guid id, Invoice entity)
        {
            var record = _invoiceRepository.getInvoiceDiscountId(id);

            if (record != null)
            {
                if (record.InputNumber == 0)
                {
                    throw new ValidateExceptionError("Mã giảm giá đã hết hạn. Vui lòng thử lại sau");
                }

                if (record.ExpirationDate <= DateTime.Now)
                {
                    throw new ValidateExceptionError("Mã giảm giá đã hết hạn. Vui lòng thử lại sau");
                }

                if (record.Minimum > entity.Total)
                {
                    throw new ValidateExceptionError("Mã áp dụng cho đơn hàng không hợp lệ vui lòng kiểm tra lại");
                }
            }

            return record;
        }
    }
}
