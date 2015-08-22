using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;

namespace prj_chamadosBRA.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int PerfilUsuario { get; set; }
        public string Nome { get; set; }
        public string Contato { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<PerfilUsuario> PerfilUsuario { get; set; }
        public DbSet<Setor> Setor { get; set; }
        public DbSet<Chamado> Chamado { get; set; }
        public DbSet<Obra> Obra { get; set; }
        public DbSet<UsuarioObra> UsuarioObra { get; set; }
        public DbSet<CentroAdministrativo> CentroAdministrativo { get; set; }
        public DbSet<ChamadoAnexo> ChamadoAnexo { get; set; }
        public DbSet<UsuarioSetor> UsuarioSetor { get; set; }
        public DbSet<ChamadoHistorico> ChamadoHistorico { get; set; }
        public DbSet<ChamadoClassificacao> ChamadoClassificacao { get; set; }
        public DbSet<ChamadoSubClassificacao> ChamadoSubClassificacao { get; set; }

        public ApplicationDbContext()
            : base("ChamadosBRAConnectionString", throwIfV1Schema: false)
        {


        }

        //public System.Data.Entity.DbSet<prj_chamadosBRA.Models.ApplicationUser> ApplicationUsers { get; set; }

        //protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        //{
        //    dbModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //    dbModelBuilder.Entity<ApplicationUser>().HasMany<Setor>(user => user.setores)
        //                                   .WithMany(setor => setor.usuarios).Map(c =>
        //                                             {
        //                                                 c.MapLeftKey("Usuario_Id");
        //                                                    c.MapRightKey("idSetor");
        //                                                    c.ToTable("UsuarioSetor");
        //                                                });
        //    base.OnModelCreating(dbModelBuilder);
        //    dbModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}
       
    }
}