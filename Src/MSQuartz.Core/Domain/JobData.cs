namespace MSQuartz.Core.Domain
{
    using System.Collections.Generic;

    public class JobData : ActivityNode<TriggerData>
    {
        public JobData(Quartz.JobKey jobKey, SortedList<string,TriggerData> triggers)
            : base(jobKey.Name)
        {
            Triggers = triggers;
            GroupName = jobKey.Group;
        }

        public SortedList<string, TriggerData> Triggers { get; set; }

        public string GroupName { get; private set; }

        public string UniqueName
        {
            get
            {
                return string.Format("{0}_{1}", GroupName, Name);
            }
        }

        public bool HaveTriggers
        {
            get
            {
                return Triggers != null && Triggers.Count > 0;
            }
        }

        protected override IList<TriggerData> ChildrenActivities
        {
            get { return Triggers.Values; }
        }
    }
}