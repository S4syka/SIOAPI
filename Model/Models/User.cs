using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? FullName { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    public virtual ICollection<Write> Writes { get; set; } = new List<Write>();
}
