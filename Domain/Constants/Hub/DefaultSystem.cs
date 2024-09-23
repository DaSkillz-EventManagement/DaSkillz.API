using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants.Hub;

public class DefaultSystem
{
    public const string CheckinHubConnection = "/checkinHub";
    public const int LimitFile = 1073741824; // 1GB
    public const int CacheTime = 60; // 1 hour
}
