using System;
using System.Collections.Generic;
using System.Text;

namespace PingPongClientTcp
{
    public class Person
    {
        public string _name;
        public int _age;
        public Person()
        {

        }
        public Person(string name, int age)
        {
            _name = name;
            _age = age;
        }

        public override string ToString()
        {
            return $"{_name} is {_age} years old";
        }
    }

}

