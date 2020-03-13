using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Response
{
    public class UpdateResponseModel
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int UserId { get; set; }
    }
}
