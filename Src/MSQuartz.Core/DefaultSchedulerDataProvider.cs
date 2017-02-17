namespace MSQuartz.Core
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Quartz;
    using SchedulerProviders;
    using System.Linq;

    public class DefaultSchedulerDataProvider : ISchedulerDataProvider
    {
        private readonly ISchedulerProvider _schedulerProvider;

        public DefaultSchedulerDataProvider(ISchedulerProvider schedulerProvider)
        {
            _schedulerProvider = schedulerProvider;
        }

        public SchedulerData Data
        {
            get
            {
                var scheduler = _schedulerProvider.Scheduler;
                var metadata = scheduler.GetMetaData();
                return new SchedulerData
                           {
                               SchedulerName = scheduler.SchedulerName,
                               InstanceId = scheduler.SchedulerInstanceId,
                               JobGroups = GetJobGroups(scheduler),
                               TriggerGroups = GetTriggerGroups(scheduler),
                               Status = GetSchedulerStatus(scheduler),
                               IsRemote = metadata.SchedulerRemote,
                               JobsExecuted = metadata.NumberOfJobsExecuted,
                               RunningSince = metadata.RunningSince,
                               SchedulerType = metadata.SchedulerType
                           };
            }
        }                                                

        public static SchedulerStatus GetSchedulerStatus(IScheduler scheduler)
        {
            if (scheduler.IsShutdown)
            {
                return SchedulerStatus.Shutdown;
            }

            if (scheduler.GetJobGroupNames() == null || scheduler.GetJobGroupNames().Count == 0)
            {
                return SchedulerStatus.Empty;
            }

            if (scheduler.IsStarted)
            {
                return SchedulerStatus.Started;
            }

            return SchedulerStatus.Ready;
        }


        private static ActivityStatus GetTriggerStatus(string triggerName, string triggerGroup, IScheduler scheduler)
        {

            TriggerKey triggerKey = new TriggerKey(triggerName, triggerGroup);
            var state = scheduler.GetTriggerState(triggerKey);
            switch (state)
            {
                case TriggerState.Paused:
                    return ActivityStatus.Paused;
                case TriggerState.Complete:
                    return ActivityStatus.Complete;
                default:
                    return ActivityStatus.Active;
            }
        }

        private static ActivityStatus GetTriggerStatus(ITrigger trigger, IScheduler scheduler)
        {
            return GetTriggerStatus(trigger.Key.Name, trigger.Key.Group, scheduler);
        }


        public JobDetailsData GetJobDetailsData(JobKey jobKey)
        {
            var scheduler = _schedulerProvider.Scheduler;

            if (scheduler.IsShutdown)
            {
                return null;
            }

            var job = scheduler.GetJobDetail(jobKey);
            if (job == null)
            {
                return null;
            }

            var detailsData = new JobDetailsData
            {
                PrimaryData = GetJobData(scheduler, jobKey)
            };

            foreach (var key in job.JobDataMap.Keys)
            {
                detailsData.JobDataMap.Add(key, job.JobDataMap[key]);
            }

            detailsData.JobProperties.Add("Description", job.Description);
            //detailsData.JobProperties.Add("Full name", job.FullName);
            detailsData.JobProperties.Add("Job type", job.JobType);
            detailsData.JobProperties.Add("Durable", job.Durable);
            //detailsData.JobProperties.Add("Volatile", job.Volatile);

            return detailsData;
        }

        public IEnumerable<ActivityEvent> GetJobEvents(string name, DateTime minDateUtc, DateTime maxDateUtc)
        {
            return new List<ActivityEvent>();
        }

        //public IList<TriggerData> GetTriggers(string name, string group)
        //{

        //    return this.Data.JobGroups.Where(p => p.Name == group)
        //        .SelectMany(p => p.Jobs)
        //        .Where(p => p.Name == name)
        //        .SelectMany(p => p.Triggers).ToList();
        //}

        private static IList<TriggerGroupData> GetTriggerGroups(IScheduler scheduler)
        {
            var result = new List<TriggerGroupData>();
            if (!scheduler.IsShutdown)
            {
                foreach (var groupName in scheduler.GetTriggerGroupNames())
                {
                    var data = new TriggerGroupData(groupName);
                    data.Init();
                    result.Add(data);
                }
            }

            return result;
        }

        private static SortedList<string,JobGroupData> GetJobGroups(IScheduler scheduler)
        {
            var result = new SortedList<string,JobGroupData>();

            if (!scheduler.IsShutdown)
            {
                foreach (var groupName in scheduler.GetJobGroupNames())
                {
                    var groupData = new JobGroupData(
                        groupName,
                        GetJobs(scheduler, groupName));
                    groupData.Init();
                    result.Add(groupName,groupData);
                }
            }

            return result;
        }

        private static SortedList<string, JobData> GetJobs(IScheduler scheduler, string groupName)
        {
            var result = new SortedList<string, JobData>();

            foreach (var jobKey in scheduler.GetJobKeys(Quartz.Impl.Matchers.GroupMatcher<JobKey>.GroupEquals(groupName)))
            {                   
               result.Add(jobKey.Name,GetJobData(scheduler,jobKey));
            }            
            return result;
        }

        private static JobData GetJobData(IScheduler scheduler, JobKey jobkey)
        {
            var jobData = new JobData(jobkey, GetTriggers(scheduler, jobkey));
            jobData.Init();
            return jobData;
        }

        private static SortedList<string,TriggerData> GetTriggers(IScheduler scheduler, JobKey jobkey)
        {
            var result = new SortedList<string,TriggerData>();

            foreach (var trigger in scheduler.GetTriggersOfJob(jobkey))
            {
                var data = new TriggerData(trigger.Key.Name, GetTriggerStatus(trigger, scheduler))
                {
                    StartDate = trigger.StartTimeUtc,
                    EndDate = trigger.EndTimeUtc,
                    NextFireDate = trigger.GetNextFireTimeUtc(),
                    PreviousFireDate = trigger.GetPreviousFireTimeUtc(),
                    TypeOfTrigger = trigger.GetType() 
                };
                
                result.Add(trigger.Key.Name,data);
            }                       
            return result;
        }
            
        public IList<JobData> GetJobsOfGroup(string groupName) 
        {
            var result = Data.JobGroups[groupName].Jobs.Values;         
            return result;
        }
        public IList<TriggerData> GetTriggersOfJob(string groupName, string jobName)
        {
            var result = Data.JobGroups[groupName].Jobs[jobName].Triggers.Values;
            return result;
        }  

        public void PauseTrigger(TriggerKey triggerKey)
        {
            _schedulerProvider.Scheduler.PauseTrigger(triggerKey);
        }

        public void ResumeTrigger(TriggerKey triggerKey)
        {
            _schedulerProvider.Scheduler.ResumeTrigger(triggerKey);
        }
       
        public void ScheduleCronTriggerForJob(JobKey jobKey, string triggerName, string cronExpression)
        {
            if (CronExpression.IsValidExpression(cronExpression))
            {
                var jobDetail = _schedulerProvider.Scheduler.GetJobDetail(jobKey);

                ICronTrigger cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                                 .ForJob(jobDetail)
                                 .WithIdentity(triggerName)
                                 .WithCronSchedule(cronExpression)
                                 .Build();
                _schedulerProvider.Scheduler.ScheduleJob(cronTrigger);
            }
            else
            {
                throw new Exception("A cronExpression está inválida (" + cronExpression + ")");
            }
        }

        #region ISchedulerDataProvider Members
                                            
        public void DeleteTrigger(TriggerKey triggerKey)
        {
            _schedulerProvider.Scheduler.UnscheduleJob(triggerKey);
        }

        #endregion
    }
}