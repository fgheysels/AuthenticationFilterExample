using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codit.SharedAccessKeyExample.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        public Customer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        public Customer(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
