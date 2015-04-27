using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dentists.Page.im.IMdata
{
    class WordList
    {
        public List<Msg> wordlist;
        public int wordnum;
        public WordList(List<Msg> a) 
        {
            this.wordlist = a;
            wordnum = 0;
        }
    }
}
