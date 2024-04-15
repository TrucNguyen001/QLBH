using Dapper;
using QLBanHang.Core.Interfaces.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using QLBanHang.Core.Interfaces.Infastructure;

namespace QLBanHang.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : class
    {
        IDbContext _dbContext;

        private string _nameClass;

        public BaseRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _nameClass = typeof(T).Name;
        }

        public IEnumerable<T> GetAll()
        {
            var entities = _dbContext.Connection.Query<T>($"SELECT * FROM {_nameClass}");
            return entities;
        }

        public bool CheckDuplicateCode(string code)
        {
            var sqlCommand = $"SELECT * FROM {_nameClass} WHERE {_nameClass}Code = @entityCode";

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
            var sqlCommand = $"DELETE FROM {_nameClass} WHERE {_nameClass}Id = @entityId";

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("@entityId", id);

            var misaEntity = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);
            return 1;
        }

        public T GetById(Guid id)
        {
            var sqlCommand = $"SELECT * FROM {_nameClass} WHERE {_nameClass}Id = @entityId";

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("@entityId", id);

            var entity = _dbContext.Connection.QueryFirstOrDefault<T>(sql: sqlCommand, param: paramet);

            return entity;
        }

        public int Insert(T record)
        {
            string colNameList = "";
            string colPramList = "";

            var props = typeof(T).GetProperties();

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

            var sqlCommand = $"INSERT INTO {_nameClass}({colNameList}) VALUES({colPramList})";

            var misaEntityInsert = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);

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
            var sqlCommand = $"DELETE FROM {_nameClass} WHERE {_nameClass}Id IN ({string.Join(", ", parametList)})";

            var delete = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);

            return 1;
        }

        public int MultiplePost(List<T> entitys)
        {
            string colNameList = "";

            var props = typeof(Type).GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                colNameList += $"{props[i].Name},";
            }
            colNameList = colNameList.Substring(0, colNameList.Length - 1);

            var sqlCommand = $"INSERT INTO {_nameClass}({colNameList}) VALUES ";

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

        public int Update(T entity, Guid id)
        {
            string stringUpdate = "";

            var props = typeof(T).GetProperties();

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("@entityId", id);

            for (int i = 1; i < props.Length; i++)
            {
                stringUpdate += $"{props[i].Name} = @{props[i].Name},";

                paramet.Add($"@{props[i].Name}", props[i].GetValue(entity));
            }


            stringUpdate = stringUpdate.Substring(0, stringUpdate.Length - 1);

            var sqlCommand = $"UPDATE {_nameClass} SET {stringUpdate} WHERE {_nameClass}Id = @entityId";

            var update = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);
            return 1;
        }

        public IEnumerable<T> GetByText(string text)
        {
            var sqlCommand = $"SELECT * FROM {_nameClass} WHERE {_nameClass}Name LIKE @text OR {_nameClass}Code LIKE @text";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@text", "%" + text + "%", System.Data.DbType.String);

            var entities = _dbContext.Connection.Query<T>(sql: sqlCommand, param: paramet);

            return entities;
        }

        public IEnumerable<T> GetPaging(int pageSize, int pageIndex, string text)
        {
            var sqlCommand = $"SELECT * FROM {_nameClass} WHERE {_nameClass}Name LIKE @text OR {_nameClass}Code LIKE @text ORDER BY COALESCE(ModifiedDate, CreatedDate) DESC";
            DynamicParameters paramet = new DynamicParameters();
            paramet.Add("@text", "%" + text + "%", System.Data.DbType.String);

            var entities = _dbContext.Connection.Query<T>(sql: sqlCommand, param: paramet);
            entities = entities.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            return entities;
        }

        protected virtual T RecordDTOs(T entity)
        {
            return entity;
        }

        public void Dispose()
        {
            _dbContext.Connection?.Dispose();
        }

        public int MultipleUpdateStatus(List<Guid> ids, int status)
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
            var sqlCommand = $"UPDATE {_nameClass} SET StatusInvoice = {status} WHERE {_nameClass}Id IN ({string.Join(", ", parametList)})";

            var delete = _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);

            return 1;
        }
    }
}
