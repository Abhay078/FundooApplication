using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class LabelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LabelId { get; set; }
        public long NoteId { get; set; }
        [ForeignKey("NoteId")]
        public NotesEntity NotesEntity { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public UserEntity UserEntity { get; set; }

        
        public string LabelName { get; set; }

    }
}
