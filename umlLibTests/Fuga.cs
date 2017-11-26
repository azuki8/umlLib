using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLMappingCSTests
{
    class Fuga //  : IComparable
    {
        private string name = "";

        public int ID = 1;
        
        public Fuga()
        {

        }

        public Fuga(string _name)
        {
            name = _name;
        }

        //public int CompareTo(object obj)
        //{
        //    Fuga objFuga = obj as Fuga;

        //    if( objFuga == null)
        //    {
        //        throw new ArgumentNullException();
        //    }
        //    string objName = objFuga.name;
        //    return objName.CompareTo(this.name);
        //}

        public void Func()
        {

        }
    }
}
