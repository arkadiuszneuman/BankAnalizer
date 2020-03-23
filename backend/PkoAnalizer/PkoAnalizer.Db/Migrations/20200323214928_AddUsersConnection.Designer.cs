﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PkoAnalizer.Db;

namespace PkoAnalizer.Db.Migrations
{
    [DbContext(typeof(PkoContext))]
    [Migration("20200323214928_AddUsersConnection")]
    partial class AddUsersConnection
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BankTransactionTypeId");

                    b.HasIndex("UserId");

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
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("UserId");

                    b.ToTable("BankTransactionTypes");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RuleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RuleId");

                    b.HasIndex("UserId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.Rule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BankTransactionTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RuleDefinition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RuleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BankTransactionTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasAlternateKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.UsersConnection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsRequestApproved")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserRequestedToConnectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserRequestingConnectionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasAlternateKey("UserRequestedToConnectId", "UserRequestingConnectionId");

                    b.HasIndex("UserRequestedToConnectId");

                    b.HasIndex("UserRequestingConnectionId");

                    b.ToTable("UsersConnections");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.BankTransaction", b =>
                {
                    b.HasOne("PkoAnalizer.Db.Models.BankTransactionType", "BankTransactionType")
                        .WithMany("BankTransactions")
                        .HasForeignKey("BankTransactionTypeId");

                    b.HasOne("PkoAnalizer.Db.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
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

            modelBuilder.Entity("PkoAnalizer.Db.Models.BankTransactionType", b =>
                {
                    b.HasOne("PkoAnalizer.Db.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.Group", b =>
                {
                    b.HasOne("PkoAnalizer.Db.Models.Rule", "Rule")
                        .WithMany()
                        .HasForeignKey("RuleId");

                    b.HasOne("PkoAnalizer.Db.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.Rule", b =>
                {
                    b.HasOne("PkoAnalizer.Db.Models.BankTransactionType", "BankTransactionType")
                        .WithMany()
                        .HasForeignKey("BankTransactionTypeId");

                    b.HasOne("PkoAnalizer.Db.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PkoAnalizer.Db.Models.UsersConnection", b =>
                {
                    b.HasOne("PkoAnalizer.Db.Models.User", "UserRequestedToConnect")
                        .WithMany()
                        .HasForeignKey("UserRequestedToConnectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PkoAnalizer.Db.Models.User", "UserRequestingConnection")
                        .WithMany()
                        .HasForeignKey("UserRequestingConnectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
