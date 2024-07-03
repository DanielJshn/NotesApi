using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace apief
{
    public class NoteControllerTests
    {
        private Mock<INoteService> _mockNoteService;
        private Mock<IMapper> _mockMapper;
        private Mock<IConfiguration> _mockConfig;
        private NoteController _controller;

        [SetUp]
        public void Setup()
        {
            _mockNoteService = new Mock<INoteService>();
            _mockMapper = new Mock<IMapper>();
            _mockConfig = new Mock<IConfiguration>();

            _controller = new NoteController(_mockNoteService.Object, _mockMapper.Object, _mockConfig.Object);
        }

        [Test]
        public void PostNote_ShouldReturnOkResult_WhenNoteIsAdded()
        {
            // Arrange
            var noteDto = new NoteDto { Title = "Test Note", Description = "This is a test note" };
            var note = new Note { Id = 1, Title = "Test Note", Description = "This is a test note" };
            _mockMapper.Setup(m => m.Map<NoteDto>(noteDto)).Returns(noteDto);
            _mockNoteService.Setup(s => s.AddNote(It.IsAny<int>(), noteDto)).Returns(note);

            // Act
            var result = _controller.PostNote(noteDto) as OkObjectResult;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(200, result?.StatusCode);
            ClassicAssert.AreEqual(note, result?.Value);
        }

        [Test]
        public void PostNote_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var noteDto = new NoteDto { Title = "Test Note", Description = "This is a test note" };
            _mockMapper.Setup(m => m.Map<NoteDto>(noteDto)).Returns(noteDto);
            _mockNoteService.Setup(s => s.AddNote(It.IsAny<int>(), noteDto)).Throws(new Exception("Error adding note"));

            // Act
            var result = _controller.PostNote(noteDto) as BadRequestObjectResult;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(400, result?.StatusCode);
            ClassicAssert.AreEqual("Error adding note", result?.Value);
        }

        [Test]
        public void UpdateNote_ShouldReturnOkResult_WhenNoteIsUpdated()
        {
            // Arrange
            var noteDto = new NoteDto { Title = "Updated Title", Description = "Updated Description" };
            _mockNoteService.Setup(s => s.UpdateNote(It.IsAny<int>(), It.IsAny<int>(), noteDto)).Verifiable();

            // Act
            var result = _controller.UpdateNote(1, noteDto) as OkResult;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public void UpdateNote_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var noteDto = new NoteDto { Title = "Updated Title", Description = "Updated Description" };
            _mockNoteService.Setup(s => s.UpdateNote(It.IsAny<int>(), It.IsAny<int>(), noteDto)).Throws(new Exception("Error updating note"));

            // Act
            var result = _controller.UpdateNote( 1, noteDto) as BadRequestObjectResult;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(400, result?.StatusCode);
            ClassicAssert.AreEqual("Error updating note", result?.Value);
        }

        [Test]
        public void GetSingleNote_ShouldReturnOkResult_WhenNoteIsRetrieved()
        {
            // Arrange
            var note = new Note { Id = 1, Title = "Test Note", Description = "This is a test note" };
            _mockNoteService.Setup(s => s.GetNoteById(It.IsAny<int>(), It.IsAny<int>())).Returns(note);

            // Act
            var result = _controller.GetSingleNote(1) as OkObjectResult;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(200, result?.StatusCode);
            ClassicAssert.AreEqual(note, result?.Value);
        }

        [Test]
        public void GetSingleNote_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            _mockNoteService.Setup(s => s.GetNoteById(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Error retrieving note"));

            // Act
            var result = _controller.GetSingleNote(1) as BadRequestObjectResult;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(400, result?.StatusCode);
            ClassicAssert.AreEqual("Error retrieving note", result?.Value);
        }

        [Test]
        public void GetNotes_ShouldReturnOkResult_WhenNotesAreRetrieved()
        {
            // Arrange
            var notes = new List<Note>
            {
                new Note { Id = 1, Title = "Note 1", Description = "Description 1" },
                new Note { Id = 2, Title = "Note 2", Description = "Description 2" }
            };
            _mockNoteService.Setup(s => s.GetNotesByUserId(It.IsAny<int>())).Returns(notes);

            // Act
            var result = _controller.GetNotes() as OkObjectResult;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(200, result?.StatusCode);
            ClassicAssert.AreEqual(notes, result?.Value);
        }

        [Test]
        public void GetNotes_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            _mockNoteService.Setup(s => s.GetNotesByUserId(It.IsAny<int>())).Throws(new Exception("Error retrieving notes"));

            // Act
            var result = _controller.GetNotes() as BadRequestObjectResult;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(400, result?.StatusCode);
            ClassicAssert.AreEqual("Error retrieving notes", result?.Value);
        }

        [Test]
        public void DeleteNote_ShouldReturnOkResult_WhenNoteIsDeleted()
        {
            // Arrange
            _mockNoteService.Setup(s => s.DeleteNoteById(It.IsAny<int>(), It.IsAny<int>())).Verifiable();

            // Act
            var result = _controller.DeleteNote( 1) as OkObjectResult;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(200, result?.StatusCode);
            ClassicAssert.AreEqual("Note Was Deleted", result?.Value);
        }

        [Test]
        public void DeleteNote_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            _mockNoteService.Setup(s => s.DeleteNoteById(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Error deleting note"));

            // Act
            var result = _controller.DeleteNote(1 ) as BadRequestObjectResult;

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(400, result?.StatusCode);
            ClassicAssert.AreEqual("Error deleting note", result?.Value);
        }
    }
}