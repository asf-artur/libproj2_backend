using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Contracts.Users
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UserCategory UserCategory{ get; set; }

        public string? Barcode{ get; set; }

        public string? Rfid{ get; set; }

        public bool CanBorrowBooks{ get; set; }

        public DateTime RegistrationDate{ get; set; }

        public DateTime? LastVisitDate{ get; set; }

        public string? ImagePath{ get; set; }

        public string? ClientToken { get; set; }

        public User()
        {
        }

        public User(int id, string name, UserCategory userCategory, string? barcode, string? rfid, bool canBorrowBooks, DateTime registrationDate, DateTime lastVisitDate, string? imagePath, string? clientToken)
        {
            Id = id;
            Name = name;
            UserCategory = userCategory;
            Barcode = barcode;
            Rfid = rfid;
            CanBorrowBooks = canBorrowBooks;
            RegistrationDate = registrationDate;
            LastVisitDate = lastVisitDate;
            ImagePath = imagePath;
            ClientToken = clientToken;
        }
    }
}