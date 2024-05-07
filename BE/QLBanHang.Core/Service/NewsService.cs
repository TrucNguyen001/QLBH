using QLBanHang.Core.Entities;
using QLBanHang.Core.Interfaces.Infastructure;
using QLBanHang.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.Service
{
    public class NewsService : BaseService<News>, INewsService
    {
        INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository) : base(newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public string GetNewsCodeBiggest()
        {
            var news = _newsRepository.SortDecrease();
            if (news.FirstOrDefault() == null)
            {
                return "TT-00001";
            }
            else
            {
                News newsFirst = news.FirstOrDefault();
                var newsFirstCode = newsFirst.NewsCode;
                var numberCode = newsFirstCode.Substring(3);
                var numberCodeBiggest = Convert.ToInt64(numberCode) + 1;
                numberCode = PadLeftCustom(Convert.ToString(numberCodeBiggest), numberCode.Length, '0');
                newsFirstCode = newsFirstCode.Substring(0, 3) + numberCode;
                return newsFirstCode;
            }
        }
    }
}
