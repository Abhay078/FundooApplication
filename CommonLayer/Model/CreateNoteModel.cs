using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class CreateNoteModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }
        //public string BackgroundColor { get; set; }
        //public string Image { get; set; }
        public bool Archived { get; set; } = true;
        public bool Trash { get; set; } = true;
        public bool IsPinned { get; set; } = true;
        
    }
}
