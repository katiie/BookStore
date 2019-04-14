using System;
using System.Collections.Generic;
using System.Text;

namespace Bookstore.Core
{
   
     public enum RequestStatusEnum
    {
        Requested,
        New,
        AwaitingPostCardUpdate,
        Generated,
        Dispatched,
        FullyReceived,
        Approved
    }
}
