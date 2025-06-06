using System;
using System.Collections.Generic;

namespace Backend.Model;

public partial class Card
{
    public int CardId { get; set; }

    public int PersonId { get; set; }

    public DateOnly ValidFrom { get; set; }

    public DateOnly ValidThru { get; set; }

    public virtual User Person { get; set; } = null!;
}
