namespace vk_facade.Runtime.helpers
{
    public static class LaunchParamsHelper
    {
        public static string ConvertToLanguage(int code)
        {
            return code switch
            {
                0 => "ru",
                1 => "uk",
                2 => "be",
                3 => "en",
                4 => "es",
                5 => "fi",
                6 => "de",
                7 => "it",
                _ => "en" // default to English if unknown code
            };
        }
        
       
    }
}