using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class CustomerModel : AbstractModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public int DiscountValue { get; set; }

        public ICollection<int> ReceiptsIds { get; set; }

        public CustomerModel() { }

        public CustomerModel(int id, 
            string name, 
            string surname, 
            DateTime birthDate,
            ICollection<int> receiptsIds) : base(id)
        {
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            ReceiptsIds = receiptsIds;
        }
    }
}
