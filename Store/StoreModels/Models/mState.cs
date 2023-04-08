using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mState
{
    public int StateId { get; set; }

    public int RegionId { get; set; }

    public string Name { get; set; } = null!;
}
