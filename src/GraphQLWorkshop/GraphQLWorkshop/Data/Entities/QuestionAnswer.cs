#nullable enable
using System.ComponentModel.DataAnnotations;

namespace GraphQLWorkshop.Data.Entities
{
    public class QuestionAnswer : BaseEntity
    {
        [Display(Name = "Cevap")]
        [Required(ErrorMessage = "Cevap gereklidir.")]
        public string Answer { get; set; }
        [Display(Name = "Doğru Mu?")]
        public bool IsCorrect { get; set; }
        [Display(Name = "Soru")]
        public int QuestionId { get; set; }
        [Display(Name = "Soru")]
        public Question? Question { get; set; }

    }
}
