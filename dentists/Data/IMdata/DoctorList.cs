using dentists.Page.doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dentists.Data.IMdata
{
    class DoctorList
    {
        public List<ClassList> doclist;
        public int totalunread;
        public DoctorList(List<ClassList> a) 
        {
            doclist = a;
        }
    }
}
