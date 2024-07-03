using AutoMapper;

namespace apief
{
    public class TestableNoteController : NoteController
    {
        public TestableNoteController(IConfiguration config, NoteService notesService, IMapper _mapper)
         : base(notesService, _mapper, config)
        {
        }

        protected override int GetUserId()
        {
            // Возвращаем фиксированное значение для тестов
            return 1;
        }
    }
}