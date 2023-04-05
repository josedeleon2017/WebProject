using System;
using System.Collections.Generic;

namespace StoreDataAccess.Models;

public partial class ProductReview
{
    public int ProductReviewId { get; set; }

    public int ProductId { get; set; }

    public string ReviewerName { get; set; } = null!;

    public DateTime ReviewDate { get; set; }

    public string EmailAddress { get; set; } = null!;

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }

    public virtual Product Product { get; set; } = null!;
}
