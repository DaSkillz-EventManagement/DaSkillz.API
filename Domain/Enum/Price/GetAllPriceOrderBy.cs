using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum.Price;

public enum GetAllPriceOrderBy
{
    [Description("PriceType")]
    PriceType,

    [Description("amount")]
    amount,

    [Description("UpdatedAt")]
    UpdateAt,

    [Description("CreatedAt")]
    CreateAt

}
