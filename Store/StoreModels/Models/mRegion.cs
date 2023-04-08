using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mRegion
{
    public int RegionId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;
}
