using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Tag
{
    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public virtual Category CategoryNavigation { get; set; } = null!;

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
