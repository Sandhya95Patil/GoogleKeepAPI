//-----------------------------------------------------------------------
// <copyright file="LabelModel.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace CommonLayer.Model
{
    using System;

    /// <summary>
    /// Label Model class
    /// </summary>
    public class LabelModel
    {
        /// <summary>
        /// Get and set the id
        /// </summary>
        public int Id { get; set; }

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

        /// <summary>
        /// Get and set the user id
        /// </summary>
        public int UserId { get; set; }
    }
}
