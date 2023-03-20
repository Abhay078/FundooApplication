using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Model
{
    public class UserRegModel
    {
        [RegularExpression(@"[A-Z]{1}[a-z]{1,}")]
        public string FirstName { get; set; }
        [RegularExpression(@"[A-Z]{1}[a-z]{1,}")]
        public string LastName { get; set; }
        [RegularExpression(@"^[a-z0-9\w]{3,}@[-a-z0-9]{5,}\.[a-z]{2,5}$")]
        public string Email { get; set; }
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8}")]
        public string Password { get; set; }
    }
}
