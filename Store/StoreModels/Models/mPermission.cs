using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class mPermission
{
    public int PermissionId { get; set; }

    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool ActiveFlag { get; set; }

    public virtual mRole Role { get; set; } = null!;
}
