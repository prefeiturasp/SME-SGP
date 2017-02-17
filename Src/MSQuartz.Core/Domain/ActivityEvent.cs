using System;

namespace MSQuartz.Core.Domain
{
    public class ActivityEvent
    {
        public ActivityEvent(DateTime dateUtc, string description)
        {
            DateUtc = dateUtc;
            Description = description;
        }

        public DateTime DateUtc { get; private set; }

        public string Description { get; private set; }
    }
}