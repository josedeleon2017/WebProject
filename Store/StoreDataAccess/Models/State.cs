using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class State
{
    public int StateId { get; set; }

    public int RegionId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; } = new List<Address>();

    public virtual Region Region { get; set; } = null!;
}
