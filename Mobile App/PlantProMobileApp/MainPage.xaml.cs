using Newtonsoft.Json;
using PlantProMobileApp.Models;
using PlantProMobileApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using ZXing;
using ZXing.Aztec.Internal;
using ZXing.Net.Mobile.Forms;

namespace PlantProMobileApp
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _client = new HttpClient(); //allows the xamarin forms to communicate with the api
        public const string PlanningsUrl = "http://172.20.10.3:4500/api/plannings/"; //api url
        public const string GetPlantUrl = "http://172.20.10.3:4500/api/plants/"; //api url
        private ObservableCollection<Planning> planning;
        string token = CurrentPropertiesService.GetToken();
        string name = CurrentPropertiesService.GetName();
        string email = CurrentPropertiesService.GetEmail();

        ZXingScannerPage scanPage;
        public MainPage()
        {
            InitializeComponent();
            ScanButton.Clicked += ScanButton_Clicked;
            DateTime now = DateTime.Now;

            switch (now.Hour)
            {
                case int n when (n >= 4 && n < 12):
                    lbl2.Text = "Bonjour " + name + ".";
                    break;
                case int n when (n >= 12 && n < 24):
                    lbl2.Text = "Bonsoir " + name + ".";
                    break;
                default:
                    lbl2.Text = "Travailler tard le soir " + name + "?";
                    break;
            }
        }

        async override protected void OnAppearing()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await _client.GetAsync(PlanningsUrl + email);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    CurrentPropertiesService.Logout();
                    await Navigation.PushAsync(new LoginPage());
                }
                string responsecontent = await response.Content.ReadAsStringAsync(); //get result from the api
                List<Planning> mylist = JsonConvert.DeserializeObject<List<Planning>>(responsecontent); //convert result from string to object of type alley
                foreach (Planning item in mylist)
                {
                    if (item.Order == null)
                    {
                        item.Order = "Aucun";
                    }
                } //if the value for order is null it displays aucun
                planning = new ObservableCollection<Planning>(mylist); //store the list in a variable
                ItemlistView.ItemsSource = planning; //binding the listview context with the list variable
                base.OnAppearing(); //launch the method
            }
            catch (Exception ex)
            {
                await DisplayAlert("Délai d'attente", "La demande a expiré. il semble qu'il y a un problème de connexion au serveur. Veuillez réessayer plus tard", "OK");
                return;
            }
        }
        private async void myRefreshView_Refreshing(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet && Connectivity.NetworkAccess != NetworkAccess.Local) //neither internet nor lan connections
            {
                await DisplayAlert("Aucune connexion au serveur", "Impossible de se connecter à PlantPro. Veuillez vérifier votre connexion Internet ou votre connexion LAN, redémarrer l'application et réessayer.", "OK"); //pop alert message
                myRefreshView.IsEnabled = false; //disabe the refreshing
                ConnectivityLabel.IsVisible = true; //show label error message
                ItemlistView.IsVisible = false; //hide the list view
                ScanButton.IsVisible = false;
                TaskView.IsVisible = false;
                return;
            }
            myRefreshView.IsEnabled = true; //else enabe the refreshing
            await Task.Delay(2000); //wait for 2 seconds
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                
                var response = await _client.GetAsync(PlanningsUrl + email);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    CurrentPropertiesService.Logout();
                    await Navigation.PushAsync(new LoginPage());
                }
                string responsecontent = await response.Content.ReadAsStringAsync(); //get result from the api
                List<Planning> mylist = JsonConvert.DeserializeObject<List<Planning>>(responsecontent); //convert result from string to object of type alley
                foreach (Planning item in mylist)
                {
                    if (item.Order == null)
                    {
                        item.Order = "Aucun";
                    }
                } //if the value for order is null it displays aucun
                planning = new ObservableCollection<Planning>(mylist); //store the list in a variable
                ItemlistView.ItemsSource = planning; //binding the listview context with the list variable
                myRefreshView.IsRefreshing = false; //stop refresh animation
            }
            catch (Exception ex)
            {
                await DisplayAlert("Délai d'attente", "La demande a expiré. il semble qu'il y a un problème de connexion au serveur. Veuillez réessayer plus tard", "OK");
                return;
            }
        }

        private async void ScanButton_Clicked(object sender, EventArgs e)
        {
            scanPage = new ZXingScannerPage(); //create a scanpage
            scanPage.OnScanResult += (result) => //scanpage wait for the result
            {
                scanPage.IsScanning = false; //scanning is not done yet

                //Do something with the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync(); //close to the scanpage
                    await GetPlant(result); //pass the result to the GetPlant method
                });
            };
            await Navigation.PushAsync(scanPage); //move to the scanpage
        }

        public async Task GetPlant(ZXing.Result result)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                var response = await httpClient.GetAsync(GetPlantUrl + result.Text);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    CurrentPropertiesService.Logout();
                    await Navigation.PushAsync(new LoginPage());
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Show error message if plant not found
                    await DisplayAlert("Erreur", "Aucune plante n'a été trouvée, assurer que le résultat du code-barres correspond au nom de la plante dans la base de données et réessayer.", "OK");
                    return;
                }
                var plantJson = await response.Content.ReadAsStringAsync();
                var plant = JsonConvert.DeserializeObject<Plant>(plantJson);

                // Navigate to UpdatePlant page with the plant object
                await Navigation.PushAsync(new UpdatePlant(plant));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Délai d'attente", "La demande a expiré. il semble qu'il y a un problème de connexion au serveur. Veuillez réessayer plus tard", "OK");
                return;
            }
        }

        private void LogoutClick(object sender, EventArgs e)
        {
            CurrentPropertiesService.Logout();
            Navigation.PushAsync(new LoginPage());
        }
    }
}