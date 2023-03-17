using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class NotesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NoteId { get; set; }
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity UserEntity { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }
        public string BackgroundColor { get; set; }
        public string Image { get; set; }
        public bool Archived { get; set; }
        public bool Trash { get; set; }
        public bool IsPinned { get; set; }
        public DateTime Created  { get; set; }
        public DateTime Edited { get; set; }
    }
}
