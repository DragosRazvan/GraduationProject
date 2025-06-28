using GraduationProject.DTOs;
using GraduationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GraduationProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[HttpGet("GetAllProjectRequests")]
        //public async Task<ActionResult<List<ProjectRequestModel>>> GetAllProjectRequestsAsync(int professorId)
        //{
        //    return await _context.ProjectRequests.Where(r => r.ProfessorId == professorId).ToListAsync();
        //}

        [HttpGet("GetAllProjectRequests/{professorId}")]
        public async Task<ActionResult<List<ProjectsCoordinatedByProfessorDto>>> GetAllProjectRequestsAsync(int professorId)
        {
            List<ProjectRequestModel> projectRequests = await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId != null).ToListAsync();

            if (projectRequests == null)
                return NotFound("No project reuqests");

            List<ProjectsCoordinatedByProfessorDto> projectsCoordinatedByProfessorDtos = new List<ProjectsCoordinatedByProfessorDto>();

            foreach(ProjectRequestModel project in projectRequests)
            {
                StudentModel studentMdodel = await _context.Students.FindAsync(project.StudentId);

                if (studentMdodel == null)
                    return NotFound("No student found");

                StudentDto student = new StudentDto
                {
                    Id = studentMdodel.Id,
                    FirstName = studentMdodel.FirstName,
                    SecondName = studentMdodel.SecondName,
                    Email = studentMdodel.Email,
                    LevelOfEducation = studentMdodel.LevelOfEducation,
                    ProjectRequestId = studentMdodel.ProjectRequestId,
                    SpecializationId = studentMdodel.SpecializationId
                };

                ProjectsCoordinatedByProfessorDto p1 = new ProjectsCoordinatedByProfessorDto
                {
                    Id = project.Id,
                    Title = project.Title,
                    Description = project.Description,
                    LevelOfEducation = project.LevelOfEducation,
                    IsAcceptedByProfessor = project.IsAcceptedByProfessor,
                    Student = student,
                    ProfessorId = project.ProfessorId
                };

                if (p1.IsAcceptedByProfessor)
                    p1.Status = "acceptată";
                else
                    p1.Status = "în așteptare";

                projectsCoordinatedByProfessorDtos.Add(p1);
            }

            return projectsCoordinatedByProfessorDtos;
        }

        [HttpGet("GetOwnProjects/{professorId}")]
        public async Task<ActionResult<ICollection<ProfessorOwnProjectDto>>> GetOwnProjectsAsync(int professorId)
        {
            List<ProjectRequestModel> projectsModel = await _context.ProjectRequests.Where(p => p.ProfessorId == professorId).ToListAsync<ProjectRequestModel>();

            if (projectsModel == null)
                return NotFound();

            List<ProfessorOwnProjectDto> professorProjects = new List<ProfessorOwnProjectDto>();

            foreach(ProjectRequestModel project in projectsModel)
            {
                ProjectIdeaModel projectIdea = await _context.ProfessorsProjectIdeas.Where(p => p.Title == project.Title).FirstOrDefaultAsync();

                if(projectIdea != null)
                {
                    StudentDto student = new StudentDto();
                    StudentModel studentModel;
                    if (project.StudentId != null) {
                        studentModel = await _context.Students.FindAsync(project.StudentId);
                        
                        student = new StudentDto
                        {
                            Id = studentModel.Id,
                            FirstName = studentModel.FirstName,
                            SecondName = studentModel.SecondName,
                            Email = studentModel.Email,
                            LevelOfEducation = studentModel.LevelOfEducation,
                            SpecializationId = studentModel.SpecializationId,
                            ProjectRequestId = studentModel.ProjectRequestId
                        };
                    }
                    else
                    {
                        studentModel = null;
                    }

                    ProfessorOwnProjectDto p = new ProfessorOwnProjectDto
                    {
                        Id = project.Id,
                        Title = project.Title,
                        Description = project.Description,
                        LevelOfEducation = project.LevelOfEducation,
                        ProfessorId = project.ProfessorId,
                        Student = student
                    };

                    professorProjects.Add(p);
                }

            }

            return Ok(professorProjects);
        }

        //[HttpGet("GetSpecificProjectRequest/{studentId}")]
        //public async Task<ActionResult<ProjectRequestModel>> GetSpecificProjectRequestAsync(int professorId, int studentId)
        //{
        //    var projectRequest = await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId == studentId).FirstOrDefaultAsync() ?? new ProjectRequestModel();

        //    return projectRequest;
        //}

        [HttpGet("GetSpecificProjectRequest/{studentId}")]
        public async Task<ActionResult<ProjectRequestDto>> GetSpecificProjectRequestAsync(int professorId, int studentId)
        {
            ProjectRequestModel projectRequest = await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId == studentId).FirstOrDefaultAsync();

            ProjectRequestDto projectRequestDto = new ProjectRequestDto
            {
                Title = projectRequest.Title,
                Description = projectRequest.Description,
                LevelOfEducation = projectRequest.LevelOfEducation,
                IsAcceptedByProfessor = projectRequest.IsAcceptedByProfessor,
                StudentId = projectRequest.StudentId,
                ProfessorId = projectRequest.ProfessorId
            };

            return Ok(projectRequestDto);
        }

        //[HttpPut]
        //public async Task<IActionResult> AcceptProjectRequestAsync(int professorId, int studentId)
        //{
        //    await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId == studentId).ExecuteUpdateAsync(r => r.SetProperty(
        //        request => request.IsAcceptedByProfessor, true));

        //    return Ok();
        //}

        //[HttpPut]
        //public async Task<IActionResult> RejectProjectRequestAsync(int professorId, int studentId)
        //{
        //    await _context.ProjectRequests.Where(r => r.ProfessorId == professorId && r.StudentId == studentId).ExecuteUpdateAsync(r => r.SetProperty(
        //        request => request.IsAcceptedByProfessor, false));

        //    NotificationModel notificationModel = new NotificationModel
        //    {
        //        NotificationMessage = $"The project request with the "
        //    };

        //    await _context.ProjectRequests.ExecuteDeleteAsync();

        //    return Ok();
        //}

        [HttpPut("UpdateProjectRequest")]
        public async Task<ActionResult> UpdateProjectRequestStateAsync([FromBody] UpdateProjectRequestProfessorDto updateProjectRequestProfessorDto)
        {
            try
            {
                var projectRequest = await _context.ProjectRequests.FindAsync(updateProjectRequestProfessorDto.ProjectRequestId);

                if (projectRequest == null)
                {
                    return NotFound("Project request not found.");
                }

                if (!updateProjectRequestProfessorDto.Accepted)
                {
                    _context.ProjectRequests.Remove(projectRequest);
                    await _context.SaveChangesAsync();

                    return Ok("Project request deleted successfully!");
                }

                projectRequest.IsAcceptedByProfessor = updateProjectRequestProfessorDto.Accepted;
                _context.Entry(projectRequest).Property(p => p.IsAcceptedByProfessor).IsModified = true;

                var professor = await _context.Professors.FindAsync(projectRequest.ProfessorId);
                professor.NumberOfCoordinatedProjects++;
                _context.Entry(professor).Property(p => p.NumberOfCoordinatedProjects).IsModified = true;

                await _context.SaveChangesAsync();

                return Ok("Project request accepted successfully!");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPost("PostNewProjectIdea")]
        public async Task<ActionResult> PostNewProjectIdeaAsync([FromBody] NewProjectIdeaDto newProjectIdeaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(newProjectIdeaDto);
                }

                ProjectIdeaModel newProjectIdeaModel = new ProjectIdeaModel
                {
                    Title = newProjectIdeaDto.Title,
                    Description = newProjectIdeaDto.Description,
                    LevelOfEducation = newProjectIdeaDto.LevelOfEducation,
                    ProfessorId = newProjectIdeaDto.ProfessorId
                };
                _context.ProfessorsProjectIdeas.Add(newProjectIdeaModel);


                ProjectRequestModel newProjectRequestModel = new ProjectRequestModel
                {
                    Title = newProjectIdeaDto.Title,
                    Description = newProjectIdeaDto.Description,
                    LevelOfEducation = newProjectIdeaDto.LevelOfEducation,
                    ProfessorId = newProjectIdeaDto.ProfessorId,
                    StudentId = null,
                    IsAcceptedByProfessor = false
                };
                _context.ProjectRequests.Add(newProjectRequestModel);

                await _context.SaveChangesAsync();

                return Created();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPut("UpdateProjectIdea/{oldProjectTitle}")]
        public async Task<ActionResult> UpdateProjectIdeaAsync([FromBody] NewProjectIdeaDto newProjectIdeaDto, string oldProjectTitle)
        {
            ProjectRequestModel project = await _context.ProjectRequests.Where(p => p.Title == oldProjectTitle).FirstOrDefaultAsync<ProjectRequestModel>();

            if (project == null)
                return NotFound("No project request found");

            project.Title = newProjectIdeaDto.Title;
            _context.Entry(project).Property(p => p.Title).IsModified = true;

            project.Description = newProjectIdeaDto.Description;
            _context.Entry(project).Property(p => p.Description).IsModified = true;



            ProjectIdeaModel projectIdea = await _context.ProfessorsProjectIdeas.Where(p => p.Title == oldProjectTitle).FirstOrDefaultAsync<ProjectIdeaModel>();

            if (projectIdea == null)
                return NotFound("No project idea found");

            projectIdea.Title = newProjectIdeaDto.Title;
            _context.Entry(projectIdea).Property(p => p.Title).IsModified = true;

            projectIdea.Description = newProjectIdeaDto.Description;
            _context.Entry(projectIdea).Property(p => p.Description).IsModified = true;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("GetProfessorsByDepartment/{departmentId}")]
        public async Task<ActionResult<ICollection<ProfessorDto>>> GetProfessorsByDepartment(int departmentId)
        {
            List<ProfessorModel> professors = await _context.Professors.Where(p => p.DepartmentId == departmentId).ToListAsync();

            List<ProfessorDto> professorsDto = new List<ProfessorDto>();

            foreach(ProfessorModel professor in professors)
            {
                ProfessorDto prof = new ProfessorDto
                {
                    Id = professor.Id,
                    FirstName = professor.FirstName,
                    SecondName = professor.SecondName,
                    Email = professor.Email,
                    NumberOfCoordinatedProjects = professor.NumberOfCoordinatedProjects,
                    DepartmentId = professor.DepartmentId
                };

                professorsDto.Add(prof);
            }

            if (professorsDto == null)
                return NotFound();

            return Ok(professorsDto);
        }

        [HttpGet("GetProfessorByEmail/{professorEmail}")]
        public async Task<ActionResult<ProfessorDto>> GetProfessorByEmailAsync(string professorEmail)
        {
            try
            {
                ProfessorModel professorModel = await _context.Professors.Where(p => p.Email == professorEmail).FirstOrDefaultAsync();

                if (professorModel == null)
                    return NotFound();

                ProfessorDto professor = new ProfessorDto
                {
                    Id = professorModel.Id,
                    FirstName = professorModel.FirstName,
                    SecondName = professorModel.SecondName,
                    Email = professorModel.Email,
                    NumberOfCoordinatedProjects = professorModel.NumberOfCoordinatedProjects,
                    DepartmentId = professorModel.DepartmentId
                };

                return Ok(professor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
