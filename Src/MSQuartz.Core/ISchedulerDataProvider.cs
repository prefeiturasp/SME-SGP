using System;
using System.Collections.Generic;

namespace MSQuartz.Core
{
    using Domain;

    /// <summary>
    /// Translates Quartz.NET entyties to MSQuartz objects graph.
    /// </summary>
    public interface ISchedulerDataProvider
    {
        SchedulerData Data { get; }

        IEnumerable<ActivityEvent> GetJobEvents(string name, DateTime minDateUtc, DateTime maxDateUtc); 

        IList<TriggerData> GetTriggersOfJob(string groupName, string jobName);

        IList<JobData> GetJobsOfGroup(string groupName);

        void PauseTrigger(Quartz.TriggerKey triggerKey);

        void ResumeTrigger(Quartz.TriggerKey triggerKey);
        
        void ScheduleCronTriggerForJob(Quartz.JobKey jobKey, string triggerName, string cronExpression);

        void DeleteTrigger(Quartz.TriggerKey triggerKey);
    }
}