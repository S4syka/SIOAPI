using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Group
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid? Owner { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    public virtual User? OwnerNavigation { get; set; }
}
