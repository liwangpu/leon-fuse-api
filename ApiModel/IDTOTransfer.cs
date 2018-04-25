using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
    public interface IDTOTransfer<T>
        where T : IData
    {
        T ToDTO();
    }
}
