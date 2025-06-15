using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Category
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
