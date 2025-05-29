using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Course
{
    public string CourseId { get; set; } = null!;

    public string CourseName { get; set; } = null!;

    public string? Description { get; set; }

    public string TeacherId { get; set; } = null!;

    public decimal? Price { get; set; }

    public int? MaxStudentQuantity { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreatedUserId { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? UpdatedUserId { get; set; }

    public virtual ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();

    public virtual ICollection<RoomCourse> RoomCourses { get; set; } = new List<RoomCourse>();

    public virtual AspNetUser Teacher { get; set; } = null!;
}
