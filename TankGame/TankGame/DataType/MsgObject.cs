using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks_Client.DataType
{

    /****** this class is used to store the incoming msgs with its date and time*****/
    class MsgObject
    {
        private String message;
        private DateTime time;

        public MsgObject(String msg, DateTime time)
        {
            this.message = msg;
            this.time = time;
        }

        public void setMessage(String msg)
        {
            this.message = msg;
        }
        public void setTime(DateTime time)
        {
            this.time = time;
        }
        public String getMessage()
        {
            return this.message;
        }
        public DateTime getTime()
        {
            return this.time;
        }

    }
}
