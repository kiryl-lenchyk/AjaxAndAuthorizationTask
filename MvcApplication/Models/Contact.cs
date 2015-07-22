using System;
using System.Collections.Generic;

namespace MvcApplication.Models
{

    [Serializable]
    public class Address
    {
        private string city;
        private string country;

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }
    }

    [Serializable]
    public class Contact
    {
        private string name;
        private string surname;
        private int age;
        private Address address;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public Address Address
        {
            get { return address; }
            set { address = value; }
        }
    }
}