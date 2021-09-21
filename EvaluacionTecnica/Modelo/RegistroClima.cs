using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class RegistroClima
    {
        public int id { get; set; }
        public Ciudad ciudad { get; set; }
        public string temperatura { get; set; }
        public string termica { get; set; }
    }
}
