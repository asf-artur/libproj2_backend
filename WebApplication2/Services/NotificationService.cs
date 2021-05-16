using System;
using System.Collections.Generic;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Books;
using WebApplication2.Contracts.Notifications;
using WebApplication2.Contracts.Notifications.Firebases;
using WebApplication2.Contracts.Users;
using WebApplication2.Repositories;
using Notification = WebApplication2.Contracts.Notifications.Notification;

namespace WebApplication2.Services
{
    public class NotificationService
    {
        private readonly UserRepository _userRepository;
        private readonly NotificationsRepository _notificationsRepository;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public NotificationService(UserRepository userRepository, NotificationsRepository notificationsRepository, IWebHostEnvironment env)
        {
            _userRepository = userRepository;
            _notificationsRepository = notificationsRepository;
            _env = env;
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile($"{_env.ContentRootPath}/firebase_key.json")
            });
        }

        public void CreateNewsNotification(List<UserCategory>? userCategoryList, List<int>? userIdList, string title, string text)
        {
            var newId = _notificationsRepository.GetAll().Count;
            var notification = new Notification(newId, title, text, userCategoryList, userIdList, NotificationType.News);

            _notificationsRepository.Add(notification);

            var users = _userRepository.GetAllLibrarians();

            users
                .FindAll(c => c.ClientToken != null)
                .ForEach(
                    c => SendNotificationMessageAsync(c.ClientToken, title, text));
        }

        public void TryBorrowBookNotification(BookCopy bookCopy, User user)
        {
            var librarians = _userRepository.GetAllLibrarians();

            var firebaseDm = new FirebaseDm(NotificationType.TryBookBorrow, user.Id, bookCopy.Id);
            var json = JsonConvert.SerializeObject(firebaseDm);
            var dict = new Dictionary<string, string>
            {
                {"data", json}
            };
            librarians
                .FindAll(c => c.ClientToken != null)
                .ForEach(
                c => SendDataMessageAsync(c.ClientToken, dict));
        }

        public void RejectBorrowBookNotification(BookCopy bookCopy, User userReader, User userLibrarian)
        {
            var firebaseDm = new FirebaseDmWLibrarian(NotificationType.TryBookBorrowFailure, userReader.Id, bookCopy.Id, userLibrarian.Id);

            SendDataMessageAsync(userReader.ClientToken, firebaseDm.Data);
        }

        public void CompleteBorrowBookNotification(BookCopy bookCopy, User userReader, User userLibrarian)
        {
            var firebaseDmReader = new FirebaseDmWLibrarian(NotificationType.BookIsBorrowed, userLibrarian.Id, bookCopy.Id, userLibrarian.Id);
            var json = JsonConvert.SerializeObject(firebaseDmReader);
            var dict = new Dictionary<string, string>
            {
                {"data", json}
            };

            SendDataMessageAsync(userReader.ClientToken, dict);

            var librarians = _userRepository.GetAllLibrarians();

            librarians
                .FindAll(c => c.ClientToken != null)
                .ForEach(
                c => SendDataMessageAsync(c.ClientToken, dict));
        }

        public void ReturnBookNotification(BookCopy bookCopy, User userReader, User userLibrarian)
        {
            var firebaseDm = new FirebaseDmWLibrarian(NotificationType.BookReturned, userReader.Id, bookCopy.Id, userLibrarian.Id);
            var json = JsonConvert.SerializeObject(firebaseDm);
            var dict = new Dictionary<string, string>
            {
                {"data", json}
            };

            SendDataMessageAsync(userReader.ClientToken, dict);
        }

        private async void SendDataMessageAsync(string clientToken, Dictionary<string, string> data)
        {
            var message = new Message()
            {
                Token = clientToken,
                Data = data,
            };

            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine("Firebase Done");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }

        private async void SendNotificationMessageAsync(string clientToken, string title, string text)
        {
            var message = new Message()
            {
                Token = clientToken,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = text
                }
            };

            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine("Firebase Done");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }
    }
}