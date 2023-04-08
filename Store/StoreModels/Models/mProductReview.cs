using System;
using System.Collections.Generic;

namespace StoreModels.Models;

public partial class mProductReview
{
    public int ProductReviewId { get; set; }

    public int ProductId { get; set; }

    public string ReviewerName { get; set; } = null!;

    public DateTime ReviewDate { get; set; }

    public string EmailAddress { get; set; } = null!;

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }
}
