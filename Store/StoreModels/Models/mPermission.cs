using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mPermission
{
    public int PermissionId { get; set; }

    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool ActiveFlag { get; set; }
}
