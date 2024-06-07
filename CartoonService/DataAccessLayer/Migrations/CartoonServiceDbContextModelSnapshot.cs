﻿// <auto-generated />
using System;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(CartoonServiceDbContext))]
    partial class CartoonServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
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

                    b.Property<int>("AgeRestriction")
                        .HasColumnType("int");

                    b.Property<int>("AnimationTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("CountParts")
                        .HasColumnType("int");

                    b.Property<int?>("CountSeasons")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfPublish")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Duration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Poster")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PosterPartOne")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PosterPartThree")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PosterPartTwo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Quality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrailerUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AnimationTypeId");

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

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PartNumber")
                        .HasColumnType("int");

                    b.Property<int?>("SeasonNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CartoonId");

                    b.ToTable("CartoonParts");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.CategoriesCartoon", b =>
                {
                    b.Property<int>("CartoonId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("CartoonId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoriesCartoons");
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

                    b.Property<int?>("CartoonId")
                        .HasColumnType("int");

                    b.Property<int?>("CartoonPartId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ParentCommentId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CartoonId");

                    b.HasIndex("CartoonPartId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.CommentDislike", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CommentId");

                    b.HasIndex("CommentId");

                    b.ToTable("CommentDislikes");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.CommentLike", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CommentId");

                    b.HasIndex("CommentId");

                    b.ToTable("CommentLikes");
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

            modelBuilder.Entity("DataAccessLayer.Entities.Rating", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CartoonId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.HasKey("UserId", "CartoonId");

                    b.HasIndex("CartoonId");

                    b.ToTable("CartoonRating", t =>
                        {
                            t.HasCheckConstraint("CK_Rating_Rate_Range", "[Rate] >= 1 AND [Rate] <= 10");
                        });
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Selection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Selections");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.SelectionCartoon", b =>
                {
                    b.Property<int>("CartoonId")
                        .HasColumnType("int");

                    b.Property<int>("SelectionId")
                        .HasColumnType("int");

                    b.HasKey("CartoonId", "SelectionId");

                    b.HasIndex("SelectionId");

                    b.ToTable("SelectionCartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Studio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Studios");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.StudiosCartoon", b =>
                {
                    b.Property<int>("CartoonId")
                        .HasColumnType("int");

                    b.Property<int>("StudioId")
                        .HasColumnType("int");

                    b.HasKey("CartoonId", "StudioId");

                    b.HasIndex("StudioId");

                    b.ToTable("StudiosCartoons");
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

                    b.Navigation("AnimationType");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.CartoonPart", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Cartoon", "Cartoon")
                        .WithMany("CartoonParts")
                        .HasForeignKey("CartoonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cartoon");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.CategoriesCartoon", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Cartoon", "Cartoon")
                        .WithMany("CategoriesCartoons")
                        .HasForeignKey("CartoonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.Category", "Category")
                        .WithMany("CategoriesCartoons")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cartoon");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Comment", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Cartoon", "Cartoon")
                        .WithMany("Comments")
                        .HasForeignKey("CartoonId");

                    b.HasOne("DataAccessLayer.Entities.CartoonPart", "CartoonPart")
                        .WithMany()
                        .HasForeignKey("CartoonPartId");

                    b.HasOne("DataAccessLayer.Entities.Comment", "ParentComment")
                        .WithMany()
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("DataAccessLayer.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cartoon");

                    b.Navigation("CartoonPart");

                    b.Navigation("ParentComment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.CommentDislike", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Comment", "Comment")
                        .WithMany("CommentDislikes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.User", "User")
                        .WithMany("CommentDislikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.CommentLike", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Comment", "Comment")
                        .WithMany("CommentLikes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.User", "User")
                        .WithMany("CommentLikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Comment");

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

            modelBuilder.Entity("DataAccessLayer.Entities.Rating", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Cartoon", "Cartoon")
                        .WithMany("CartoonRatings")
                        .HasForeignKey("CartoonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cartoon");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.SelectionCartoon", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Cartoon", "Cartoon")
                        .WithMany("SelectionCartoons")
                        .HasForeignKey("CartoonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.Selection", "Selection")
                        .WithMany("SelectionsAnimes")
                        .HasForeignKey("SelectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cartoon");

                    b.Navigation("Selection");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.StudiosCartoon", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Cartoon", "Cartoon")
                        .WithMany("StudiosCartoons")
                        .HasForeignKey("CartoonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.Studio", "Studio")
                        .WithMany("StudiosCartoons")
                        .HasForeignKey("StudioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cartoon");

                    b.Navigation("Studio");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.AnimationType", b =>
                {
                    b.Navigation("Cartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Cartoon", b =>
                {
                    b.Navigation("CartoonParts");

                    b.Navigation("CartoonRatings");

                    b.Navigation("CategoriesCartoons");

                    b.Navigation("Comments");

                    b.Navigation("GenresCartoons");

                    b.Navigation("SelectionCartoons");

                    b.Navigation("StudiosCartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Category", b =>
                {
                    b.Navigation("CategoriesCartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Comment", b =>
                {
                    b.Navigation("CommentDislikes");

                    b.Navigation("CommentLikes");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Genre", b =>
                {
                    b.Navigation("GenresCartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Selection", b =>
                {
                    b.Navigation("SelectionsAnimes");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Studio", b =>
                {
                    b.Navigation("StudiosCartoons");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.User", b =>
                {
                    b.Navigation("CommentDislikes");

                    b.Navigation("CommentLikes");

                    b.Navigation("Comments");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
