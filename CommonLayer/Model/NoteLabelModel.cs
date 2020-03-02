using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class NoteLabelModel
    {
        /// <summary>
        /// Get and set the id
        /// </summary>

        /// <summary>
        /// Get and set the label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Get and set the created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Get and set the modified date
        /// </summary>
        public DateTime ModifiedDate { get; set; }
        public int NoteId { get; set; }
        public int LabelId { get; set; }

        /// <summary>
        /// Get and set the user id
        /// </summary>
        public int UserId { get; set; }
    }
}
