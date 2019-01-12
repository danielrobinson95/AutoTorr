using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDownloader.Models.UIPath
{
    public class Environment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Robot[] Robots { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }
    }
}
