using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Room
{
    public string RoomId { get; set; } = null!;

    public string RoomName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreatedUserId { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? UpdatedUserId { get; set; }

    public virtual ICollection<RoomCourse> RoomCourses { get; set; } = new List<RoomCourse>();
}
 