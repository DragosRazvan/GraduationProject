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
        public async Task<ActionResult> PostProjectRequestAsync(ProjectRequestDto projectRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var projectRequestModel = new ProjectRequestModel
                {
                    Title = projectRequestDto.Title,
                    Description = projectRequestDto.Description,
                    IsAcceptedByProfessor = false,
                    LevelOfEducation = projectRequestDto.LevelOfEducation,
                    StudentId = projectRequestDto.StudentId,
                    ProfessorId = projectRequestDto.ProfessorId
                };

                _context.ProjectRequests.Add(projectRequestModel);
                await _context.SaveChangesAsync();

                return Created();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
