using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Biblioteca.Dominio
{
    public class Livro
    {
        public int LivroId { get; set; }

        [DisplayName("Título")]
        public string Titulo { get; set; }

        public string Autor {get; set; }        

        public int Ano { get; set; }

        [DisplayName("Preço")]
        public decimal Preco { get; set; }

        public ICollection<ComentarioLivro> Comentarios { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Livro);
        }

        public bool Equals(Livro outro)
            {
                // If parameter is null, return false. 
                if (Object.ReferenceEquals(outro, null))
                {
                    return false;
                }

                // Optimization for a common success case. 
                if (Object.ReferenceEquals(this, outro))
                {
                    return true;
                }

                // If run-time types are not exactly the same, return false. 
                if (this.GetType() != outro.GetType())
                    return false;

                // Return true if the fields match. 
                // Note that the base class is not invoked because it is 
                // System.Object, which defines Equals as reference equality. 
                return (outro.LivroId == this.LivroId);
            }

            public override int GetHashCode()
            {
                return this.LivroId * 347;;
            }

            public static bool operator ==(Livro lhs, Livro rhs)
            {
                // Check for null on left side. 
                if (Object.ReferenceEquals(lhs, null))
                {
                    if (Object.ReferenceEquals(rhs, null))
                    {
                        // null == null = true. 
                        return true;
                    }

                    // Only the left side is null. 
                    return false;
                }
                // Equals handles case of null on right side. 
                return lhs.Equals(rhs);
            }

            public static bool operator !=(Livro lhs, Livro rhs)
            {
                return !(lhs == rhs);
            }
        }

 
}