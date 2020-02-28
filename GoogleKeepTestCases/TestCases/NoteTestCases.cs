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
    using CommonLayer.Model;
    using CommonLayer.ShowModel;
    using GoogleKeepAPI.Controllers;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using RepositoryLayer.Interface;
    using System;
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
        /// Inject Interface of form file class
        /// </summary>
      //  private readonly IFormFile formFile;

        /// <summary>
        /// Inject the bussiness layer interface
        /// </summary>
        INotesBL notesBL;

        /// <summary>
        /// Initializes the memory for note test cases
        /// </summary>
        public NoteTestCases()
        {
           // this.formFile = formFile;
            var repository = new Mock<INotesRL>();
            this.notesBL = new NotesBL(repository.Object);
            noteController = new NoteController(this.notesBL);
        }

        /// <summary>
        /// Check Valid Value method 
        /// </summary>
        [Fact]
        public void CheckNotNull_AddNote()
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
            Assert.NotNull(model);
        }

        /// <summary>
        /// Check Invalid Value 
        /// </summary>
        [Fact]
        public void NotOkResult_AddNote()
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

        [Fact]
        public void NotBadResult_AddNote()
        {
            var model = new ShowNoteModel()
            {
                Title = "",
                Color = "",
                Image = "",
                IsArchive = false,
                IsPin = true,
                IsTrash = false,
            };
            var response = noteController.AddNote(model);
            Assert.IsNotType<BadRequestResult>(response);
        }

        /// <summary>
        /// Check valid value method
        /// </summary>
        [Fact]
        public void NotNull_UpdateNote()
        {
            var model = new ShowUpdateNoteModel()
            {
                Title = "asd",
                Description = "asd"
            };
            var response = noteController.UpdateNote(model, 1);
            Assert.NotNull(response);
        }

        /// <summary>
        /// check invalid value
        /// </summary>
        [Fact]
        public void NotOkResult_UpdateNote()
        {
            var model = new ShowUpdateNoteModel()
            {
                Title = "asd",
                Description = "sdfg"
            };
            var response = noteController.UpdateNote(model, 4002);
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Check valid id 
        /// </summary>
        [Fact]
        public void NotBadResult_DeleteNote()
        {
            var response = noteController.DeleteNote(4002);
            Assert.IsNotType<BadRequestResult>(response);
        }

        /// <summary>
        /// Check invalid id
        /// </summary>
        [Fact]
        public void NotOkResult_DeleteNote()
        {
            var response = noteController.DeleteNote(4002);
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Check note list result
        /// </summary>
        [Fact]
        public void NotBadResult_ReturnsOk()
        {
            var response = noteController.GetAllNotes();
            Assert.IsNotType<BadRequestResult>(response);
        }

        /// <summary>
        /// check note list result
        /// </summary>
        [Fact]
        public void NotOkResult_ReturnsNotOk()
        {
            var response = noteController.GetAllNotes();
            Assert.IsNotType<OkObjectResult>(response);
        }

        [Fact]
        public void NotBadResult_ArchiveNote()
        {
            var response = noteController.ArchiveNote(4002);
            Assert.IsNotType<BadRequestResult>(response);
        }

        [Fact]
        public void NotOkResult_ArchiveNote()
        {
            var response = noteController.ArchiveNote(4002);
            Assert.IsNotType<OkObjectResult>(response);
        }

        [Fact]
        public void NotBadResult_TrashNote()
        {
            var response = noteController.TrashNote(4002);
            Assert.IsNotType<BadRequestResult>(response);
        }

        [Fact]
        public void NotOkResult_TrashNote()
        {
            var response = noteController.TrashNote(4002);
            Assert.IsNotType<OkObjectResult>(response);
        }

        [Fact]
        public void NotBadResult_PinNote()
        {
            var response = noteController.PinNote(4002);
            Assert.IsNotType<BadRequestResult>(response);
        }

        [Fact]
        public void NotOkResult_PinNote()
        {
            var response = noteController.PinNote(4002);
            Assert.IsNotType<OkObjectResult>(response);
        }

        [Fact]
        public void NotBadResult_GetAllPin()
        {
            var response = noteController.GetAllPinNotes();
            Assert.IsNotType<BadRequestResult>(response);
        }

        [Fact]
        public void NotOkResult_GetAllPin()
        {
            var response = noteController.GetAllPinNotes();
            Assert.IsNotType<OkObjectResult>(response);
        }

        [Fact]
        public void NotBadResult_GetAllTrash()
        {
            var response = noteController.GetAllTrashNotes();
            Assert.IsNotType<BadRequestResult>(response);
        }

        [Fact]
        public void NotOkResult_GetAllTrash()
        {
            var response = noteController.GetAllTrashNotes();
            Assert.IsNotType<OkObjectResult>(response);
        }

        [Fact]
        public void NotBadResult_GetAllArchive()
        {
            var response = noteController.GetAllArchiveNotes();
            Assert.IsNotType<BadRequestResult>(response);
        }

        [Fact]
        public void NotOkResult_GetAllArchive()
        {
            var response = noteController.GetAllArchiveNotes();
            Assert.IsNotType<OkObjectResult>(response);
        }

        [Fact]
        public void NotBadResult_ChangeColor()
        {
            var model = new ColorModel()
            {
                Color = "#000000"
            };
            var response = noteController.ChangeColor(4002,model);
            Assert.IsNotType<BadRequestResult>(response);
        }

        [Fact]
        public void NotOkResult_ChangeColor()
        {
            var model=new ColorModel()
            {
                Color=""
            };
            var response = noteController.ChangeColor(4002, model);
            Assert.IsNotType<OkObjectResult>(response);
        }
    }
}
