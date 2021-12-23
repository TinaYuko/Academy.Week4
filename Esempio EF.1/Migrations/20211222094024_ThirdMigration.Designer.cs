﻿// <auto-generated />
using System;
using Esempio_EF._1.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Esempio_EF._1.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20211222094024_ThirdMigration")]
    partial class ThirdMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Esempio_EF._1.Entities.Azienda", b =>
                {
                    b.Property<int>("AziendaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AziendaId"), 1L, 1);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AziendaId");

                    b.ToTable("Azienda");
                });

            modelBuilder.Entity("Esempio_EF._1.Entities.Impiegato", b =>
                {
                    b.Property<int>("ImpiegatoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImpiegatoId"), 1L, 1);

                    b.Property<int>("AziendaId")
                        .HasColumnType("int");

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataNascita")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImpiegatoId");

                    b.HasIndex("AziendaId");

                    b.ToTable("Impiegato");
                });

            modelBuilder.Entity("Esempio_EF._1.Entities.Impiegato", b =>
                {
                    b.HasOne("Esempio_EF._1.Entities.Azienda", "Azienda")
                        .WithMany("Impiegati")
                        .HasForeignKey("AziendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Azienda");
                });

            modelBuilder.Entity("Esempio_EF._1.Entities.Azienda", b =>
                {
                    b.Navigation("Impiegati");
                });
#pragma warning restore 612, 618
        }
    }
}
