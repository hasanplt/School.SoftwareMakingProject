namespace School.SoftwareMakingProject.Domain.DbEntities
{
    public class Event
    {
        public Guid? Id { get; set; }
        public DateTime? event_start_datetime { get; set; }
        public Guid? typeid { get; set; }
        public string? description { get; set; }
        public DateTime? event_end_datetime { get; set; }
        public DateTime? created_datetime { get; set; }
        public Guid userId { get; set; }
        public bool? is_on_remind { get; set; }
        public bool? is_complete { get; set; }
    }
}
