using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Response
{
    public class CollaboratorListModel
    {
        public int CreatedId { get; set; }
        public int NoteId { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverProfile { get; set; }
    }
}
