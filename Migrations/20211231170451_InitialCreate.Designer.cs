﻿// <auto-generated />
using System;
using BankTimeNET;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankTimeNET.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20211231170451_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BankTimeNET.Models.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Place")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("BankTimeNET.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int?>("BankId")
                        .HasColumnType("int");

                    b.Property<string>("Dni")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BankTimeNET.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DoneTime")
                        .HasColumnType("int");

                    b.Property<int>("DoneUserId")
                        .HasColumnType("int");

                    b.Property<int>("RequestTime")
                        .HasColumnType("int");

                    b.Property<int>("RequestUserId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.HasIndex("DoneUserId");

                    b.HasIndex("RequestUserId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("BankTimeNET.Models.User", b =>
                {
                    b.HasOne("BankTimeNET.Models.Bank", "Bank")
                        .WithMany("Users")
                        .HasForeignKey("BankId");

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("BankTimeNET.Service", b =>
                {
                    b.HasOne("BankTimeNET.Models.Bank", "Bank")
                        .WithMany("Services")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BankTimeNET.Models.User", "DoneUser")
                        .WithMany()
                        .HasForeignKey("DoneUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BankTimeNET.Models.User", "RequestUser")
                        .WithMany()
                        .HasForeignKey("RequestUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("DoneUser");

                    b.Navigation("RequestUser");
                });

            modelBuilder.Entity("BankTimeNET.Models.Bank", b =>
                {
                    b.Navigation("Services");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
