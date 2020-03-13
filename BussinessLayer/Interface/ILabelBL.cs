//-----------------------------------------------------------------------
// <copyright file="ILabelBL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace BussinessLayer.Interface
{
    using CommonLayer.Model;
    using CommonLayer.ShowModel;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// ILabelBL interface
    /// </summary>
    public interface ILabelBL
    {
        /// <summary>
        /// Add Label method
        /// </summary>
        /// <param name="showLabel">showLabel parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <returns></returns>
        Task<LabelModel> AddLabel(ShowLabelModel showLabel, int userId);

        /// <summary>
        /// Update label method
        /// </summary>
        /// <param name="showLabelModel">showLabelModel</param>
        /// <param name="userId">userId parameter</param>
        /// <param name="labelId">labelId parameter</param>
        /// <returns></returns>
        Task<LabelModel> UpdateLabel(ShowLabelModel showLabelModel, int userId, int labelId);

        /// <summary>
        /// Delete Label method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="labelId">labelId parameter</param>
        /// <returns>return the deleted note</returns>
        Task<string> DeleteLabel(int userId, int labelId);

        /// <summary>
        /// Get All Labels method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return the all label</returns>
        Task<IList<LabelModel>> GetAllLabels(int userId);
    }
}
