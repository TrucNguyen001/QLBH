using Dapper;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.DBContext;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Infrastructure.Repository
{
    public class CartRepository : ICartRepository
    {
        IDbContext _dbContext;
        IInvoiceService _invoiceService;

        public CartRepository(IDbContext dbContext, IInvoiceService invoiceService)
        {
            _dbContext = dbContext;
            _invoiceService = invoiceService;
        }

        public int Delete(Guid id, Guid accountId)
        {
            // Kiểm tra giỏ hàng còn sản phẩm nào không. Nếu còn 1 sản phẩm nào thì xoá hoá đơn
            var recordCart = GetAllById(accountId.ToString(), "");
            Guid invoiceId = (Guid)recordCart.FirstOrDefault().InvoiceId;

            var sqlCommand = $"DELETE FROM Cart WHERE CartId = @entityId AND AccountId = @accountId AND Status = 1";

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("@entityId", id);
            paramet.Add("@accountId", accountId);

            _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);


            if(recordCart.Count() == 1)
            {
                var sqlDelete = "DELETE FROM Invoice WHERE InvoiceId = @InvoiceId AND Status = 0";
                DynamicParameters parametDelete = new DynamicParameters();

                parametDelete.Add("@InvoiceId", invoiceId);

                _dbContext.Connection.Execute(sql: sqlDelete, param: parametDelete);
            }
            
            return 1;
        }

        public IEnumerable<CartDTOs> GetAllById(string id, string text)
        {
            var sqlCommand = "Proc_CartInvoiceUser";
            DynamicParameters paramt = new DynamicParameters();

            paramt.Add("accountId", id);
            paramt.Add("text", text);

            var entities = _dbContext.Connection.Query<CartDTOs>(sql: sqlCommand, param: paramt, commandType: System.Data.CommandType.StoredProcedure);

            return entities;
        }

        public int Insert(Cart cart)
        {
            Guid invoiceId = Guid.NewGuid();
            List<CartDTOs> records = (List<CartDTOs>)GetAllById(cart.AccountId.ToString(), "");
            if(records.Count() > 0)
            {
                invoiceId = (Guid)records[0].InvoiceId;
            }
            else
            {
                var invoiceCode = _invoiceService.GetInvoiceCodeBiggest();
                var sqlInsertInvoice = "INSERT INTO Invoice(InvoiceId, InvoiceCode, StatusInvoice) VALUES(@InvoiceId, @InvoiceCode, 0)";
                DynamicParameters paramtInsertInvoice = new DynamicParameters();

                paramtInsertInvoice.Add("@InvoiceId", invoiceId);
                paramtInsertInvoice.Add("@InvoiceCode", invoiceCode);

                _dbContext.Connection.Query<Invoice>(sql: sqlInsertInvoice, param: paramtInsertInvoice);
            }

            var sqlSelect = "SELECT * FROM Cart WHERE ProductId = @ProductId AND AccountId = @AccountId AND Status = 1";
            DynamicParameters paramtSelect = new DynamicParameters();

            paramtSelect.Add("@ProductId", cart.ProductId);
            paramtSelect.Add("@AccountId", cart.AccountId);

            var resultSelect = _dbContext.Connection.QueryFirstOrDefault<Cart>(sql: sqlSelect, param: paramtSelect);

            if(resultSelect == null)
            {
                var sqlCommand = $"INSERT INTO Cart(CartId, ProductId, InvoiceId, AccountId, QuantityPurchased, Status)  VALUES(@CartId, @ProductId, @InvoiceId, @AccountId, @QuantityPurchased, @Status)";
                DynamicParameters paramt = new DynamicParameters();

                paramt.Add("@CartId", Guid.NewGuid());
                paramt.Add("@ProductId", cart.ProductId);
                paramt.Add("@InvoiceId", invoiceId);
                paramt.Add("@AccountId", cart.AccountId);
                paramt.Add("@QuantityPurchased", cart.QuantityPurchased);
                paramt.Add("@Status", 1);

                var resultInsert = _dbContext.Connection.Query<Cart>(sql: sqlCommand, param: paramt);
            }
            else
            {
                var sqlUpdate = $"UPDATE Cart SET QuantityPurchased = @QuantityPurchased WHERE ProductId = @ProductId AND AccountId = @AccountId AND Status = 1";
                DynamicParameters paramtupdate = new DynamicParameters();

                paramtupdate.Add("@QuantityPurchased", resultSelect.QuantityPurchased + cart.QuantityPurchased);
                paramtupdate.Add("@ProductId", cart.ProductId);
                paramtupdate.Add("@AccountId", cart.AccountId);

                var resultInsert = _dbContext.Connection.Query<Cart>(sql: sqlUpdate, param: paramtupdate);
            }
            return 1;

        }

        public int MultipleDelete(List<Guid> ids)
        {
            // Tạo danh sách tham số và chuỗi các tham số
            var paramet = new DynamicParameters();
            var parametList = new List<string>();

            // Tạo tham số cho mỗi ID trong danh sách
            for (int i = 0; i < ids.Count; i++)
            {
                var parametName = $"@Id{i}";
                paramet.Add(parametName, ids[i]);
                parametList.Add(parametName);
            }

            // Tạo câu lệnh SQL sử dụng IN và thực hiện xóa
            var sqlCommand = $"DELETE FROM Cart WHERE CartId IN ({string.Join(", ", parametList)})";

            var delete = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);

            return 1;
        }

        public int Update(Guid invoiceId, Guid accountId)
        {
            var sqlCommand = "UPDATE Cart SET Status = 0 WHERE AccountId = @accountId AND InvoiceId = @invoiceId AND Status = 1";
            DynamicParameters paramt = new DynamicParameters();

            paramt.Add("@accountId", accountId);
            paramt.Add("@invoiceId", invoiceId);

            var result = _dbContext.Connection.Query<Cart>(sqlCommand, paramt);
            return 1;
        }
    }
}
