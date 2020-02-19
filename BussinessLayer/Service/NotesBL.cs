using BussinessLayer.Interface;
using CommonLayer.Model;
using CommonLayer.ShowModel;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class NotesBL : INotesBL
    {
        private readonly INotesRL noteRL;
        public NotesBL (INotesRL noteRL)
        {
            this.noteRL = noteRL;
        }
        public Task<NoteModel> AddNote(ShowNoteModel showNoteModel, int userId)
        {
            try
            {
                return noteRL.AddNote(showNoteModel, userId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public Task<NoteModel> UpdateNote(ShowNoteModel showNoteModel, int userId, int noteId)
        {
            try
            {
                return noteRL.UpdateNote(showNoteModel, userId, noteId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
