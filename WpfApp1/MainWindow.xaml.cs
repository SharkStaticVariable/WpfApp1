using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using static WpfApp1.MainWindow;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using System.Windows.Media;
using System.Text.RegularExpressions;
using WpfApp1.Module;
using System.Diagnostics;


namespace WpfApp1
{
    
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        private Dictionary<string, bool> tabDataLoaded = new Dictionary<string, bool>();

        /**/
        #region FOR_ORDERS
        public ObservableCollection<Clients> CLIENTS { get; set; }
        public ObservableCollection<string> CLIENTSItems { get; set; }

        public ObservableCollection<PaymentMethods> PAYMENTMETHODS { get; set; }
        public ObservableCollection<string> PAYMENTMETHODSItems { get; set; }

        public ObservableCollection<Sessions> SESSIONS { get; set; }
        public ObservableCollection<string> SESSIONSItems { get; set; }
        #endregion

        #region FOR_PLACES
        public ObservableCollection<PlaceCategories> PLACECATEGORIES { get; set; }
        public ObservableCollection<string> PLACECATEGORIESItems { get; set; }

        public ObservableCollection<Status> STATUS { get; set; }
        public ObservableCollection<string> STATUSItems { get; set; }
        #endregion

        public ObservableCollection<Movies> MOVIES { get; set; }
        public ObservableCollection<string> MOVIESItems { get; set; }

        public ObservableCollection<Halls> HALLS { get; set; }
        public ObservableCollection<string> HALLSItems { get; set; }

        public ObservableCollection<Places> PLACES { get; set; }
        public ObservableCollection<string> PLACESItems { get; set; }

