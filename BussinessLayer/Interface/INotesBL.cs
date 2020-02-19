using CommonLayer.Model;
using CommonLayer.ShowModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interface
{
    public interface INotesBL
    {
        Task<NoteModel> AddNote(ShowNoteModel showNoteModel, int userId);
        Task<NoteModel> UpdateNote(ShowNoteModel showNoteModel, int userId, int noteId);
    }
}
