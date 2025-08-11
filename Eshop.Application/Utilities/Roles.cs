namespace Eshop.Application.Utilities
{
    public static class Roles
    {
        public const string Administrator = "7";
        public const string UserSystem = "8";
        public const string ContentUploader = "10";
        public const string AdminAssistant = "11";
        public const string BikeDelivery = "12";

        public static string GetRoleBy(long id)
        {
            return id switch
            {
                7 => "مدیر سیستم",
                8 => "کاربر سیستم",
                10 => "محتوا گذار",
                11 => "دستیار مدیر",
                12 => "پیک موتوری",
                _ => ""
            };
        }
    }
}
