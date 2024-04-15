using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.Core.ValidateException
{
    public class ValidateExceptionError : Exception
    {
        #region Field
        string? msgValidateException = null;
        #endregion

        #region Contructor
        public ValidateExceptionError(string msg)
        {
            this.msgValidateException = msg;
        }
        #endregion

        #region Methods
        //Ghi đè phương thức của Exception
        public override string Message
        {
            get
            {
                return msgValidateException;
            }
        }
        #endregion
    }
}
