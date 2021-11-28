#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphQLWorkshop.Data.Entities
{
    public class Question : BaseEntity
    {
        [Display(Name = "Soru Metni")]
        [Required(ErrorMessage = "Soru Metni gereklidir.")]
        [DataType(DataType.Html)]
        public string QuestionText { get; set; }
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }
        [Display(Name = "Kategori")]
        public QuestionCategory? Category { get; set; }
        [Display(Name = "Onay Durumu")]
        public QuestionApprovalStatus ApprovalStatus { get; set; }
        [Display(Name = "Cevaplar")]
        public IList<QuestionAnswer>? Answers { get; set; }
    }

    public enum QuestionApprovalStatus
    {
        [Display(Name = "Onay Bekliyor")]
        Waiting,
        [Display(Name = "Onaylandı")]
        Approved,
        [Display(Name = "Reddedildi")]
        Denied
    }
}
