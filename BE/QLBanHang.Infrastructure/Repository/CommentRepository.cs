using Dapper;
using QLBanHang.Core.DTOs;
using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.DBContext;
using QLBanHang.Core.Interfaces.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Infrastructure.Repository
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        IDbContext _dbContext;
        public CommentRepository(IDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public IEnumerable<FeedbackDTOs> GetCommentById(string id)
        {
            var sqlCommand = "Proc_GetCommentById";

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("id", id);

            var entity = _dbContext.Connection.Query<FeedbackDTOs>(sql: sqlCommand, param: paramet, commandType: System.Data.CommandType.StoredProcedure);

            return entity;
        }

        public int Insert(Comment comment)
        {
            string colNameList = "";
            string colPramList = "";

            var props = typeof(Comment).GetProperties();

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add($"@{props[0].Name}", Guid.NewGuid());
            colNameList += $"{props[0].Name},";
            colPramList += $"@{props[0].Name},";

            for (int i = 1; i < props.Length; i++)
            {
                colNameList += $"{props[i].Name},";
                colPramList += $"@{props[i].Name},";

                paramet.Add($"@{props[i].Name}", props[i].GetValue(comment));
            }


            colNameList = colNameList.Substring(0, colNameList.Length - 1);
            colPramList = colPramList.Substring(0, colPramList.Length - 1);

            var sqlCommand = $"INSERT INTO Comment({colNameList}) VALUES({colPramList})";

            _dbContext.Connection.Execute(sql: sqlCommand, param: paramet);

            return 1;
        }

        public IEnumerable<Comment> SortDecrease()
        {
            var sqlCommand = "SELECT * FROM Comment";
            var entities = _dbContext.Connection.Query<Comment>(sql: sqlCommand);
            return entities.OrderByDescending(comment => Convert.ToInt64(comment.CommentCode.Substring(3)));
        }

        public IEnumerable<FeedbackDTOs> GetById(Guid id)
        {
            var sqlCommand = "Proc_GetCommentFeedbackById";

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("commentId", id);

            var entity = _dbContext.Connection.Query<FeedbackDTOs>(sql: sqlCommand, param: paramet, commandType: System.Data.CommandType.StoredProcedure);

            return entity;
        }

        public Comment GetByIdComment(Guid id)
        {
            var sqlCommand = $"SELECT * FROM Comment WHERE CommentId = @entityId";

            DynamicParameters paramet = new DynamicParameters();

            paramet.Add("@entityId", id);

            var entity = _dbContext.Connection.QueryFirstOrDefault<Comment>(sql: sqlCommand, param: paramet);

            return entity;
        }
    }
}
