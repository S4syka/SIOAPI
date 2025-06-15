using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Homework
{
    public Guid Id { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid? GroupId { get; set; }

    public virtual Group? Group { get; set; }

    public virtual ICollection<Handin> Handins { get; set; } = new List<Handin>();

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
