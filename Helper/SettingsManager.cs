using System.Configuration;

namespace Helper
{
    public static class SettingsManager
    {
        public static string TimezoneName => ConfigurationManager.AppSettings["Timezone:Name"];

        public static string EmailToCustomerFrom => ConfigurationManager.AppSettings["Email:Customer:From"];
        public static string EmailToPartnerFrom => ConfigurationManager.AppSettings["Email:Partner:From"];

        public static string PlivoAuthId = "";

        public static string PlivoAuthToken = "";

        public static string PlivoFromPhone = "";
    }
}
