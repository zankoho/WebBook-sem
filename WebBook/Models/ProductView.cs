namespace WebBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

    public class ProductView
    {
        [DisplayName("Category")]
        public IEnumerable<SelectListItem> ListofCategory { get; set; }

    }

