using Microsoft.EntityFrameworkCore;
using TalentConnect.Models;

namespace TalentConnect.Data
{
    public class TalentConnectBD : DbContext
    {
        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Experiencia> Experiencias { get; set; }
        public DbSet<Formacao> Formacoes { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Vaga> Vagas { get; set; }
        public TalentConnectBD(DbContextOptions<TalentConnectBD> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração do relacionamento entre Candidato e Experiencia
            modelBuilder.Entity<Candidato>()
                .HasMany(c => c.Experiencias)
                .WithOne(e => e.Candidato)
                .HasForeignKey(e => e.CandidatoID);

            // Configuração do relacionamento entre Candidato e Formacao
            modelBuilder.Entity<Candidato>()
                .HasMany(c => c.Formacoes)
                .WithOne(f => f.Candidato)
                .HasForeignKey(f => f.CandidatoID);

            // Configuração do relacionamento entre Candidato e Portfolio
            modelBuilder.Entity<Candidato>()
                .HasMany(c => c.Portfolios)
                .WithOne(p => p.Candidato)
                .HasForeignKey(p => p.CandidatoID);   
            
        }
    }
}
