﻿namespace API_Consumer.Authority
{
    public static class AppRepository
    {
        private static List<Application> _applications = new List<Application>()
        {
            new Application()
            {
                ApplicationId = 1,
                ApplicationName = "MVCWebApp",
                ClientId = "53D3C1E6-4587-4AD5-8C6E-A8E4BD59940E",
                Secret = "0673FC70-0514-4011-B4A3-DF9BC03201BC"
            }
        };

        public static bool Authenticate(string clientId, string secret)
        {
            return (from application in _applications
                    where application.ClientId == clientId
                    where application.Secret == secret
                    select application).Any();
        }

        public static Application? GetApplication(string clientId)
        {
            return _applications.FirstOrDefault(a => a.ClientId == clientId);
        }
    }
}