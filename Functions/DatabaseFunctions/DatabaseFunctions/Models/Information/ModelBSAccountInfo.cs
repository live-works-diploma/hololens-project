using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFunctions.Models.Information
{
    /// <summary>
    /// The information needed to connect to an azure blob storage
    /// </summary>
    public class ModelBSAccountInfo
    {
        public static string connectionString = "DefaultEndpointsProtocol=https;AccountName=hololens1storage;AccountKey=TrOzIaK/ZQLXCb1TAN6IVvRogLBXxcRiSXxUDiDC9pWep/W2K3t6E/Hg3PNhR9Bc2VzyJjg2rtNp+AStEaROEw==;EndpointSuffix=core.windows.net";
        public static string containerName = "container1";
    }
}
