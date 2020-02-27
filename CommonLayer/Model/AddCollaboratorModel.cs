using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class AddCollaboratorModel
    {
        public int Id { get; set; }
        public int NoteId { get; set; }
        public int CreatedId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverProfile { get; set; }
    }
}
