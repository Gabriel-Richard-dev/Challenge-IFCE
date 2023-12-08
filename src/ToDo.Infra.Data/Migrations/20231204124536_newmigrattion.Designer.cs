﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDo.Infra.Data.Context;

#nullable disable

namespace ToDo.Infra.Data.Migrations
{
    [DbContext(typeof(ToDoContext))]
    [Migration("20231204124536_newmigrattion")]
    partial class newmigrattion
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ToDo.Domain.Entities.Assignment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIGINT")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AtListId")
                        .HasColumnType("BIGINT");

                    b.Property<sbyte>("Concluded")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TINYINT")
                        .HasDefaultValue((sbyte)0);

                    b.Property<DateTime?>("DateConcluded")
                        .HasColumnType("DATE");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("DATE");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("VARCHAR(300)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<long>("UserId")
                        .HasColumnType("BIGINT");

                    b.HasKey("Id");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("ToDo.Domain.Entities.AssignmentList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIGINT")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ListId")
                        .HasColumnType("BIGINT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<long>("UserId")
                        .HasColumnType("BIGINT");

                    b.HasKey("Id");

                    b.ToTable("AssignmentList", (string)null);
                });

            modelBuilder.Entity("ToDo.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("BIGINT")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<sbyte>("AdminPrivileges")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TINYINT")
                        .HasDefaultValue((sbyte)0);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(180)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(70)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(40)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
