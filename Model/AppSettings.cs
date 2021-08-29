using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Model
{
  public static class AppSettings
    {
        public static string DbConnection { get; set; }

        public static string fileformat { get; set; }
    }
}

