using System.Collections.Generic;
using MvcApplication.Models;

namespace MvcApplication.ViewModels
{
    public class JsonActionPageViewModel
    {
        public int PagesCount { get; set; }

        public int PageNumber { get; set; }

        public IList<Contact> Contacts { get; set; }

        public IEnumerable<string> Comments { get; set; }
    }
}