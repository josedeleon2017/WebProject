using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class mProductPhoto
{
    public int ProductPhotoId { get; set; }

    public int ProductId { get; set; }

    public string PhotoFileName { get; set; } = null!;

    public string PhotoExtension { get; set; } = null!;

    public string PhotoPath { get; set; } = null!;

    public int OrderDisplay { get; set; }

    public virtual mProduct Product { get; set; } = null!;
}
