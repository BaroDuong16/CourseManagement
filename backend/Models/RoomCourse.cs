using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class RoomCourse
{
    public string RoomCourseId { get; set; } = null!;

    public string RoomId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public string? CreatedUserId { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? UpdatedUserId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
