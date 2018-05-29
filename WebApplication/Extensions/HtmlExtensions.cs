using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Extensions
{
    public static class HtmlExtensions
    {
        public static int CalculateAvarage(IEnumerable<Vote> votes)
        {
            if(votes.Count() != 0)
            {
                int x = 0;
                foreach (var vote in votes)
                {
                    x += vote.RatingGiven;
                }

                return x / votes.Count();
            }
            return 0;
        }
    }
}
