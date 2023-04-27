using System;
using System.Collections.Generic;

namespace StoreDataAccess.Models;

public partial class Region
{
    public int RegionId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<State> States { get; } = new List<State>();
}
