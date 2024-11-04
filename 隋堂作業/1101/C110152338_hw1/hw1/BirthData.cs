using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

// BirthData.cs
namespace hw1
{
    public class BirthData
    {
        public int Year { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public BirthData()
        {
            Year = 0;
            Male = 0;
            Female = 0;
        }
        ~BirthData()
        {

        }
    }
}
