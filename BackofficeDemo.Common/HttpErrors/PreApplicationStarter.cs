using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: PreApplicationStartMethod(typeof(BackofficeDemo.Common.HttpErrors.PreApplicationStarter), "Start")]
namespace BackofficeDemo.Common.HttpErrors
{
    public static class PreApplicationStarter
    {
        private static bool started;

        public static void Start()
        {
            if (started)
            {
                // Only start once.
                return;
            }

            started = true;
            DynamicModuleUtility.RegisterModule(typeof(InstallerModule));
        }
    }
}
