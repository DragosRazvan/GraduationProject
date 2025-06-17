using GraduationProject.DTOs;
using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<ProjectRequestDto>> GetProjectByIdAsync(int projectId)
        {
            ProjectRequestModel projectModel = await _context.ProjectRequests.FindAsync(projectId);

            if (projectModel == null)
                return NotFound();

            ProfessorModel professor = await _context.Professors.FindAsync(projectModel.ProfessorId);

            if (professor == null)
                return NotFound();

            string projectRequestStatus = "";
            if (projectModel.IsAcceptedByProfessor)
                projectRequestStatus = "Aprobat";
            else
                projectRequestStatus = "În așteptare";

            ProjectDetailsDto projectDetailsDto = new ProjectDetailsDto
            {
                ProjectRequestId = projectModel.Id,
                Title = projectModel.Title,
                Description = projectModel.Description,
                LevelOfEducation = projectModel.LevelOfEducation,
                IsAcceptedByProfessor = projectModel.IsAcceptedByProfessor,
                StatusProjectRequest = projectRequestStatus,
                ProfessorName = professor.SecondName + " " + professor.FirstName,
                StudentId = projectModel.StudentId,
                ProfessorId = projectModel.ProfessorId
            };

            return Ok(projectDetailsDto);
        }

        [HttpGet("GetProjectsByProfessorId/{professorId}")]
        public async Task<ActionResult<ICollection<ProjectRequestDto>>> GetProjectsByProfessorId(int professorId){
            List<ProjectRequestModel> projectRequests = await _context.ProjectRequests.Where(p => (p.StudentId == null && p.ProfessorId == professorId)).ToListAsync<ProjectRequestModel>();

            List<ProjectRequestDto> projectsByProfessorId = new List<ProjectRequestDto>();

            foreach(ProjectRequestModel project in projectRequests)
            {
                ProjectRequestDto newProject = new ProjectRequestDto
                {
                    Id = project.Id,
                    Title = project.Title,
                    Description = project.Description,
                    LevelOfEducation = project.LevelOfEducation,
                    IsAcceptedByProfessor = project.IsAcceptedByProfessor,
                    ProfessorId = project.ProfessorId,
                    StudentId = project.StudentId
                };

                projectsByProfessorId.Add(newProject);
            }
            return Ok(projectsByProfessorId);
        }

    }
}
