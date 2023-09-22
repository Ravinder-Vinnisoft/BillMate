using BillMate.Data;
using BillMate.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace PaymentScheduler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SendPaymentlinktoPatients();
        }

      //  private readonly static SchedulerConfigurationHandler _schedulerConfigurationHandler = new SchedulerConfigurationHandler();

        public static void SendPaymentlinktoPatients()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TestsContext"].ConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<BillMateDBContext>();
            optionsBuilder.UseSqlServer(connectionString); 
        
            using (var context = new BillMateDBContext(optionsBuilder.Options))
            {
                SchedulerConfigurationHandler _schedulerConfigurationHandler = new SchedulerConfigurationHandler(context, null, null);
                //get the second through n attempts
                var configs = context.SchedulerConfiguration.Where(s => s.AttemptNumber != 1).ToList();
                _schedulerConfigurationHandler.SendSubsequentAttempts(configs);
            }
        }
    }
}