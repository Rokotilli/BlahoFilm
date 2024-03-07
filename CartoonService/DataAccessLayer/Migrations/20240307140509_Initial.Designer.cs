﻿// <auto-generated />
using System;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(CartoonServiceDbContext))]
    [Migration("20240307140509_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccessLayer.Entities.AnimationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AnimationTypes");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Cartoon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnimationTypeId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("CountParts")
                        .HasColumnType("int");

                    b.Property<int?>("CountSeasons")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeOnly?>("Duration")
                        .HasColumnType("time");

                    b.Property<byte[]>("Poster")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("StudioName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrailerUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnimationTypeId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Cartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.CartoonPart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CartoonId")
                        .HasColumnType("int");

                    b.Property<TimeOnly>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PartNumber")
                        .HasColumnType("int");

                    b.Property<int>("SeasonNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CartoonId");

                    b.ToTable("CartoonParts");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CartoonPartId")
                        .HasColumnType("int");

                    b.Property<int>("CountDislikes")
                        .HasColumnType("int");

                    b.Property<int>("CountLikes")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ParentCommentId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CartoonPartId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.GenresCartoon", b =>
                {
                    b.Property<int>("CartoonId")
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.HasKey("CartoonId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("GenresCartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.TagsCartoon", b =>
                {
                    b.Property<int>("CartoonId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("CartoonId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("TagsCartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Cartoon", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.AnimationType", "AnimationType")
                        .WithMany("Cartoons")
                        .HasForeignKey("AnimationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.Category", "Category")
                        .WithMany("Cartoons")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnimationType");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.CartoonPart", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Cartoon", "Cartoon")
                        .WithMany("Cartoons")
                        .HasForeignKey("CartoonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cartoon");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Comment", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.CartoonPart", "CartoonPart")
                        .WithMany()
                        .HasForeignKey("CartoonPartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.Comment", "ParentComment")
                        .WithMany()
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("DataAccessLayer.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CartoonPart");

                    b.Navigation("ParentComment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.GenresCartoon", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Cartoon", "Cartoon")
                        .WithMany("GenresCartoons")
                        .HasForeignKey("CartoonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.Genre", "Genre")
                        .WithMany("GenresCartoons")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cartoon");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.TagsCartoon", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Cartoon", "Cartoon")
                        .WithMany("TagsCartoons")
                        .HasForeignKey("CartoonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.Tag", "Tag")
                        .WithMany("TagsCartoons")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cartoon");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.AnimationType", b =>
                {
                    b.Navigation("Cartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Cartoon", b =>
                {
                    b.Navigation("Cartoons");

                    b.Navigation("GenresCartoons");

                    b.Navigation("TagsCartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Category", b =>
                {
                    b.Navigation("Cartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Genre", b =>
                {
                    b.Navigation("GenresCartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Tag", b =>
                {
                    b.Navigation("TagsCartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.User", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
