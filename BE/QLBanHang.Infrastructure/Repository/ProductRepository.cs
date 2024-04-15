using AutoMapper;
using Dapper;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.DBContext;
using QLBanHang.Core.Interfaces.Infastructure;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        IDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductRepository(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<Product> SortDecrease()
        {
            var sqlCommand = $"SELECT * FROM Product";
            var entities = _dbContext.Connection.Query<Product>(sql: sqlCommand);
            return entities.OrderByDescending(product => Convert.ToInt64(product.ProductCode.Substring(3)));
        }

        public IEnumerable<Product> GetAll()
        {
            var entities = _dbContext.Connection.Query<Product>($"SELECT * FROM Product");
            return entities;
        }

        public bool CheckDuplicateCode(string code)
        {
            var sqlCommand = $"SELECT * FROM Product WHERE ProductCode = @entityCode";

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("@entityCode", code);

            var misaEntity = _dbContext.Connection.QueryFirstOrDefault(sql: sqlCommand, param: paramet);

            if (misaEntity == null)
            {
                return false;
            }

            return true;
        }

        public int Delete(Guid id)
        {
            var sqlCommand = $"DELETE FROM Product WHERE ProductId = @entityId";
            var sqlConfig = $"DELETE FROM Configuration WHERE ProductId = @entityId";

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("@entityId", id);

            var config = _dbContext.Connection.Execute(sql: sqlConfig, param: paramet);
            var product = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);
            return 1;
        }

        public ProductDTOs GetById(Guid id)
        {
            var sqlCommand = "Proc_GetProductById";

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("id", id);

            var entity = _dbContext.Connection.QueryFirstOrDefault<ProductDTOs>(sql: sqlCommand, param: paramet, commandType: System.Data.CommandType.StoredProcedure);

            return entity;
        }

        public int Insert(ProductDTOs record)
        {
            Product product = _mapper.Map<Product>(record);
            Configuration configuration = _mapper.Map<Configuration>(record);

            Guid productId = Guid.NewGuid();
            configuration.ProductId = productId;
            if (string.IsNullOrEmpty(product.PriceBuy.ToString()))
            {
                product.PriceBuy = product.Price;
            }
            product.PriceReduced = product.Price - product.PriceBuy;
            product.Status = 1;
            product.QuantityBuy = 0;
            product.CreatedDate = DateTime.Now;

            string colNameList = "";
            string colPramList = "";

            var props = typeof(Product).GetProperties();

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add($"@{props[0].Name}", productId);
            colNameList += $"{props[0].Name},";
            colPramList += $"@{props[0].Name},";

            for (int i = 1; i < props.Length; i++)
            {
                colNameList += $"{props[i].Name},";
                colPramList += $"@{props[i].Name},";

                paramet.Add($"@{props[i].Name}", props[i].GetValue(product));
            }


            colNameList = colNameList.Substring(0, colNameList.Length - 1);
            colPramList = colPramList.Substring(0, colPramList.Length - 1);

            var sqlCommand = $"INSERT INTO Product({colNameList}) VALUES({colPramList})";

            var insert = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);

            InsertConfig(configuration);

            return 1;
        }

        public int InsertConfig(Configuration record)
        {
            string colNameList = "";
            string colPramList = "";

            var props = typeof(Configuration).GetProperties();

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add($"@{props[0].Name}", Guid.NewGuid());
            colNameList += $"{props[0].Name},";
            colPramList += $"@{props[0].Name},";

            for (int i = 1; i < props.Length; i++)
            {
                colNameList += $"{props[i].Name},";
                colPramList += $"@{props[i].Name},";

                paramet.Add($"@{props[i].Name}", props[i].GetValue(record));
            }


            colNameList = colNameList.Substring(0, colNameList.Length - 1);
            colPramList = colPramList.Substring(0, colPramList.Length - 1);

            var sqlCommand = $"INSERT INTO Configuration({colNameList}) VALUES({colPramList})";

            var insert = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);

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
            var sqlCommand = $"DELETE FROM Product WHERE ProductId IN ({string.Join(", ", parametList)})";
            var sqlCommandConfig = $"DELETE FROM Configuration WHERE ProductId IN ({string.Join(", ", parametList)})";

            var deleteConfig = _dbContext.Connection.Execute(sql: sqlCommandConfig, param: paramet);
            var delete = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);

            return 1;
        }

        public int MultiplePost(List<Product> entitys)
        {
            string colNameList = "";

            var props = typeof(Type).GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                colNameList += $"{props[i].Name},";
            }
            colNameList = colNameList.Substring(0, colNameList.Length - 1);

            var sqlCommand = $"INSERT INTO Product({colNameList}) VALUES ";

            DynamicParameters paramet = new DynamicParameters();

            // Gán vị trí 
            var index = 0;
            foreach (var item in entitys)
            {
                string colPramList = "(";
                colPramList += $"@{props[0].Name}{index},";
                paramet.Add($"@{props[0].Name}{index}", Guid.NewGuid());

                for (int i = 1; i < props.Length; i++)
                {
                    colPramList += $"@{props[i].Name}{index},";
                    paramet.Add($"@{props[i].Name}{index}", props[i].GetValue(item));
                }
                colPramList = colPramList.Substring(0, colPramList.Length - 1);
                colPramList += "),";

                sqlCommand += colPramList;

                // Thay đổi vị trí tránh trùng trong pram
                index++;
            }
            // Xóa dấu phẩy cuối cùng và thêm dấu chấm phẩy vào cuối câu lệnh SQL
            sqlCommand = sqlCommand.TrimEnd(',') + ";";

            var insert = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);

            return 1;
        }

        public int Update(ProductDTOs record, Guid id)
        {
            Product product = _mapper.Map<Product>(record);
            Configuration configuration = _mapper.Map<Configuration>(record);
            product.ModifiedDate = DateTime.Now;
            string stringUpdate = "";

            var props = typeof(Product).GetProperties();

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("@entityId", id);

            for (int i = 1; i < props.Length; i++)
            {
                stringUpdate += $"{props[i].Name} = @{props[i].Name},";

                paramet.Add($"@{props[i].Name}", props[i].GetValue(product));
            }


            stringUpdate = stringUpdate.Substring(0, stringUpdate.Length - 1);

            var sqlCommand = $"UPDATE Product SET {stringUpdate} WHERE ProductId = @entityId";

            var update = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);

            UpdateConfig(configuration);
            return 1;
        }

        public int UpdateConfig(Configuration entity)
        {
            string stringUpdate = "";

            var props = typeof(Configuration).GetProperties();

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("@entityId", entity.ConfigurationId);

            for (int i = 1; i < props.Length; i++)
            {
                stringUpdate += $"{props[i].Name} = @{props[i].Name},";

                paramet.Add($"@{props[i].Name}", props[i].GetValue(entity));
            }


            stringUpdate = stringUpdate.Substring(0, stringUpdate.Length - 1);

            var sqlCommand = $"UPDATE Configuration SET {stringUpdate} WHERE ConfigurationId = @entityId";

            var update = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);
            return 1;
        }

        public IEnumerable<Product> GetByText(string text)
        {
            var sqlCommand = $"SELECT * FROM Product WHERE ProductName LIKE @text OR ProductCode LIKE @text";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@text", "%" + text + "%", System.Data.DbType.String);

            var entities = _dbContext.Connection.Query<Product>(sql: sqlCommand, param: paramet);

            return entities;
        }

        public IEnumerable<ProductDTOs> GetPaging(int pageSize, int pageIndex, string text)
        {
            var sqlCommand = "Proc_GetPagingProduct";
            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("pageSize", pageSize);
            paramet.Add("pageIndex", pageIndex);
            paramet.Add("text", text);

            var paging = _dbContext.Connection.Query<ProductDTOs>(sql: sqlCommand, param: paramet, commandType: System.Data.CommandType.StoredProcedure);

            return paging;
        }

        public IEnumerable<Product> GetPagingUser(int pageIndex, string text, int optionSelect, string productTypeId)
        {
            var sqlCommand = "Proc_GetProductsUser";
            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("pageIndex", pageIndex);
            paramet.Add("text", text);
            paramet.Add("optionSelect", optionSelect);
            paramet.Add("productTypeId", productTypeId);

            var paging = _dbContext.Connection.Query<Product>(sql: sqlCommand, param: paramet, commandType: System.Data.CommandType.StoredProcedure);

            return paging;
        }

        public void Dispose()
        {
            _dbContext.Connection?.Dispose();
        }

        public IEnumerable<Product> TotalRecordPaging(string text, string productTypeId)
        {
            var sqlCommand = $"SELECT * FROM Product WHERE ProductName LIKE @text AND ProductTypeId LIKE @productTypeId";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@text", "%" + text + "%", System.Data.DbType.String);
            paramet.Add("@productTypeId", "%" + productTypeId + "%", System.Data.DbType.String);

            var entities = _dbContext.Connection.Query<Product>(sql: sqlCommand, param: paramet);
            return entities;
        }
    }
}
