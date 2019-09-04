using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCPOS.Entities
{
    class PodaciKalkulacije
    {
        public int brojKalkulacije { get; set; }
        public int sifraKalkulacije { get; set; }

        public PodaciKalkulacije(int i, int j)
        {
            brojKalkulacije = i;
            sifraKalkulacije = j;
        }
    }
}
