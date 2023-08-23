using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Context
{
    // Necessário herdar a classe DbContext do EF Core!!
    public class AppDbContext : DbContext
    {
        // : base () chama o construtor da classe Pai ao instanciar um obj Filho, semelhante ao parent::__construct()
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Produto>? Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
           /* Configuração do SQL das tabelas */

            // Tabela Categoria

            mb.Entity<Categoria>().HasKey(c => c.Id);
            mb.Entity<Categoria>().Property(c => c.Nome)
                                            .HasMaxLength(100)
                                            .IsRequired();
            mb.Entity<Categoria>().Property(c => c.Descricao)
                                            .HasMaxLength(150)
                                            .IsRequired();

            // Tabela Produto

            mb.Entity<Produto>().HasKey(c => c.Id);
            mb.Entity<Produto>().Property(c => c.Nome)
                                            .HasMaxLength(100)
                                            .IsRequired();
            mb.Entity<Produto>().Property(c => c.Descricao).HasMaxLength(150);
            mb.Entity<Produto>().Property(c => c.Imagem).HasMaxLength(100);
                                         
            mb.Entity<Produto>().Property(c => c.Preco).HasPrecision(14, 2);

            // Relacionamentos

            mb.Entity<Produto>()
                .HasOne<Categoria>(c => c.Categoria)
                    .WithMany(p => p.Lista_Produtos)
                        .HasForeignKey(c => c.Id_Categoria);

        }
    }
}
