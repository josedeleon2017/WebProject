using System;
using System.Collections.Generic;

namespace StoreDataAccess.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool ActiveFlag { get; set; }

    public virtual Role Role { get; set; } = null!;
}