        /**/
        public MainWindow()
        {
            client.BaseAddress = new Uri("http://localhost:5068/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
            tabDataLoaded["Клиенты"] = false;
            tabDataLoaded["Залы"] = false;
            tabDataLoaded["Фильмы"] = false;
            tabDataLoaded["Заказы"] = false;
            tabDataLoaded["Места"] = false;
            tabDataLoaded["Сеансы"] = false;
            tabDataLoaded["Билеты"] = false;
            tabDataLoaded["Статус"] = false;
            tabDataLoaded["Метод оплаты"] = false;
            tabDataLoaded["Категория места"] = false;


            this.Title = "Учёт продаж билетов";
            /* WindowState = WindowState.Maximized;*/
            tabControl.SelectionChanged += TabControl_SelectionChanged;


            CLIENTS = new ObservableCollection<Clients>();
            PAYMENTMETHODS = new ObservableCollection<PaymentMethods>();
            PLACECATEGORIES = new ObservableCollection<PlaceCategories>();
            STATUS = new ObservableCollection<Status>();
            SESSIONS = new ObservableCollection<Sessions>();
            MOVIES = new ObservableCollection<Movies>();
            HALLS = new ObservableCollection<Halls>();
            PLACES = new ObservableCollection<Places>();

            LoadDataCLIENTS();
            LoadDataPAYMENTMETHODS();
            LoadDataPLACECATEGORIES();
            LoadDataSTATUS();
            LoadDataSESSIONS();
            LoadDataMOVIES();
            LoadDataHALLS();
            LoadDataPLACES();
            this.DataContext = this;
        }

        #region COMBOBOX
        private async void LoadDataCLIENTS()
        {
            try
            {
                var clients = await GetClientsFromApiAsync();
                foreach (var client in clients)
                {
                    CLIENTS.Add(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading clients: {ex.Message}");
            }
        }
        private async void LoadDataPAYMENTMETHODS()
        {
            try
            {
                var paymentmethods = await GetPaymentMethodsFromApiAsync();
                foreach (var paymentmethod in paymentmethods)
                {
                    PAYMENTMETHODS.Add(paymentmethod);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payment methods: {ex.Message}");
            }
        }
        private async void LoadDataPLACECATEGORIES()
        {
            try
            {
                var placeCategories = await GetLoadDataPLACECATEGORIESFromApiAsync();
                foreach (var placeCategorie in placeCategories)
                {
                    PLACECATEGORIES.Add(placeCategorie);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading placeCategorie: {ex.Message}");
            }
        }
        private async void LoadDataSTATUS()
        {
            try
            {
                var status = await GetSTATUSFromApiAsync();
                foreach (var statu in status)
                {
                    STATUS.Add(statu);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statu: {ex.Message}");
            }
        }
        private async void LoadDataSESSIONS()
        {
            try
            {
                var sessions = await GetSESSIONSFromApiAsync();
                foreach (var session in sessions)
                {
                    SESSIONS.Add(session);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statu: {ex.Message}");
            }
        }
        private async void LoadDataPLACES()
        {
            try
            {
                var places = await GetPLACESFromApiAsync();
                foreach (var place in places)
                {
                    PLACES.Add(place);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statu: {ex.Message}");
            }
        }
        private async void LoadDataMOVIES()
        {
            try
            {
                var movies = await GetMOVIESFromApiAsync();
                foreach (var movie in movies)
                {
                    MOVIES.Add(movie);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statu: {ex.Message}");
            }
        }
        private async void LoadDataHALLS()
        {
            try
            {
                var halls = await GetHALLSFromApiAsync();
                foreach (var hall in halls)
                {
                    HALLS.Add(hall);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statu: {ex.Message}");
            }
        }

        private async Task<List<Clients>> GetClientsFromApiAsync()
        {
            HttpResponseMessage response = await client.GetAsync("clients");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Clients>>();
        }
        private async Task<List<PaymentMethods>> GetPaymentMethodsFromApiAsync()
        {
            HttpResponseMessage response = await client.GetAsync("paymentmethods");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<PaymentMethods>>();
        }
        private async Task<List<PlaceCategories>> GetLoadDataPLACECATEGORIESFromApiAsync()
        {
            HttpResponseMessage response = await client.GetAsync("PlaceCategories");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<PlaceCategories>>();
        }
        private async Task<List<Status>> GetSTATUSFromApiAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Status");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Status>>();
        }
        private async Task<List<Sessions>> GetSESSIONSFromApiAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Sessions");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Sessions>>();
        }
        private async Task<List<Places>> GetPLACESFromApiAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Places");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Places>>();
        }
        private async Task<List<Movies>> GetMOVIESFromApiAsync()
        {
            HttpResponseMessage response = await client.GetAsync("movies");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Movies>>();
        }
        private async Task<List<Halls>> GetHALLSFromApiAsync()
        {
            HttpResponseMessage response = await client.GetAsync("halls");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Halls>>();
        }

        #endregion

        #region VIEW

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem selectedTab)
            {
                string tabName = selectedTab.Header.ToString();

                if (!tabDataLoaded[tabName])
                {
                    switch (tabName)
                    {
                        case "Клиенты":
                             GetClients();
                            break;
                        case "Фильмы":
                            GetMovies();
                            break;
                        case "Залы": 
                            GetHalls();
                            break;
                        case "Заказы":
                            GetOrders();
                            break;
                        case "Места":
                            GetPlaces();
                            break;
                        case "Сеансы": 
                            GetSessions();
                            break;
                        case "Билеты":
                            GetTickets();
                            break;
                        case "Статус":
                            GetStatus();
                            break;
                        case "Категория места":
                            GetPlaceCategories();
                            break;
                        case "Метод оплаты":
                            GetPaymentMethods();
                            break;
                    }
                    tabDataLoaded[tabName] = true;
                }
            }
        }
        private void ButtonView_Click(object sender, RoutedEventArgs e)
            {
                if (tabControl.SelectedItem is TabItem selectedTab)
                    {
                       
                        if (selectedTab.Header.ToString() == "Клиенты") { GetClients(); }
                        if (selectedTab.Header.ToString() == "Фильмы") { GetMovies(); }
                        if (selectedTab.Header.ToString() == "Залы") { GetHalls(); }
                        if (selectedTab.Header.ToString() == "Заказы") { GetOrders(); }
                        if (selectedTab.Header.ToString() == "Места") { GetPlaces(); }
                        if (selectedTab.Header.ToString() == "Сеансы") { GetSessions(); }
                        if (selectedTab.Header.ToString() == "Билеты") { GetTickets(); }
                        if (selectedTab.Header.ToString() == "Статус") { GetStatus(); }
                        if (selectedTab.Header.ToString() == "Категория места") { GetPlaceCategories(); }
                        if (selectedTab.Header.ToString() == "Метод оплаты") { GetPaymentMethods(); }

            }
        }
        #endregion

        #region TASK
        private async Task InsertData<T>(string url, T data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Данные успешно добавлены!");
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении данных.");
            }
        }
        private async Task DeleteData(string url)
        {
            var response = await client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Данные успешно удалены!");
            }
            else
            {
                MessageBox.Show("Ошибка при удалении данных.");
            }
        }
        private async Task UpdateData(string url, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Данные успешно обновлены!");
            }
            else
            {
                MessageBox.Show("Ошибка при обновлении данных.");
            }
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber) && Regex.IsMatch(phoneNumber, @"^7\d{10}$");
        }
        private bool IsValidEmail(string email)
        {
           
            return !string.IsNullOrEmpty(email) && Regex.IsMatch(email,
                @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }
        #endregion

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem selectedTab)
            {
                switch (selectedTab.Header.ToString())
                {
                    case "Клиенты":
                        string clientUrl = "/Clients"; 
                        Clients selectedClient = dbClients.SelectedItem as Clients;
                        if (selectedClient == null)
                        {
                            MessageBox.Show("Выберите поле для добавления.");
                            return;
                        }
                        
                        if (!IsValidPhoneNumber(selectedClient.Phone))
                        {
                            MessageBox.Show("Некорректный формат номера телефона.");
                            return;
                        }

                        
                        if (!IsValidEmail(selectedClient.Email))
                        {
                            MessageBox.Show("Некорректный формат электронной почты.");
                            return;
                        }
                        Clients client = new Clients()
                        {
                            Id = selectedClient.Id,
                            Phone = selectedClient.Phone,
                            Email = selectedClient.Email
                        };

                        InsertData(clientUrl, client);
                        break;
                    case "Статус":
                        string StatusUrl = "/Status";
                        Status selectedStatus = dbStatus.SelectedItem as Status;
                        if (selectedStatus == null)
                        {
                            MessageBox.Show("Выберите поле для добавления.");
                            return;
                        }
                        Status status = new Status()
                        {
                            Id = selectedStatus.Id,
                            status = selectedStatus.status

                        };

                        InsertData(StatusUrl, status);
                        break;
                    case "Категория места":
                        string PlaceCategoriesUrl = "/PlaceCategories";
                        PlaceCategories selectedPlaceCategories = dbPlaceCategories.SelectedItem as PlaceCategories;
                        if (selectedPlaceCategories == null)
                        {
                            MessageBox.Show("Выберите поле для добавления.");
                            return;
                        }
                        PlaceCategories placeCategories = new PlaceCategories()
                        {
                            Id = selectedPlaceCategories.Id,
                            type = selectedPlaceCategories.type

                        };

                        InsertData(PlaceCategoriesUrl, placeCategories);
                        break;
                    case "Метод оплаты":
                        string PaymentMethodsUrl = "/PaymentMethods";
                        PaymentMethods selectedPaymentMethods = dbPaymentMethods.SelectedItem as PaymentMethods;
                        if (selectedPaymentMethods == null)
                        {
                            MessageBox.Show("Выберите поле для добавления.");
                            return;
                        }
                        PaymentMethods paymentMethods = new PaymentMethods()
                        {
                            Id = selectedPaymentMethods.Id,
                            Payment_method = selectedPaymentMethods.Payment_method

                        };

                        InsertData(PaymentMethodsUrl, paymentMethods);
                        break;
                    case "Фильмы":
                        string moviUrl = "/Movies";
                        Movies selectedMovie = dbMovies.SelectedItem as Movies;
                        if (selectedMovie == null)
                        {
                            MessageBox.Show("Выберите поле для добавления.");
                            return;
                        }
                        Movies movi = new Movies()
                        {
                            Id = selectedMovie.Id,
                            genre = selectedMovie.genre,
                            timing = selectedMovie.timing,
                            name = selectedMovie.name,
                            country = selectedMovie.country,
                            year = selectedMovie.year,
                            director = selectedMovie.director,
                           
                            release_date = selectedMovie.release_date,
                            price = selectedMovie.price,
                        };

                        InsertData(moviUrl, movi);
                        break;
                    case "Залы":
                        string halUrl = "/Halls";
                        Halls selectedHall = dbHalls.SelectedItem as Halls;
                        if (selectedHall == null)
                        {
                            MessageBox.Show("Выберите поле для добавления.");
                            return;
                        }
                        Halls hal = new Halls()
                        {
                            Id = selectedHall.Id,
                            Count_rows = selectedHall.Count_rows,
                            Count_place_of_rows = selectedHall.Count_place_of_rows,
                            Name_hall = selectedHall.Name_hall
                           
                        };

                        InsertData(halUrl, hal);
                        break;
                    case "Сеансы":
                        string SesUrl = "/Sessions";
                        Sessions selectedSessions = dbSessions.SelectedItem as Sessions;
                        if (selectedSessions == null)
                        {
                            MessageBox.Show("Выберите поле для добавления.");
                            return;
                        }

                        selectedHall = selectedSessions.Halls;
                        selectedMovie = selectedSessions.Movies;

                        // Проверьте, что выбраны клиент и метод оплаты
                        if (selectedHall == null || selectedMovie == null )
                        {
                            MessageBox.Show("Выберите поля для добавления.");
                            return;
                        }

                        Sessions ses = new Sessions()
                        {
                            Id = selectedSessions.Id,
                            Data_session = selectedSessions.Data_session,
                            Time_session = selectedSessions.Time_session,
                            Id_hall = selectedHall.Id,
                            Id_movie = selectedMovie.Id
                        };

                        InsertData(SesUrl, ses);
                        break;

                    case "Заказы":
                        string ordUrl = "/orders";
                        Orders selectedOrders = dbOrders.SelectedItem as Orders;
                        if (selectedOrders == null)
                        {
                            MessageBox.Show("Выберите поле для добавления.");
                            return;
                        }

                        
                        selectedClient = selectedOrders.Clients;
                        selectedPaymentMethods = selectedOrders.PaymentMethods;
                        /*selectedSessions = selectedOrders.Sessions;*/

                        // Проверьте, что выбраны клиент и метод оплаты
                        if (selectedClient == null || selectedPaymentMethods == null /*|| selectedSessions == null */)
                        {
                            MessageBox.Show("Выберите клиента и метод оплаты для добавления.");
                            return;
                        }
                        
                        Orders ord = new Orders() 
                        { 
                        
                            Id = selectedOrders.Id,
                            /**/
                            Id_client = selectedClient.Id,
                            Id_payment_method = selectedPaymentMethods.Id,
                            /**/
                            Number_order = selectedOrders.Number_order,
                            Data_order = selectedOrders.Data_order,
                            payment_amount = selectedOrders.payment_amount
                            /*Id_session = selectedSessions.Id*/

                        };

                        InsertData(ordUrl, ord);
                        break;




                    case "Места":
                        string PlacUrl = "/Places";
                        Places selectedPlaces = dbPlaces.SelectedItem as Places;
                        if (selectedPlaces == null)
                        {
                            MessageBox.Show("Выберите поле для добавления.");
                            return;
                        }

                        selectedStatus = selectedPlaces.Status;
                        selectedPlaceCategories = selectedPlaces.PlaceCategories;

                        // Проверьте, что выбраны клиент и метод оплаты
                        if (selectedStatus == null || selectedPlaceCategories == null)
                        {
                            MessageBox.Show("Выберите поля для добавления.");
                            return;
                        }

                        Places pls = new Places()
                        {
                            Id = selectedPlaces.Id,
                          
                            Place_number = selectedPlaces.Place_number,

                            /**/
                            Id_category = selectedPlaceCategories.Id,
                            Id_status = selectedStatus.Id
                            /**/
                        };

                        InsertData(PlacUrl, pls);
                        break;

                   

                    case "Билеты":
                        string TicUrl = "/Tickets";
                        Tickets selectedTickets = dbTickets.SelectedItem as Tickets;
                        if (selectedTickets == null)
                        {
                            MessageBox.Show("Выберите поле для добавления.");
                            return;
                        }

                        selectedSessions = selectedTickets.Sessions;
                        selectedPlaces = selectedTickets.Places;

                        // Проверьте, что выбраны клиент и метод оплаты
                        if (selectedSessions == null || selectedPlaces == null)
                        {
                            MessageBox.Show("Выберите поля для добавления.");
                            return;
                        }


                        Tickets tic = new Tickets()
                        {
                            Id = selectedTickets.Id,
                            Id_session = selectedSessions.Id,
                            Id_place = selectedPlaces.Id,

                            Ticket_number = selectedTickets.Ticket_number
                        };

                        InsertData(TicUrl, tic);
                        break;

                   
                    default:
                        MessageBox.Show("Выберите вкладку для добавления данных.");
                        break;
                }
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

            if (tabControl.SelectedItem is TabItem selectedTab)
            {
                switch (selectedTab.Header.ToString())
                {
                    case "Клиенты":
                        Clients selectedClient = dbClients.SelectedItem as Clients;
                        MessageBox.Show(selectedClient.Id.ToString());

                        if (selectedClient == null)
                        {
                            MessageBox.Show("Выбери данные в таблице для удаления");
                        }
                        else
                        {
                            string clientUrl = $"/clients/{selectedClient.Id}";

                            DeleteData(clientUrl);
                        }

                        break;
                    case "Фильмы":
                        Movies selectedMovie = dbMovies.SelectedItem as Movies;
                        MessageBox.Show(selectedMovie.Id.ToString());

                        if (selectedMovie == null)
                        {
                            MessageBox.Show("Выбери данные в таблице для удаления");
                        }
                        else
                        {
                            string moviUrl = $"/movies/{selectedMovie.Id}";

                            DeleteData(moviUrl);
                        }
                        
                        break;
                    case "Залы":
                        Halls selectedHall = dbHalls.SelectedItem as Halls;
                        MessageBox.Show(selectedHall.Id.ToString());

                        if (selectedHall == null)
                        {
                            MessageBox.Show("Выбери данные в таблице для удаления");
                        }
                        else
                        {
                            string hallUrl = $"/halls/{selectedHall.Id}";

                            DeleteData(hallUrl);
                        }
                        
                        break;

                    case "Заказы":
                        Orders selectedOrders = dbOrders.SelectedItem as Orders;
                        MessageBox.Show(selectedOrders.Id.ToString());

                        if (selectedOrders == null)
                        {
                            MessageBox.Show("Выбери данные в таблице для удаления");
                        }
                        else
                        {
                            string OrdUrl = $"/orders/{selectedOrders.Id}";

                            DeleteData(OrdUrl);
                        }
                        
                        break;

                    case "Места":
                        Places selectedPlaces = dbPlaces.SelectedItem as Places;
                        MessageBox.Show(selectedPlaces.Id.ToString());

                        if (selectedPlaces == null)
                        {
                            MessageBox.Show("Выбери данные в таблице для удаления");
                        }
                        else
                        {
                            string PlacUrl = $"/places/{selectedPlaces.Id}";

                            DeleteData(PlacUrl);
                        }
                        
                        break;

                    case "Сеансы":
                        Sessions selectedSessions = dbSessions.SelectedItem as Sessions;
                        MessageBox.Show(selectedSessions.Id.ToString());

                        if (selectedSessions == null)
                        {
                            MessageBox.Show("Выбери данные в таблице для удаления");
                        }
                        else
                        {
                            string SesUrl = $"/sessions/{selectedSessions.Id}";

                            DeleteData(SesUrl);
                        }
                      
                        break;

                    case "Билеты":
                        Tickets selectedTickets = dbTickets.SelectedItem as Tickets;
                        MessageBox.Show(selectedTickets.Id.ToString());

                        if (selectedTickets== null)
                        {
                            MessageBox.Show("Выбери данные в таблице для удаления");
                        }
                        else
                        {
                            string TicUrl = $"/tickets/{selectedTickets.Id}";

                            DeleteData(TicUrl);
                        }
                       
                        break;

                    case "Статус":
                        Status selectedStatus = dbStatus.SelectedItem as Status;
                        MessageBox.Show(selectedStatus.Id.ToString());

                        if (selectedStatus == null)
                        {
                            MessageBox.Show("Выбери данные в таблице для удаления");
                        }
                        else
                        {
                            string StatusUrl = $"/Status/{selectedStatus.Id}";

                            DeleteData(StatusUrl);
                        }

                        break;
                    case "Категория места":
                        PlaceCategories selectedPlaceCategories = dbPlaceCategories.SelectedItem as PlaceCategories;
                        MessageBox.Show(selectedPlaceCategories.Id.ToString());

                        if (selectedPlaceCategories == null)
                        {
                            MessageBox.Show("Выбери данные в таблице для удаления");
                        }
                        else
                        {
                            string selectedPlaceCategoriesUrl = $"/PlaceCategories/{selectedPlaceCategories.Id}";

                            DeleteData(selectedPlaceCategoriesUrl);
                        }

                        break;

                        /*метод оплаты*/
                    default:
                        MessageBox.Show("Выберите вкладку для удаления данных.");
                        break;
                }
            }
        }
        private async void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem selectedTab)
            {
                switch (selectedTab.Header.ToString())
                {
                    case "Клиенты":
                        var selectedClient = dbClients.SelectedItem as Clients;
                        if (selectedClient != null)
                        {
                            string clientUrl = "clients/" + selectedClient.Id;
                            await UpdateData(clientUrl, selectedClient);
                        }
                        break;

                    case "Фильмы":
                        var selectedMovie = dbMovies.SelectedItem as Movies;
                        if (selectedMovie != null)
                        {
                            string moviUrl = "movies/" + selectedMovie.Id;
                            await UpdateData(moviUrl, selectedMovie);
                        }
                        break;

                    case "Залы":
                        var selectedHall = dbHalls.SelectedItem as Halls;
                        if (selectedHall != null)
                        {
                            string hallUrl = "halls/" + selectedHall.Id;
                            await UpdateData(hallUrl, selectedHall);
                        }
                        break;

                    case "Заказы":
                        var selectedOrders = dbOrders.SelectedItem as Orders;
                        if (selectedOrders != null)
                        {
                            string OrdUrl = "orders/" + selectedOrders.Id;
                            await UpdateData(OrdUrl, selectedOrders);
                        }
                        break;

                    case "Места":
                        var selectedPlaces = dbPlaces.SelectedItem as Places;
                        if (selectedPlaces != null)
                        {
                            string PlacUrl = "places/" + selectedPlaces.Id;
                            await UpdateData(PlacUrl, selectedPlaces);
                        }
                        break;

                    case "Сеансы":
                        var selectedSessions = dbSessions.SelectedItem as Sessions;
                        if (selectedSessions != null)
                        {
                            string SesUrl = "sessions/" + selectedSessions.Id;
                            await UpdateData(SesUrl, selectedSessions);
                        }
                        break;

                    case "Билеты":
                        var selectedTickets = dbTickets.SelectedItem as Tickets;
                        if (selectedTickets != null)
                        {
                            string TicUrl = "tickets/" + selectedTickets.Id;
                            await UpdateData(TicUrl, selectedTickets);
                        }
                        break;
                        /*если что дописать сюда три таблицы*/
                    default:
                        MessageBox.Show("Выберите вкладку для обновления данных.");
                        break;
                }
            }
        }

        private void OrdersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                DataGrid dataGrid = sender as DataGrid;
                DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromItem(e.Row.Item) as DataGridRow;

                // Получение заголовка столбца
                string columnHeader = e.Column.Header.ToString();

                // Обработка в зависимости от заголовка столбца
                switch (columnHeader)
                {
                    case "Данные клиента":
                        HandleClientComboBox(e.EditingElement as ComboBox);
                        break;
                    case "Метод оплаты":
                        HandlePaymentMethodComboBox(e.EditingElement as ComboBox);
                        break;
                    // Добавьте дополнительные случаи для других ComboBox
                   
                    case "Категория места":
                        HandlePlaceCategoriesComboBox(e.EditingElement as ComboBox);
                        break;
                    case "статус":
                        HandleStatusComboBox(e.EditingElement as ComboBox);
                        break;
                    case "Сеанс":
                        HandleSessionsComboBox(e.EditingElement as ComboBox);
                        break;
                    case "Место":
                        HandlePlaceComboBox(e.EditingElement as ComboBox);
                        break;
                    default:
                        break;
                }
            }
        }

        private void HandleClientComboBox(ComboBox comboBox)
        {
            if (comboBox != null)
            {
                var selectedClient = comboBox.SelectedItem as Clients;
                if (selectedClient != null)
                {
                    // Обработка выбранного клиента
                }
            }
        }

        private void HandlePaymentMethodComboBox(ComboBox comboBox)
        {
            if (comboBox != null)
            {
                var selectedPaymentMethod = comboBox.SelectedItem as PaymentMethods;
                if (selectedPaymentMethod != null)
                {
                    // Обработка выбранного метода оплаты
                }
            }
        }
        
        // Добавьте методы для обработки других ComboBox
        private void HandlePlaceCategoriesComboBox(ComboBox comboBox)
        {
            if (comboBox != null)
            {
                var selectedItem = comboBox.SelectedItem as PlaceCategories;
                if (selectedItem != null)
                {
                    // Обработка выбранного элемента для OtherColumn1
                }
            }
        }

        private void HandleStatusComboBox(ComboBox comboBox)
        {
            if (comboBox != null)
            {
                var selectedItem = comboBox.SelectedItem as Status;
                if (selectedItem != null)
                {
                    // Обработка выбранного элемента для OtherColumn2
                }
            }
        }
        private void HandleSessionsComboBox(ComboBox comboBox)
        {
            if (comboBox != null)
            {
                var selectedItem = comboBox.SelectedItem as Sessions;
                if (selectedItem != null)
                {
                    // Обработка выбранного элемента для OtherColumn2
                }
            }
        }
        private void HandlePlaceComboBox(ComboBox comboBox)
        {
            if (comboBox != null)
            {
                var selectedItem = comboBox.SelectedItem as Places;
                if (selectedItem != null)
                {
                    // Обработка выбранного элемента для OtherColumn2
                }
            }
        }


        #region GETMETHODS
        private async void GetClients()
            {
                var response = await client.GetStringAsync("clients");
                var clients = JsonConvert.DeserializeObject<List<Clients>>(response);
                dbClients.ItemsSource = clients;

            allClients = clients; // Сохраняем все данные 
            filteredClients = new List<Clients>(allClients); // Копируем данные для фильтрации


        }
        private async void GetMovies()
            {
                var response = await client.GetStringAsync("movies");
                var movies = JsonConvert.DeserializeObject<List<Movies>>(response);
             

            dbMovies.ItemsSource = movies;

            allMovies = movies; // Сохраняем все данные 
        }
        private async void GetHalls()
            {
                var response = await client.GetStringAsync("halls");
                var halls = JsonConvert.DeserializeObject<List<Halls>>(response);
                

            dbHalls.ItemsSource = halls;

            allHalls = halls; // Сохраняем все данные
        }
        private async void GetOrders()
        {
            try { 
            var response = await client.GetStringAsync("orders");
            var response_clients = await client.GetStringAsync("clients");
            var response_payment_method = await client.GetStringAsync("PaymentMethods");
               /* var response_sessions = await client.GetStringAsync("Sessions");*/


                var order = JsonConvert.DeserializeObject<List<Orders>>(response);
                var clients = JsonConvert.DeserializeObject <List<Clients>>(response_clients);
                var paymentMethods = JsonConvert.DeserializeObject<List<PaymentMethods>>(response_payment_method);
                /*var sessions = JsonConvert.DeserializeObject<List<Sessions>>(response_sessions);*/

                dbOrders.ItemsSource = order;
                // Сопоставление
                foreach (var orders in order)
                {
                    orders.Clients = clients.Where(p => p.Id.Equals(orders.Id_client)).ToList().First();
                }

                dbOrders.ItemsSource = order;

                // Сопоставление
                foreach (var orders in order)
                {
                    orders.PaymentMethods = paymentMethods.Where(p => p.Id.Equals(orders.Id_payment_method)).ToList().First();
                }

                dbOrders.ItemsSource = order;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке: " + ex.Message);
            }
        }

        private async void GetPaymentMethods()
        {
            var response = await client.GetStringAsync("paymentmethods");
            var paymentMethods = JsonConvert.DeserializeObject<List<PaymentMethods>>(response);


            dbPaymentMethods.ItemsSource = paymentMethods;

            
        }

        private async void GetPlaceCategories()
        {
            var response = await client.GetStringAsync("Placecategories");
            var placecategories = JsonConvert.DeserializeObject<List<PlaceCategories>>(response);


            dbPlaceCategories.ItemsSource = placecategories;


        }
        private async void GetStatus()
        {
            var response = await client.GetStringAsync("Status");
            var status = JsonConvert.DeserializeObject<List<Status>>(response);


            dbStatus.ItemsSource = status;


        }

        private async void GetPlaces()
        {
            try { 
            var response = await client.GetStringAsync("places");
            var place = JsonConvert.DeserializeObject<List<Places>>(response);

                var response_PlaceCategories = await client.GetStringAsync("PlaceCategories");
                var placeCategories = JsonConvert.DeserializeObject<List<PlaceCategories>>(response_PlaceCategories);

                var response_Status = await client.GetStringAsync("Status");
                var status = JsonConvert.DeserializeObject<List<Status>>(response_Status);

                dbPlaces.ItemsSource = place;

            allPlaces = place; // Сохраняем все данные
                foreach (var places in place)
                {
                    places.PlaceCategories = placeCategories.Where(p => p.Id.Equals(places.Id_category)).ToList().First();
                }

                dbPlaces.ItemsSource = place;

                // Сопоставление
                foreach (var places in place)
                {
                    places.Status = status.Where(p => p.Id.Equals(places.Id_status)).ToList().First();
                }

                dbPlaces.ItemsSource = place;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке " + ex.Message);
            }

        }

        private async void GetSessions()
        {
            try { 
            var response = await client.GetStringAsync("sessions");
            var session = JsonConvert.DeserializeObject<List<Sessions>>(response);

                var response_Halls = await client.GetStringAsync("Halls");
                var halls = JsonConvert.DeserializeObject<List<Halls>>(response_Halls);

                var response_Movies = await client.GetStringAsync("Movies");
                var movies = JsonConvert.DeserializeObject<List<Movies>>(response_Movies);
                dbSessions.ItemsSource = session;

            allSessions = session; // Сохраняем все данные
                foreach (var sessions in session)
                {
                    sessions.Halls = halls.Where(p => p.Id.Equals(sessions.Id_hall)).ToList().First();
                }

                dbSessions.ItemsSource = session;

                // Сопоставление
                foreach (var sessions in session)
                {
                    sessions.Movies = movies.Where(p => p.Id.Equals(sessions.Id_movie)).ToList().First();
                }

                dbSessions.ItemsSource = session;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке студентов: " + ex.Message);
            }

        }
        private async void GetTickets()
        {
            try { 
            var response = await client.GetStringAsync("tickets");
            var ticket = JsonConvert.DeserializeObject<List<Tickets>>(response);

                var response_Sessions = await client.GetStringAsync("sessions");
                var sessions = JsonConvert.DeserializeObject<List<Sessions>>(response_Sessions);

                var response_places = await client.GetStringAsync("places");
                var places = JsonConvert.DeserializeObject<List<Places>>(response_places);
                dbTickets.ItemsSource = ticket;
                foreach (var tickets in ticket)
                {
                    tickets.Sessions = sessions.Where(p => p.Id.Equals(tickets.Id_session)).ToList().First();
                }

                dbTickets.ItemsSource = ticket;

                // Сопоставление
                foreach (var tickets in ticket)
                {
                    tickets.Places = places.Where(p => p.Id.Equals(tickets.Id_place)).ToList().First();
                }

                dbTickets.ItemsSource = ticket;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке студентов: " + ex.Message);
            }
        }
        #endregion

        #region FILTER

        private List<Clients> allClients;
        private List<Halls> allHalls;
        private List<Movies> allMovies;
        private List<Sessions> allSessions;
        private List<Places> allPlaces;
        private List<Orders> allOrders;
        private List<Tickets> allTickets;

        private List<Clients> filteredClients;

        private void TextBox_TextFilter(object sender, TextChangedEventArgs e)
        {
            string searchText = txtFilter.Text.ToLower(); // Получаем текст из TextBox и приводим к нижнему регистру
            dbTickets.EnableRowVirtualization = false;
            dbOrders.EnableRowVirtualization = false;

            foreach (var item in dbClients.Items)
            {
                foreach (DataGridColumn column in dbClients.Columns)
                {
                    var cellContent = column.GetCellContent(item) as TextBlock;

                    if (cellContent != null)
                    {
                        if (cellContent.Text.ToLower().StartsWith(searchText))
                        {
                            cellContent.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));  // Подсветка найденных ячеек
                        }
                        else
                        {
                            cellContent.Background = Brushes.Transparent; // Очищаем подсветку
                        }
                    }
                }
            }
            foreach (var item in dbHalls.Items)
            {
                foreach (DataGridColumn column in dbHalls.Columns)
                {
                    var cellContent = column.GetCellContent(item) as TextBlock;

                    if (cellContent != null)
                    {
                        if (cellContent.Text.ToLower().StartsWith(searchText))
                        {
                            cellContent.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));  // Подсветка найденных ячеек
                        }
                        else
                        {
                            cellContent.Background = Brushes.Transparent; // Очищаем подсветку
                        }
                    }
                }
            }
            foreach (var item in dbMovies.Items)
            {
                foreach (DataGridColumn column in dbMovies.Columns)
                {
                    var cellContent = column.GetCellContent(item) as TextBlock;

                    if (cellContent != null)
                    {
                        if (cellContent.Text.ToLower().StartsWith(searchText))
                        {
                            cellContent.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));  // Подсветка найденных ячеек
                        }
                        else
                        {
                            cellContent.Background = Brushes.Transparent; // Очищаем подсветку
                        }
                    }
                }
            }
            foreach (var item in dbSessions.Items)
            {
                foreach (DataGridColumn column in dbSessions.Columns)
                {
                    var cellContent = column.GetCellContent(item) as TextBlock;

                    if (cellContent != null)
                    {
                        if (cellContent.Text.ToLower().StartsWith(searchText))
                        {
                            cellContent.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));  // Подсветка найденных ячеек
                        }
                        else
                        {
                            cellContent.Background = Brushes.Transparent; // Очищаем подсветку
                        }
                    }
                }
            }
            foreach (var item in dbOrders.Items)
            {
                foreach (DataGridColumn column in dbOrders.Columns)
                {
                    var cellContent = column.GetCellContent(item) as TextBlock;

                    if (cellContent != null)
                    {
                        if (cellContent.Text.ToLower().StartsWith(searchText))
                        {
                            cellContent.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));  // Подсветка найденных ячеек
                        }
                        else
                        {
                            cellContent.Background = Brushes.Transparent; // Очищаем подсветку
                        }
                    }
                }
            }
            foreach (var item in dbTickets.Items)
            {
                foreach (DataGridColumn column in dbTickets.Columns)
                {
                    var cellContent = column.GetCellContent(item) as TextBlock;

                    if (cellContent != null)
                    {
                        if (cellContent.Text.ToLower().StartsWith(searchText))
                        {
                            cellContent.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));  // Подсветка найденных ячеек
                        }
                        else
                        {
                            cellContent.Background = Brushes.Transparent; // Очищаем подсветку
                        }
                    }
                }
            }
            foreach (var item in dbPlaces.Items)
            {
                foreach (DataGridColumn column in dbPlaces.Columns)
                {
                    var cellContent = column.GetCellContent(item) as TextBlock;

                    if (cellContent != null)
                    {
                        if (cellContent.Text.ToLower().StartsWith(searchText))
                        {
                            cellContent.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007ACC"));  // Подсветка найденных ячеек
                        }
                        else
                        {
                            cellContent.Background = Brushes.Transparent; // Очищаем подсветку
                        }
                    }
                }
            }

        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem selectedTab)
            {
                string tabName = selectedTab.Header.ToString();

                switch (tabName)
                {
                    case "Клиенты":
                        FilterClients();
                        break;
                    case "Фильмы":
                        FilterMovies();
                        break;
                    case "Залы":
                        FilterHalls();
                        break;
                    case "Сеансы":
                        FilterSessions();
                        break;
                    case "Места":
                        FilterPlaces();
                        break;
                    case "Заказы":
                        FilterOrders();
                        break;

                    case "Билеты":
                        FilterTickets();
                        break;

                        /*Заказы,места и билеты*/
                }
            }
        }

        private void FilterClients()
        {
            string searchText = txtSearch.Text.ToLower();

            if (allClients != null)
            {
                var filteredItems = allClients.Where(item =>
                    item.Id.ToString().ToLower().Contains(searchText) ||
                    item.Phone.ToLower().Contains(searchText) ||
                    item.Email.ToString().Contains(searchText)).ToList();

                dbClients.ItemsSource = filteredItems;
            }
        }
        private void FilterHalls()
        {
            string searchText = txtSearch.Text.ToLower();

            if (allHalls != null)
            {
                var filteredItems = allHalls.Where(item =>
                    item.Id.ToString().ToLower().Contains(searchText) ||
                    item.Count_rows.ToString().ToLower().Contains(searchText) ||
                    item.Count_place_of_rows.ToString().Contains(searchText) ||
                    item.Name_hall.ToString().ToLower().Contains(searchText)).ToList();

                dbHalls.ItemsSource = filteredItems;
            }
        }
        private void FilterMovies()
        {
            string searchText = txtSearch.Text.ToLower();

            if (allMovies != null)
            {
                try
                {
                    var filteredItems = allMovies.Where(item =>
                        (item.genre != null && item.genre.ToLower().Contains(searchText)) ||
                        (item.timing != null && item.timing.ToLower().Contains(searchText)) ||
                        (item.name != null && item.name.ToLower().Contains(searchText)) ||
                        (item.country != null && item.country.ToString().ToLower().Contains(searchText)) ||
                        (item.year.ToString().Contains(searchText)) ||
                        (item.director != null && item.director.ToLower().Contains(searchText)) ||
                        (item.release_date != null && item.release_date.ToString("d.M.yyyy").ToLower().Contains(searchText))||
                        (item.price.ToString().ToLower().Contains(searchText))
                    ).ToList();

                    dbMovies.ItemsSource = filteredItems;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при фильтрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void FilterOrders()
        {
            string searchText = txtSearch.Text.ToLower();

            if (allOrders != null)
            {
                try
                {
                    var filteredItems = allOrders.Where(item =>
                                        item.Id.ToString().ToLower().Contains(searchText) ||
                        item.Id_client.ToString().Contains(searchText) ||
                        item.Id_payment_method.ToString().Contains(searchText) ||
                        item.Number_order.ToString().Contains(searchText) ||
                        item.Data_order != null && item.Data_order.ToString().ToLower().Contains(searchText) ||
                        item.payment_amount.ToString().Contains(searchText)
                    ).ToList();

                    dbOrders.ItemsSource = filteredItems;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при фильтрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void FilterTickets()
        {
            string searchText = txtSearch.Text.ToLower();

            if (allTickets != null)
            {
                try
                {
                    var filteredItems = allTickets.Where(item =>
                                        item.Id.ToString().ToLower().Contains(searchText) ||

                        item.Id_session.ToString().Contains(searchText) ||
                        item.Id_place.ToString().Contains(searchText) ||
                        item.Ticket_number.ToString().Contains(searchText) 
                    ).ToList();

                    dbTickets.ItemsSource = filteredItems;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при фильтрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void FilterSessions()
        {
            string searchText = txtSearch.Text.ToLower();

            if (allSessions != null)
            {
                var filteredItems = allSessions.Where(item =>
                    item.Id.ToString().ToLower().Contains(searchText) ||
                    item.Data_session.ToString("d.M.yyyy").ToLower().Contains(searchText) ||
                    item.Time_session.ToString("d.M.yyyy").Contains(searchText) ||
                    item.Id_hall.ToString().ToLower().Contains(searchText) ||
                    item.Id_movie.ToString().ToLower().Contains(searchText)).ToList();

                dbSessions.ItemsSource = filteredItems;
            }
        }
        private void FilterPlaces()
        {
            string searchText = txtSearch.Text.ToLower();

            if (allPlaces != null)
            {
                var filteredItems = allPlaces.Where(item =>
                    item.Id.ToString().ToLower().Contains(searchText) ||
                    item.Place_number.ToString().ToLower().Contains(searchText) ||
                    item.Id_category.ToString().Contains(searchText) ||
                    item.Id_status.ToString().Contains(searchText)).ToList();

                dbPlaces.ItemsSource = filteredItems;
            }
        }


        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }
        public void Clear(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }
        #endregion
    }
}