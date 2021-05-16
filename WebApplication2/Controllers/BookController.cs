using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Contracts;
using WebApplication2.Contracts.Users;
using WebApplication2.Exceptions;
using WebApplication2.Repositories;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BookController : Controller
    {
        private readonly string _contentRootPath;
        private readonly BookSearchService _bookSearchService;
        private readonly BookBorrowService _bookBorrowService;
        private readonly BookCopyRepository _bookCopyRepository;
        private readonly BookInfoRepository _bookInfoRepository;
        private readonly BookEventRepository _bookEventRepository;
        private readonly UserRepository _userRepository;

        public BookController(
            IWebHostEnvironment env,
            BookSearchService bookSearchService,
            BookBorrowService bookBorrowService,
            BookCopyRepository bookCopyRepository,
            BookInfoRepository bookInfoRepository,
            BookEventRepository bookEventRepository,
            UserRepository userRepository)
        {
            _bookSearchService = bookSearchService;
            _bookBorrowService = bookBorrowService;
            _bookCopyRepository = bookCopyRepository;
            _bookInfoRepository = bookInfoRepository;
            _bookEventRepository = bookEventRepository;
            _contentRootPath = env.ContentRootPath;
            _userRepository = userRepository;
        }

        [HttpGet]
        public void test()
        {
            _bookEventRepository.GetAll();
        }

        [HttpGet("~/api/[controller]/bookInfoImage/{bookInfoId}")]
        public IActionResult GetBookInfoImage(int bookInfoId)
        {
            var bookInfo = _bookInfoRepository.Get(bookInfoId);
            if (bookInfo != null && bookInfo?.ImagePath != null)
            {
                var ind = new FileInfo($"{_contentRootPath}\\Images\\{bookInfo.ImagePath}");
                var str = ind.OpenRead();
                var extension = ind.Extension.Replace(".", "");

                return File(str, $"image/{extension}");
            }

            return BadRequest("Image is null");
        }

        [HttpGet("~/api/[controller]/Images/{imagePath}")]
        public IActionResult GetBookInfoImageFromPath(string imagePath)
        {
            try
            {
                var path = Path.Combine(_contentRootPath, "Images", imagePath);
                var ind = new FileInfo(path);
                // var ind = new FileInfo($"{_contentRootPath}\\Images\\{imagePath}");
                // Console.WriteLine($"{ ind.Exists}");
                // Console.WriteLine($"{ ind.Directory}");
                var str = ind.OpenRead();
                var extension = ind.Extension.Replace(".", "");

                return File(str, $"image/{extension}");
            }
            catch
            {
                return BadRequest("Image is null");
            }
        }

        [Authorize]
        [HttpGet("~/geta")]
        public int GetA()
        {
            return -1;
        }

        [ActionName("haha")]
        [HttpGet]
        public IActionResult Login()
        {
            return Unauthorized("haha lol");
        }

        [HttpGet]
        public IActionResult GetBookCopy(int bookCopyId)
        {
            return Json(_bookCopyRepository.Get(bookCopyId));
        }

        [HttpGet]
        public IActionResult GetBookInfo(int bookInfoId)
        {
            var currentUser = GetCurrentUser();
            return Json(_bookInfoRepository.Get(bookInfoId));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBooksOnHands()
        {
            var currentUser = GetCurrentUser();
            var result = await _bookCopyRepository.GetOnHands(currentUser.Id);
            return Json(result);
        }

        [Authorize]
        [HttpGet("{readerUserId}")]
        public async Task<IActionResult> GetBooksOnHands(int readerUserId)
        {
            if (User.IsInRole(UserCategory.Admin.ToString()) || User.IsInRole(UserCategory.Librarian.ToString()))
            {
                var result = await _bookCopyRepository.GetOnHands(readerUserId);
                return Json(result);
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBooksHistory()
        {
            var currentUser = GetCurrentUser();
            var result = await _bookInfoRepository.GetBookHistory(currentUser.Id);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookInfo()
        {
            return Json( await _bookInfoRepository.GetAll());
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllBookCopy()
        {
            return Json( await _bookCopyRepository.GetAll());
        }

        [HttpGet("~/api/[controller]/searchBookInfo")]
        public async Task<IActionResult> SearchBookInfoInLocalCatalog(string searchTerm)
        {
            var result = await _bookInfoRepository.SearchBookInfoAsync(searchTerm);

            return Json(result);
        }

        [HttpGet("~/api/[controller]/searchBookCopy")]
        public async Task<IActionResult> SearchBookCopyInLocalCatalog(string searchTerm)
        {
            if (searchTerm == "")
            {
                var all = await _bookCopyRepository.GetAll();
                return Json(all);
            }
            var result = await _bookCopyRepository.SearchBookCopyAsync(searchTerm);

            return Json(result);
        }

        [HttpGet]
        public IActionResult SearchByBarcode(string barcode)
        {
            var result = _bookCopyRepository.SearchByBarcode(barcode);

            return Json(result);
        }

        [HttpGet]
        public IActionResult SearchByRfid(string rfid)
        {
            var result = _bookCopyRepository.SearchByRfid(rfid);

            return Json(result);
        }

        [HttpGet("searcha")]
        public async Task<IActionResult> SearchAsync(string searchTerm, CancellationToken cancellationToken)
        {
            try
            {
                var hht = HttpContext.RequestAborted;
                var res = await _bookSearchService.SearchInRslAsync(searchTerm, HttpContext.RequestAborted);
                return Json(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Problem();
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public void TryBorrowBook([DefaultValue(0)] int bookId, [DefaultValue(0)] int userId)
        {
            var currentUserAccountName = User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType);
            if (currentUserAccountName?.Value == userId.ToString())
            {
                _bookBorrowService.TryBorrowBook(bookId, userId);
                Console.WriteLine($"Начата попытка ВЗЯТЬ книгу {bookId} пользователем {userId}");
            }
            else
            {
                throw new UserRoleException();
            }
        }

        // [Authorize]
        [HttpPost]
        public void RejectBorrowBook([DefaultValue(0)] int bookId, [DefaultValue(0)] int userReaderId, int userLibrarianId)
        {
            if (User.IsInRole(UserCategory.Admin.ToString()) || User.IsInRole(UserCategory.Librarian.ToString()))
            {
                _bookBorrowService.RejectBorrowBook(bookId, userReaderId, userLibrarianId);
                Console.WriteLine($"Пользователю {userReaderId} ОТКАЗАНО в выдаче книги {bookId}. Библиотекарь {userLibrarianId} подтвердил");
            }
            else
            {
                throw new UserRoleException();
            }
        }


        // [Authorize]
        [HttpPost]
        public async Task CompleteBorrowBook([DefaultValue(0)] int bookId, [DefaultValue(0)] int userReaderId, int userLibrarianId)
        {
            if (User.IsInRole(UserCategory.Admin.ToString()) || User.IsInRole(UserCategory.Librarian.ToString()))
            {
                await _bookBorrowService.CompleteBorrowBook(bookId, userReaderId, userLibrarianId);
                Console.WriteLine($"Пользователь {userReaderId} ВЗЯЛ книгу {bookId}. Библиотекарь {userLibrarianId} подтвердил");
            }
            else
            {
                throw new UserRoleException();
            }
        }

        [Authorize]
        [HttpPost]
        public void ReturnBook([DefaultValue(0)] int bookId, [DefaultValue(0)] int userReaderId, int userLibrarianId)
        {
            _bookBorrowService.ReturnBook(bookId, userReaderId, userLibrarianId);
            Console.WriteLine($"Пользователь {userReaderId} ВЕРНУЛ книгу {bookId}. Библиотекарь {userLibrarianId} подтвердил");
        }

        [NonAction]
        private User? GetCurrentUser()
        {
            var currentUserAccountName = User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultNameClaimType);
            var user = _userRepository.AllUsers.Find(c => c.Id.ToString() == currentUserAccountName.Value);

            return user;
        }
    }
}