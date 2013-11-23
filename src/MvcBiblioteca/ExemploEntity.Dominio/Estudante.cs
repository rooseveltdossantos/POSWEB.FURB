using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominioEscolar
{
    [Table("EstudantesInfo")]
    public class Estudante
    {
        public Estudante() { }

        [Key]
        public int SID { get; set; }

        [Column("Nome", TypeName = "ntext")]
        [MaxLength(20)]
        public string Nome { get; set; }

        [NotMapped]
        public int? Age { get; set; }

        public int GrauId { get; set; }

        [ForeignKey("GrauId")]
        public virtual GrauDeConhecimento GrauDeConhecimento { get; set; }

    }
}
