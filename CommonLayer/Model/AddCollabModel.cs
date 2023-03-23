using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Model
{
    public class AddCollabModel
    {
        public long NoteId { get; set; }
        [RegularExpression(@"^[a-z0-9\w]{3,}@[-a-z0-9]{5,}\.[a-z]{2,5}$")]
        public string Email { get; set; }

    }
}
