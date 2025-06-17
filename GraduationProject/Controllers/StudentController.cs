using GraduationProject.DTOs;
using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet("GetAvailableCoordinators")]
        public async Task<ActionResult<List<ProfessorDto>>> GetAvailableCoordinatorsAsync(int studentSpecializationId)
        {

            SpecializationModel specialization = await _context.Specializations.FindAsync(studentSpecializationId);

            //var department = _context.Departments.Where(d => d.Id.Equals(specialization.DeparmentId));
            var professorsList = await _context.Professors.Where(p => p.DepartmentId.Equals(specialization.DeparmentId) && p.NumberOfCoordinatedProjects < 20).ToListAsync();

            List<ProfessorDto> professorsDto = new List<ProfessorDto>();

            foreach(ProfessorModel professor in professorsList)
            {
                ProfessorDto p = new ProfessorDto
                {
                    FirstName = professor.FirstName,
                    SecondName = professor.SecondName,
                    Email = professor.Email,
                    NumberOfCoordinatedProjects = professor.NumberOfCoordinatedProjects,
                    DepartmentId = professor.DepartmentId
                };

                professorsDto.Add(p);
            }

            return Ok(professorsDto);
        }

        [HttpGet("GetProjectIdeasOfSelectedProfessor/{professsorId}")]
        public async Task<ActionResult<List<NewProjectIdeaDto>>> GetProjectIdeasOfSelectedProfessor(int professsorId)
        {
            var professorProjectList = await _context.ProfessorsProjectIdeas.Where(pr => pr.ProfessorId.Equals(professsorId)).ToListAsync();

            List<NewProjectIdeaDto> projectIdeaDtos = new List<NewProjectIdeaDto>();

            foreach(ProjectIdeaModel project in professorProjectList)
            {
                NewProjectIdeaDto projectIdeaDto = new NewProjectIdeaDto
                {
                    Title = project.Title,
                    Description = project.Description,
                    LevelOfEducation = project.LevelOfEducation,
                    ProfessorId = project.ProfessorId
                };

                projectIdeaDtos.Add(projectIdeaDto);
            }

            return Ok(projectIdeaDtos);
        }

        //[HttpGet("GetProjectRequest/{studentId}")]
        //public async Task<ActionResult<ProjectRequestModel>> GetProjectRequest(int studentId)
        //{
        //    var projectRequest = _context.ProjectRequests.Where(pr => pr.StudentId == studentId);
        //    return Ok(projectRequest);
        //}

        [HttpGet("GetProjectRequest/{studentId}")]
        public async Task<ActionResult<ProjectRequestDto>> GetProjectRequest(int studentId)
        {
            ProjectRequestModel projectRequest = _context.ProjectRequests.FirstOrDefault(pr => pr.StudentId == studentId);

            ProjectRequestDto projectRequestDto = new ProjectRequestDto
            {
                Title = projectRequest.Title,
                Description = projectRequest.Description,
                LevelOfEducation = projectRequest.LevelOfEducation,
                StudentId = projectRequest.StudentId,
                ProfessorId = projectRequest.ProfessorId
            };

            return Ok(projectRequestDto);
        }

        [HttpPost("PostProjectRequest")]
        public async Task<ActionResult> PostProjectRequestAsync([FromBody] ProjectRequestDto projectRequestDto)
        {
            
                StudentModel student = await _context.Students.FindAsync(projectRequestDto.StudentId);

                if (student == null)
                    return NotFound();

                var projectRequestModel = new ProjectRequestModel
                {
                    Title = projectRequestDto.Title,
                    Description = projectRequestDto.Description,
                    IsAcceptedByProfessor = false,
                    LevelOfEducation = student.LevelOfEducation,
                    StudentId = projectRequestDto.StudentId,
                    ProfessorId = projectRequestDto.ProfessorId,
                };

                _context.ProjectRequests.Add(projectRequestModel);
                await _context.SaveChangesAsync();

                return Created();
            
        }

        [HttpPut("UpdateProjectRequest")]
        public async Task<ActionResult> UpdateProjectRequestAsync([FromBody] UpdateProjectRequestDto updateProjectRequestDto)
        {
            ProjectRequestModel projectRequestModel = await _context.ProjectRequests.Where(p => p.Title == updateProjectRequestDto.ProjectRequestTitle).FirstOrDefaultAsync();

            if (projectRequestModel == null)
                return NotFound();

            projectRequestModel.StudentId = updateProjectRequestDto.StudentId;
            _context.Entry(projectRequestModel).Property(p => p.StudentId).IsModified = true;

            StudentModel student = await _context.Students.FindAsync(updateProjectRequestDto.StudentId);

            if (student == null)
                return NotFound();

            student.ProjectRequestId = projectRequestModel.Id;
            _context.Entry(student).Property(s => s.ProjectRequestId).IsModified = true;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("GetStudentByEmail")]
        public async Task<ActionResult<StudentDto>> GetStudentByEmail([FromBody] EmailRequest emailRequest)
        {
            StudentModel studentModel = await _context.Students.Where(s => s.Email == emailRequest.Email).FirstAsync();

            if (studentModel == null)
                return NotFound("Student not found");

            StudentDto studentDto = new StudentDto
            {
                Id = studentModel.Id,
                FirstName = studentModel.FirstName,
                SecondName = studentModel.SecondName,
                Email = emailRequest.Email,
                LevelOfEducation = studentModel.LevelOfEducation,
                ProjectRequestId = studentModel.ProjectRequestId,
                SpecializationId = studentModel.SpecializationId
            };

                return Ok(studentDto);
        }

        [HttpGet("{studentId}/GetDepartmentId")]
        public async Task<ActionResult<int>> GetStudentDepartmentId(int studentId)
        {
            StudentModel studentModel = await _context.Students.FindAsync(studentId);

            SpecializationModel specializationModel = await _context.Specializations.Where(s => s.Id == studentModel.SpecializationId).FirstOrDefaultAsync();

            if (specializationModel == null)
                return NotFound();


            return Ok(specializationModel.DeparmentId);
        }
    }

    public class EmailRequest
    {
        public string Email { get; set; }
    }
}
