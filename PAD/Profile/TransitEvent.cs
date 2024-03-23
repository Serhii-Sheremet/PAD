using System;

namespace PAD
{
    public class TransitEvent
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public DateTime EventDate { get; set; }
        public int LocationId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }

    }
}
