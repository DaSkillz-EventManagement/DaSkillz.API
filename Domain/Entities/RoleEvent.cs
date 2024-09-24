namespace Domain.Entities
{
    public partial class RoleEvent
    {
        public RoleEvent()
        {
            Participants = new HashSet<Participant>();
        }

        public int RoleEventId { get; set; }
        public string? RoleEventName { get; set; }

        public virtual ICollection<Participant> Participants { get; set; }
    }
}
