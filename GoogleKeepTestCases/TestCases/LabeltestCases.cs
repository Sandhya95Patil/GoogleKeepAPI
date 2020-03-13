//-----------------------------------------------------------------------
// <copyright file="LabeltestCases.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace GoogleKeepTestCases.TestCases
{
    using BussinessLayer.Interface;
    using BussinessLayer.Service;
    using CommonLayer.ShowModel;
    using GoogleKeepAPI.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using RepositoryLayer.Interface;
    using Xunit;

    /// <summary>
    /// Label test Cases class
    /// </summary>
    public class LabeltestCases
    {
        /// <summary>
        /// Inject the label controller class
        /// </summary>
        LabelController labelController;

        /// <summary>
        /// Inject the business layer interface
        /// </summary>
        ILabelBL labelBL;

        /// <summary>
        /// Initializes the memory for label test cases
        /// </summary>
        public LabeltestCases()
        {
            var repository = new Mock<ILabelRL>();
            this.labelBL = new LabelBL(repository.Object);
            labelController = new LabelController(this.labelBL);
        }
        
        /// <summary>
        /// Check valid value for add label
        /// </summary>
        [Fact]
        public void NotOkResult_AddLabel()
        {
            var model = new ShowLabelModel()
            {
                Label = "asdf"
            };
            var response = labelController.AddLabel(model);
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Check invalid value for add label
        /// </summary>
        [Fact]
        public void NotBadResult_AddLabel()
        {
            var model = new ShowLabelModel()
            {
                Label = "asd"
            };
            var response = labelController.AddLabel(model);
            Assert.IsNotType<BadRequestResult>(response);
        }

        [Fact]
        public void NotNull_AddNote()
        {
            var model = new ShowLabelModel()
            {
                Label = "asd"
            };
            var response = labelController.AddLabel(model);
            Assert.NotNull(response);
        }

        /// <summary>
        /// Check Valid value for update label
        /// </summary>
        [Fact]
        public void NotOkResult_UpdateLabel()
        {
            var model = new ShowLabelModel()
            {
                Label = "aAASDF"
            };
            var response = labelController.UpdateLabel(model, 1);
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Check invalid value for update label
        /// </summary>
        [Fact]
        public void NotBadResult_UpdateLabel()
        {
            var model = new ShowLabelModel()
            {
                Label = "asd"
            };
            var response = labelController.UpdateLabel(model, 1);
            Assert.IsNotType<BadRequestResult>(response);
        }

        /// <summary>
        /// Check valid for delete label
        /// </summary>
        [Fact]
        public void NotOkResult_DeleteLabel()
        {
            var response = labelController.DeleteLabel(1);
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Check invalid value for delete label
        /// </summary>
        [Fact]
        public void NotBadResult_DeleteLabel()
        {
            var response = labelController.DeleteLabel(1);
            Assert.IsNotType<BadRequestResult>(response);
        }

        [Fact]
        public void NotNull_DeleteLabel()
        {
            var response = labelController.DeleteLabel(1);
            Assert.NotNull(response);
        }

        /// <summary>
        /// Check label list result
        /// </summary>
        [Fact]
        public void LabelList_ReturnsOk()
        {
            var response = labelController.GetAllLabels();
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Check return label list result
        /// </summary>
        [Fact]
        public void LabelList_ReturnsNotOk()
        {
            var response = labelController.GetAllLabels();
            Assert.IsNotType<BadRequestResult>(response);
        }
    }
}
