namespace ProjectMVC.Models
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ProjectMVC.Controllers;
    using System.ComponentModel.DataAnnotations;

    // عشان امنع تكرار اسم الكاتيجوري واخليه يونيك من خلال الداتا بيس 

    [Index("Name",IsUnique = true)]
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(30)]
        [Remote("checkName", controller: "Categories", ErrorMessage = "exisssssst")]
        public string Name { get; set; } = null!;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
