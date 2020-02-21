//-----------------------------------------------------------------------
// <copyright file="NoteTestCases.cs" company="BridgeLabz">
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
    /// Note Test Cases class
    /// </summary>
    public class NoteTestCases
    {
        /// <summary>
        /// Inject the note controller class
        /// </summary>
        NoteController noteController;

        /// <summary>
        /// Inject the bussiness layer interface
        /// </summary>
        INotesBL notesBL;

        /// <summary>
        /// Initializes the memory for note test cases
        /// </summary>
        public NoteTestCases()
        {
            var repository = new Mock<INotesRL>();
            this.notesBL = new NotesBL(repository.Object);
            noteController = new NoteController(this.notesBL);
        }

        /// <summary>
        /// Check Valid Value method 
        /// </summary>
        [Fact]
        public void ValidValue_AddNote()
        {
            var model = new ShowNoteModel()
            {
                Title = "abc",
                Description = "xyz",
                Color="red",
                Image="rose",
                IsPin=true,
                IsArchive=false,
                IsTrash=false,
            };
            var response = noteController.AddNote(model);
            Assert.IsType<OkObjectResult>(response);
        }

        /// <summary>
        /// Check Invalid Value 
        /// </summary>
        [Fact]
        public void InvalidValue_AddNote()
        {
            var model = new ShowNoteModel()
            {
                Title = "abc",
                Description = "xyz",
                Color = "red",
                Image = "rose",
                IsPin = true,
                IsArchive = false,
                IsTrash = false,
            };
            var response = noteController.AddNote(model);
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Check valid value method
        /// </summary>
        [Fact]
        public void ValidValue_UpdateNote()
        {
            var model = new ShowUpdateNoteModel()
            {
                Title = "asd",
                Description = "asd"
            };
            var response = noteController.UpdateNote(model, 1);
            Assert.IsType<OkObjectResult>(response);
        }

        /// <summary>
        /// check invalid value
        /// </summary>
        [Fact]
        public void InvalidValue_UpdateNote()
        {
            var model = new ShowUpdateNoteModel()
            {
                Title = "asd",
                Description = "sdfg"
            };
            var response = noteController.UpdateNote(model, 1);
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Check valid id 
        /// </summary>
        [Fact]
        public void ValidId_DeleteNote()
        {
            var response = noteController.DeleteNote(1);
            Assert.IsType<OkObjectResult>(response);
        }

        /// <summary>
        /// Check invalid id
        /// </summary>
        [Fact]
        public void InvalidId_DeleteNote()
        {
            var response = noteController.DeleteNote(1);
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Check note list result
        /// </summary>
        [Fact]
        public void NotesList_ReturnsOk()
        {
            var response = noteController.GetAllNotes();
            Assert.IsType<OkObjectResult>(response);
        }

        /// <summary>
        /// check note list result
        /// </summary>
        [Fact]
        public void NoteList_ReturnsNotOk()
        {
            var response = noteController.GetAllNotes();
            Assert.IsNotType<OkObjectResult>(response);
        }
    }
}
