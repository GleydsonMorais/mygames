using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyGames.Data.Models;
using MyGames.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGames.Data.Contexts
{
    public class MyGamesDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<TipoPessoa> TipoPessoas { get; set; }
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<TipoJogo> TipoJogos { get; set; }
        public DbSet<HistoricoEmprestimo> JogosEmprestados { get; set; }

        public MyGamesDbContext(DbContextOptions<MyGamesDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=GGUEDESM10\\SQLEXPRESS;Database=mygames;Trusted_Connection=True;MultipleActiveResultSets=true",
                    x => x.MigrationsHistoryTable("__efmigrationshistory"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region ~~ Identity Config ~~
            modelBuilder.Entity<ApplicationUser>(i =>
            {
                i.ToTable("Users");
                i.HasIndex(x => x.Email).IsUnique();
                i.HasIndex(x => x.UserName).IsUnique();

                i.HasMany(x => x.Roles)
                .WithOne()
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApplicationRole>(i =>
            {
                i.ToTable("Roles");
            });

            modelBuilder.Entity<ApplicationUserRole>(i =>
            {
                i.ToTable("UserRoles");

                i.HasOne(x => x.User)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.UserId);

                i.HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId);
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(i => 
            {
                i.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(i => 
            {
                i.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(i => 
            {
                i.ToTable("UserTokens");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(i => 
            {
                i.ToTable("RoleClaims");
            });
            #endregion

            modelBuilder.Entity<Pessoa>(i =>
            {
                i.ToTable("Pessoa");
                i.HasKey(x => x.Id);

                i.HasOne(x => x.Login)
                .WithOne(x => x.Pessoa)
                .HasForeignKey<Pessoa>(x => x.LoginId);

                i.HasOne(x => x.TipoPessoa)
                .WithMany(x => x.Pessoas)
                .HasForeignKey(x => x.TipoPessoaId);

                i.HasMany(x => x.JogosEmprestados)
                .WithOne(x => x.Pessoa)
                .HasForeignKey(x => x.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TipoPessoa>(i =>
            {
                i.ToTable("TipoPessoa");
                i.HasKey(x => x.Id);

                i.HasMany(x => x.Pessoas)
                .WithOne(x => x.TipoPessoa)
                .HasForeignKey(x => x.TipoPessoaId);
            });

            modelBuilder.Entity<Jogo>(i =>
            {
                i.ToTable("Jogo");
                i.HasKey(x => x.Id);

                i.HasOne(x => x.TipoJogo)
                .WithMany(x => x.Jogos)
                .HasForeignKey(x => x.TipoJogoId);

                i.HasMany(x => x.HistoricoEmprestimo)
               .WithOne(x => x.Jogo)
               .HasForeignKey(x => x.JogoId);
            });

            modelBuilder.Entity<TipoJogo>(i =>
            {
                i.ToTable("TipoJogo");
                i.HasKey(x => x.Id);

                i.HasMany(x => x.Jogos)
                .WithOne(x => x.TipoJogo)
                .HasForeignKey(x => x.TipoJogoId);
            });

            modelBuilder.Entity<HistoricoEmprestimo>(i =>
            {
                i.ToTable("HistoricoEmprestimo");
                i.HasKey(x => x.Id);

                i.HasOne(x => x.Pessoa)
                .WithMany(x => x.JogosEmprestados)
                .HasForeignKey(x => x.PessoaId);

                i.HasOne(x => x.Jogo)
                .WithMany(x => x.HistoricoEmprestimo)
                .HasForeignKey(x => x.JogoId);
            });
        }
    }
}
