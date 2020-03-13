//-----------------------------------------------------------------------
// <copyright file="NoteModel.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace CommonLayer.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Note Model class
    /// </summary>
    public class NoteModel
    {
        /// <summary>
        /// Get and set the id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get and set the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get and set the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get and set the reminder
        /// </summary>
        public DateTime? Reminder { get; set; }

        /// <summary>
        /// Get and set the color
        /// </summary>
        [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid Format")]
        public string Color { get; set; }

        /// <summary>
        /// Get and set the image
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Get and set the created date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Get and set the modified date
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Get and set the is pin
        /// </summary>
        public bool IsPin { get; set; }

        /// <summary>
        /// Get and set the is archive
        /// </summary>
        public bool IsArchive { get; set; }

        /// <summary>
        /// Get and set the is trash
        /// </summary>
        public bool IsTrash { get; set; }

        /// <summary>
        /// Get and set the user id
        /// </summary>
        public int UserId { get; set; }
    }
}
