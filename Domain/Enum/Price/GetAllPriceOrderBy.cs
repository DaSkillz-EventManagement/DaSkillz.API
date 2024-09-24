using System.ComponentModel;

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
