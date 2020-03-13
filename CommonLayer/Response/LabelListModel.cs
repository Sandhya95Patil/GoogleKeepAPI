using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Response
{
    public class LabelListModel
    {
        public string Label { get;set;}
        public int LabelId { get; set; }
        public int NoteId { get; set; }
        public int userId { get; set; }
    }
}
