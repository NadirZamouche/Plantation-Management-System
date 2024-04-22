using Newtonsoft.Json;
using PlantProMobileApp.Models;
using PlantProMobileApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlantProMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public AuthUserDto user { get; set; }
        public const string AuthenticateUrl = "http://172.20.10.3:4500/api/authenticate"; //api url
        public LoginPage()
        {
            InitializeComponent();
        }

        async void LoginClick(object sender, EventArgs e)
        {
            if (login.Text == null && password.Text == null)
            {
                await DisplayAlert("Erreur", "veuillez saisir votre adresse mail et mot de passe", "OK");
                return;
            }
            if (login.Text== null && password.Text != null)
            {
                await DisplayAlert("Erreur", "veuillez saisir votre adresse mail", "OK");
                return;
            }
            if (login.Text != null && password.Text == null)
            {
                await DisplayAlert("Erreur", "veuillez saisir votre mot de passe", "OK");
                return;
            }
            
            user = new AuthUserDto //create a new user object of type AuthUserDto that the arguments passed by the user in Email & password.
            {
                Login = login.Text,
                Password = password.Text,
                Name = "",
                Token = ""
            };
            var serializeItem = JsonConvert.SerializeObject(user);
            StringContent body = new StringContent(serializeItem, Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            try
            {
                var result = await httpClient.PostAsync(AuthenticateUrl, body);
                //string data = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await DisplayAlert("Erreur", "Mail ou un mot de passe invalide", "OK");
                    return;
                }
                var authuserJson = await result.Content.ReadAsStringAsync();
                var authuser = JsonConvert.DeserializeObject<AuthUserDto>(authuserJson);
                CurrentPropertiesService.SaveUser(authuser);
                await Navigation.PushAsync(new MainPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Délai d'attente", "La demande a expiré. il semble qu'il y a un problème de connexion au serveur. Veuillez réessayer plus tard", "OK");
                return;
            }
        }
    }
}