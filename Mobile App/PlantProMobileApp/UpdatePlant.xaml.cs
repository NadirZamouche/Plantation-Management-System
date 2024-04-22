using Newtonsoft.Json;
using PlantProMobileApp.Models;
using PlantProMobileApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace PlantProMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdatePlant : ContentPage
    {
        public Plant myplant { get; set; }
        public const string UpdatePlantUrl = "http://172.20.10.3:4500/api/plants"; //api url
        string token = CurrentPropertiesService.GetToken();
        public UpdatePlant(Plant plant)
        {
            InitializeComponent();

            // Set myplant properties to the plant properties that were passed to the constructor
            myplant = new Plant
            {
                Id = plant.Id,
                Name = plant.Name,
                Weight = plant.Weight,
                HarvestDate = plant.HarvestDate,
                State = plant.State,
                Verification = plant.Verification,
                Remarks = plant.Remarks,
                Greenhouse = plant.Greenhouse,
                Alley = plant.Alley
            };

            MyName.Text = myplant.Name;
            MyWeight.Text = myplant.Weight.ToString();
            MyHarvestDate.Date = myplant.HarvestDate;
            MyState.IsChecked = myplant.State;
            MyRemarks.Text = myplant.Remarks;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            float weight;
            if (!float.TryParse(MyWeight.Text, out weight))
            {
                await DisplayAlert("Error", "Veuillez saisir une valeur numérique (un nombre) pour le rendement", "OK");
                return;
            }

            // Update the myplant properties based on the user input
            myplant.Weight = weight;
            myplant.HarvestDate = MyHarvestDate.Date;
            myplant.State = MyState.IsChecked;
            myplant.Remarks = MyRemarks.Text;

            var serializeItem = JsonConvert.SerializeObject(myplant);
            StringContent body = new StringContent(serializeItem, Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var result = await httpClient.PutAsync(UpdatePlantUrl, body);
                //string data = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    CurrentPropertiesService.Logout();
                    await Navigation.PushAsync(new LoginPage());
                }
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await DisplayAlert("Erreur", "l'actualisation des données a échoué", "OK");
                    return;
                }
                await Navigation.PopAsync();
                await DisplayAlert("Succès", "l'actualisation des données a été effectuée avec succès", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Délai d'attente", "La demande a expiré. il semble qu'il y a un problème de connexion au serveur. Veuillez réessayer plus tard", "OK");
                return;
            }
        }
    }
}