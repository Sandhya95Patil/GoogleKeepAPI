using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class AddLabelByLabelId
    {
        public int LabelId { get; set; }
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public string Label { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get;set; }
    }
}
