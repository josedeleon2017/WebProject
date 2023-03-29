using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class mRegion
{
    public int RegionId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<mState> States { get; } = new List<mState>();
}
