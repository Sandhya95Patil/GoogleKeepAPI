//-----------------------------------------------------------------------
// <copyright file="LabelBL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace BussinessLayer.Service
{
    using BussinessLayer.Interface;
    using CommonLayer.Model;
    using CommonLayer.ShowModel;
    using RepositoryLayer.Interface;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// LabelBL class
    /// </summary>
    public class LabelBL : ILabelBL
    {
        /// <summary>
        /// Inject the repository layer interafce
        /// </summary>
        private readonly ILabelRL labelRL;

        /// <summary>
        /// Initializes the memory for Label class
        /// </summary>
        /// <param name="labelRL">labelRL parameter</param>
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }


        /// <summary>
        /// Add Label method
        /// </summary>
        /// <param name="showLabel">showLabel parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <returns>return the added label</returns>
        public async Task<LabelModel> AddLabel(ShowLabelModel showLabel, int userId)
        {
            try
            {
                if (showLabel != null)
                {
                    return await labelRL.AddLabel(showLabel, userId);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// UpdateLabel method
        /// </summary>
        /// <param name="showLabelModel">showLabelModel parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <param name="labelId">labelId parameter</param>
        /// <returns>return updated label</returns>
        public async Task<LabelModel> UpdateLabel(ShowLabelModel showLabelModel, int userId, int labelId)
        {
            try
            {
                if(showLabelModel != null)
                {
                    return await labelRL.UpdateLabel(showLabelModel, userId, labelId);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Delete Label method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <param name="labelId">labelId parameter</param>
        /// <returns>return the deleted label</returns>
        public async Task<string> DeleteLabel(int userId, int labelId)
        {
            try
            {
                if (userId > 0 && labelId > 0)
                {
                    return await labelRL.DeleteLabel(userId, labelId);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Get All Labels method
        /// </summary>
        /// <param name="userId">userId parameter</param>
        /// <returns>return the all labels</returns>
        public async Task<IList<LabelModel>> GetAllLabels(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    return await labelRL.GetAllLabels(userId);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
