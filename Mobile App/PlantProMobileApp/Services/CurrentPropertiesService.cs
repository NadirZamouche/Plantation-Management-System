using PlantProMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PlantProMobileApp.Services
{
    public class CurrentPropertiesService
    {
        private static string KEY_NAME = "key_name";
        private static string KEY_MAIL = "key_male";
        private static string KEY_TOKEN = "key_token";

        public static void SaveUser (AuthUserDto user)
        {
            Application.Current.Properties[KEY_NAME] = user.Name;
            Application.Current.Properties[KEY_MAIL] = user.Login;
            Application.Current.Properties[KEY_TOKEN] = user.Token;
            Application.Current.SavePropertiesAsync();
        }

        public static string GetName()
        {
            if(Application.Current.Properties.ContainsKey(KEY_NAME))
            {
                return (string)Application.Current.Properties[KEY_NAME];
            }
            return string.Empty;
        }

        public static string GetEmail()
        {
            if (Application.Current.Properties.ContainsKey(KEY_MAIL))
            {
                return (string)Application.Current.Properties[KEY_MAIL];
            }
            return string.Empty;
        }

        public static string GetToken()
        {
            if (Application.Current.Properties.ContainsKey(KEY_TOKEN))
            {
                return (string)Application.Current.Properties[KEY_TOKEN];
            }
            return string.Empty;
        }

        public static bool IsAuth()
        {
            if (Application.Current.Properties.ContainsKey(KEY_TOKEN))
            {
                return true;
            }
            return false;
        }

        public static void Logout()
        {
            Application.Current.Properties.Remove(KEY_TOKEN);
            Application.Current.SavePropertiesAsync();
        }
    }
}