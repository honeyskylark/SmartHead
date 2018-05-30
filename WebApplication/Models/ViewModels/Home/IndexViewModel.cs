using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<Feedback> Feedbacks { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
