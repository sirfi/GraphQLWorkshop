#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphQLWorkshop.Data.Entities
{
    public class QuestionCategory : BaseEntity
    {
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Ad gereklidir.")]
        public string Name { get; set; }

        public IList<Question?>? Questions { get; set; }
    }
}
