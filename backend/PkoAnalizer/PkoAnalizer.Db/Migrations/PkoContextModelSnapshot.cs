﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PkoAnalizer.Db;

namespace PkoAnalizer.Db.Migrations
{
    [DbContext(typeof(PkoContext))]
    partial class PkoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PkoAnalizer.Db.Models.BankTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("BankTransactionTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Extensions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OperationDate")
                        .HasColumnType("Date");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("Date");

                    b.HasKey("Id");

                    b.HasIndex("BankTransactionTypeId");

                    b.ToTable("BankTransactions");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.BankTransactionGroup", b =>
                {
                    b.Property<Guid>("BankTransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BankTransactionId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("BankTransactionGroups");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.BankTransactionType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("BankTransactionTypes");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.Rule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BankTransactionTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RuleDefinition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BankTransactionTypeId");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.BankTransaction", b =>
                {
                    b.HasOne("PkoAnalizer.Db.Models.BankTransactionType", "BankTransactionType")
                        .WithMany("BankTransactions")
                        .HasForeignKey("BankTransactionTypeId");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.BankTransactionGroup", b =>
                {
                    b.HasOne("PkoAnalizer.Db.Models.BankTransaction", "BankTransaction")
                        .WithMany("BankTransactionGroups")
                        .HasForeignKey("BankTransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PkoAnalizer.Db.Models.Group", "Group")
                        .WithMany("BankTransactionGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.Rule", b =>
                {
                    b.HasOne("PkoAnalizer.Db.Models.BankTransactionType", "BankTransactionType")
                        .WithMany()
                        .HasForeignKey("BankTransactionTypeId");
                });
#pragma warning restore 612, 618
        }
    }
}
