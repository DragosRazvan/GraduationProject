namespace GraduationProject.DTOs
{
    public class ProjectDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAcceptedByProfessor { get; set; }
        public string StatusProjectRequest { get; set; } // "in asteptare" sau "aprobata"
        public string LevelOfEducation { get; set; }
        public string ProfessorName { get; set; }
        public int? StudentId { get; set; }
        public int ProfessorId { get; set; }
    }
}
