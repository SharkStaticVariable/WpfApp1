using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public class Clients : IEntity
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }
    public class Halls : IEntity
    {
        public int Id { get; set; }
        public int Count_rows { get; set; }
        public int Count_place_of_rows { get; set; }
        public string Name_hall { get; set; }

    }
    public class Movies : IEntity
    {
        public int Id { get; set; }
        public string genre { get; set; }
        public string timing { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public int year { get; set; }
        public string director { get; set; }
        public byte[] img { get; set; }
        public DateTime release_date { get; set; }
        public decimal price { get; set; }
    }
    public class Orders : IEntity
    {
        public int Id { get; set; }
        public int Id_client { get; set; }
        public int Id_payment_method { get; set; }
        public int Number_order { get; set; }
        public DateTime Data_order { get; set; }
        public decimal payment_amount { get; set; }

    }
    public class PaymentMethods : IEntity
    {
        public int Id { get; set; }
        public string Payment_method { get; set; }

    }
    public class Payments : IEntity
    {
        public int Id { get; set; }
        public decimal Payment_amount { get; set; }
        public DateTime Payment_date { get; set; }
        public int Id_payment_method { get; set; }

    }
    public class PlaceCategories : IEntity
    {
        public int Id { get; set; }
        public string type { get; set; }


    }
    public class Places : IEntity
    {
        public int Id { get; set; }
        public string Place_number { get; set; }
        public int Id_category { get; set; }
        public int Id_status { get; set; }

    }
    public class Sessions : IEntity
    {
        public int Id { get; set; }
        public DateTime Data_session { get; set; }
        public TimeSpan Time_session { get; set; }
        public int Id_hall { get; set; }
        public int Id_movie { get; set; }
    }
    public class Status : IEntity
    {
        public int Id { get; set; }
        public string status { get; set; }

    }
    public class TicketOrders : IEntity
    {
        public int Id { get; set; }
        public int Id_order { get; set; }
        public int Id_ticket { get; set; }

    }
    public class Tickets : IEntity
    {
        public int Id { get; set; }
        public int Id_session { get; set; }
        public int Id_place { get; set; }
        public int Ticket_number { get; set; }

    }
}
