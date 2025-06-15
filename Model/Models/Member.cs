using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Member
{
    public Guid UserId { get; set; }

    public Guid GroupId { get; set; }

    public bool? Accepted { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
