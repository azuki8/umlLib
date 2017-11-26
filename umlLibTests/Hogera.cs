using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLMappingCSTests
{
    enum PiyoPiyo
    {
        PIYO_1,
        PIYO_2,
        PIYO_3,
        PIYO_4,
        PIYO_5,
    }

    class Hogera : IComparable
    {
        private string name = "";

        public string Name { set { name = value; } get { return name; } }

        public int ID
        {
            set;get;
        }

        public PiyoPiyo PiyoPiyoEnum { set; get; }

        public Hogera(string _name)
        {
            name = _name;
        }

        public Hogera(string _name,PiyoPiyo _piyopiyo) : this(_name)
        {
            PiyoPiyoEnum = _piyopiyo;
        }

        public int CompareTo(object obj)
        {
            Hogera objFuga = obj as Hogera;

            if (objFuga == null)
            {
                throw new ArgumentNullException();
            }
            string objName = objFuga.name;
            return this.name.CompareTo(objName);
        }

        public void Func()
        {

        }
    }

}
