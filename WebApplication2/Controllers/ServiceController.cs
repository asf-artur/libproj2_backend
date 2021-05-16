using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Users;
using WebApplication2.Repositories;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ServiceController : Controller
    {
        private readonly BookEventRepository _bookEventRepository;
        private readonly NotificationsRepository _notificationsRepository;
        private readonly NotificationService _notificationService;

        public ServiceController(BookEventRepository bookEventRepository,
            NotificationsRepository notificationsRepository,
            NotificationService notificationService)
        {
            _bookEventRepository = bookEventRepository;
            _notificationsRepository = notificationsRepository;
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult GetAllBookEvents()
        {
            return Json(_bookEventRepository.GetAll());
        }

        [HttpGet]
        // TODO: зачем это?
        public IActionResult GetAllNotifications()
        {
            return Json(_notificationsRepository.GetAll());
        }

        [HttpPost]
        public IActionResult CreateNewsNotification(
            [DefaultValue(new[] {UserCategory.Librarian})] List<UserCategory>? userCategoryList,
            [DefaultValue(null)] List<int>? userIdList,
            [DefaultValue("название")] string title,
            [DefaultValue("текст..........")] string text)
        {
            try
            {
                _notificationService.CreateNewsNotification(userCategoryList, userIdList, title, text);
            }
            catch
            {
                return Conflict();
            }

            return Accepted();
        }
    }
}