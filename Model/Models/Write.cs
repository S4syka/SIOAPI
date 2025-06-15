using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Write
{
    public Guid UserId { get; set; }

    public Guid TestId { get; set; }

    public DateTime? Date { get; set; }

    public string? Answers { get; set; }

    public string? Results { get; set; }

    public virtual ICollection<Handin> Handins { get; set; } = new List<Handin>();

    public virtual Test Test { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
