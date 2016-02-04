using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _360_Staff_Survey_Web.Class
{
    public class staffinfo
    {
        private string name;
        private string designation;
        private string section;
        private string function;
        private string uid;
        private string role;

        public staffinfo(string name, string designation, string section, string function, string uid, string role)
        {
            this.name = name;
            this.designation = designation;
            this.section = section;
            this.function = function;
            this.uid = uid;
            this.role = role;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Designation
        {
            get { return designation; }
            set { designation = value; }
        }
        public string Section
        {
            get { return section; }
            set { section = value; }
        }
        public string Function
        {
            get { return function; }
            set { function = value; }
        }
        public string Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        public string Role
        {
            get { return role; }
            set { role = value; }
        }
    }
}