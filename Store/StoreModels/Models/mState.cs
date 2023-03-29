using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class mState
{
    public int StateId { get; set; }

    public int RegionId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<mAddress> Addresses { get; } = new List<mAddress>();

    public virtual mRegion Region { get; set; } = null!;
}
