using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public Customer? Customer { get; set; }

        public Person() { }

        public Person(int Id, string name, string surName, DateTime birthDate) : base(Id)
        {
            Name = name;
            Surname = surName;
            BirthDate = birthDate;
        }
    }
}
