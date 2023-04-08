using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mRole
{
    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
}
