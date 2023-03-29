using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class mRole
{
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<mEmployee> Employees { get; } = new List<mEmployee>();

    public virtual ICollection<mPermission> Permissions { get; } = new List<mPermission>();
}
