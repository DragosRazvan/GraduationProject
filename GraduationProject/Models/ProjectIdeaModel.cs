namespace GraduationProject.Models
{
    public class ProjectIdeaModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LevelOfEducation { get; set; } //bachelor or master degree
        public int ProfessorId { get; set; }
        public ProfessorModel Professor { get; set; }
    }
}
