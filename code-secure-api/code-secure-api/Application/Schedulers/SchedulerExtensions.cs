using CodeSecure.Application.Schedulers.Job;
using Quartz;

namespace CodeSecure.Application.Schedulers;

public static class SchedulerExtensions
{
    public static IServiceCollection AddScheduleJobs(this IServiceCollection builder)
    {
        builder.AddQuartz(configure =>
        {
            configure.UseDefaultThreadPool(c => c.MaxConcurrency = 10);
            // security alert at 08:00 AM, only on Monday
            var securityAlertJobKey = new JobKey(nameof(WeeklySecurityAlertJob));
            configure
                .AddJob<WeeklySecurityAlertJob>(securityAlertJobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(securityAlertJobKey)
                    .WithCronSchedule("0 0 8 ? * MON")
                );
            // clear expired session 
            var clearExpiredSessionJobKey = new JobKey(nameof(ClearExpiredSessionJob));
            configure
                .AddJob<ClearExpiredSessionJob>(clearExpiredSessionJobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(clearExpiredSessionJobKey)
                    .WithCronSchedule("0 0 0 ? * MON")
                );
            // alert project without member at 08:00 AM, only on Mon
            var projectWithoutMemberJobKey = new JobKey(nameof(AlertProjectWithoutMemberJob));
            configure
                .AddJob<AlertProjectWithoutMemberJob>(projectWithoutMemberJobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(projectWithoutMemberJobKey)
                    .WithSimpleSchedule(c => c.WithIntervalInMinutes(1).RepeatForever())
                    .WithCronSchedule("0 0 8 ? * MON")
                );
        });
        
        builder.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });
        return builder;
    }
}