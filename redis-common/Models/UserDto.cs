using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redis_common.Models
{
    public record UserDto
    {
        public UserDto()
        {

        }

        public UserDto(string firstName, string lastName, DateTime dob)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Dob = dob;
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
    }
}
