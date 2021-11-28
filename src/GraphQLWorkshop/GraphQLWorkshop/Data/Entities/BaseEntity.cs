using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLWorkshop.Data.Entities
{
    public interface ITrackable
    {
        [Display(Name = "Oluşturulma Zamanı")]
        DateTime CreatedAt { get; set; }
        [Display(Name = "Oluşturan Kullanıcı")]
        int? CreatedBy { get; set; }
        [Display(Name = "Son Düzenlenme Zamanı")]
        DateTime LastUpdatedAt { get; set; }
        [Display(Name = "Son Düzenleyen Kullanıcı")]
        int? LastUpdatedBy { get; set; }
    }

    public class BaseEntity : ITrackable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual int? CreatedBy { get; set; }
        public virtual DateTime LastUpdatedAt { get; set; }
        public virtual int? LastUpdatedBy { get; set; }


    }
}
