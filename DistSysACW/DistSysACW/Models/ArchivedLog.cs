using System;
using System.ComponentModel.DataAnnotations;
using DistSysACW.Facades;

namespace DistSysACW.Models
{
    public class ArchivedLog
    {
        // DB fields.
        [Key] // Make primary key.
        public string LogId { get; set; } // Primary key.
        public string LogString { get; set; }
        public DateTime LogDateTime { get; set; }
        public DateTime ArchiveDateTime { get; set; }
        public string UserApiKey { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ArchivedLog() { }

        public ArchivedLog(string apiKey, Log log)
        {
            UserApiKey = apiKey;
            LogId = log.LogId;
            LogString = log.LogString;
            LogDateTime = log.LogDateTime;
            ArchiveDateTime = DateTime.Now;
        }
    }

    public static class ArchiveDatabaseAcess
    {
        public static void ArchiveUser(UserContext ctx, User user)
        {
            DbAccessFacade.ModifyDb(() =>
            {
                string apiKey = user.ApiKey;

                foreach (Log l in user.Logs)
                {
                    ArchivedLog alog = new ArchivedLog(apiKey, l);
                    ctx.ArchivedLogs.Add(alog);
                    ctx.Logs.Remove(l);
                }

                ctx.SaveChanges();
            });
        }
    }
}