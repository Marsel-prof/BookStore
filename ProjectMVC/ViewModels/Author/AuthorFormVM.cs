using System.ComponentModel.DataAnnotations;

namespace ProjectMVC.ViewModels.Author
{
    public class AuthorFormVM
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }
    }
}
