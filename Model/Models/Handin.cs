using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Handin
{
    public Guid HomeworkId { get; set; }

    public Guid UserId { get; set; }

    public Guid TestId { get; set; }

    public DateTime? Date { get; set; }

    public string? Feedback { get; set; }

    public virtual Homework Homework { get; set; } = null!;

    public virtual Write Write { get; set; } = null!;
}
