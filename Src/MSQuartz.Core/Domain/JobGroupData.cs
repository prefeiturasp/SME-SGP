namespace MSQuartz.Core.Domain
{
    using System.Collections.Generic;

    public class JobGroupData : ActivityNode<JobData>
    {
        public JobGroupData(string name, SortedList<string, JobData> jobs)
            : base(name)
        {
            Jobs = jobs;
        }

        public SortedList<string,JobData> Jobs { get; set; }

        protected override IList<JobData> ChildrenActivities
        {
            get { return Jobs.Values; }
        }
    }
}