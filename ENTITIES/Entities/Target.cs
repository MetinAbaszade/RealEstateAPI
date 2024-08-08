using System;
using System.Collections.Generic;

namespace ENTITIES.Entities;

public partial class Target
{
    public int IdTarget { get; set; }

    public string? TargetName { get; set; }

    public int? FkIdRegion { get; set; }

    public DateTime? InsertDate { get; set; }

    public int? IsDeleted { get; set; }
}
