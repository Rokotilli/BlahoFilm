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
    [DbContext(typeof(SeriesServiceDbContext))]
    partial class SeriesServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccessLayer.Entities.CategoriesSeries", b =>
                {
                    b.Property<int>("SeriesId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("SeriesId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoriesSeries");
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

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ParentCommentId")
                        .HasColumnType("int");

                    b.Property<int?>("SeriesId")
                        .HasColumnType("int");

                    b.Property<int?>("SeriesPartId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("SeriesId");

                    b.HasIndex("SeriesPartId");

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

            modelBuilder.Entity("DataAccessLayer.Entities.GenresSeries", b =>
                {
                    b.Property<int>("SeriesId")
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.HasKey("SeriesId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("GenresSeries");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Rating", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SeriesId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.HasKey("UserId", "SeriesId");

                    b.HasIndex("SeriesId");

                    b.ToTable("Ratings", t =>
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

            modelBuilder.Entity("DataAccessLayer.Entities.SelectionSeries", b =>
                {
                    b.Property<int>("SeriesId")
                        .HasColumnType("int");

                    b.Property<int>("SelectionId")
                        .HasColumnType("int");

                    b.HasKey("SeriesId", "SelectionId");

                    b.HasIndex("SelectionId");

                    b.ToTable("SelectionSeries");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Series", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Actors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AgeRestriction")
                        .HasColumnType("int");

                    b.Property<int>("CountParts")
                        .HasColumnType("int");

                    b.Property<int>("CountSeasons")
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

                    b.ToTable("Series");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.SeriesPart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PartNumber")
                        .HasColumnType("int");

                    b.Property<int>("SeasonNumber")
                        .HasColumnType("int");

                    b.Property<int>("SeriesId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SeriesId");

                    b.ToTable("SeriesParts");
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

            modelBuilder.Entity("DataAccessLayer.Entities.StudiosSeries", b =>
                {
                    b.Property<int>("SeriesId")
                        .HasColumnType("int");

                    b.Property<int>("StudioId")
                        .HasColumnType("int");

                    b.HasKey("SeriesId", "StudioId");

                    b.HasIndex("StudioId");

                    b.ToTable("StudiosSeries");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.CategoriesSeries", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Category", "Category")
                        .WithMany("CategoriesSeries")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.Series", "Series")
                        .WithMany("CategoriesSeries")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Comment", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Comment", "ParentComment")
                        .WithMany()
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("DataAccessLayer.Entities.Series", null)
                        .WithMany("Comments")
                        .HasForeignKey("SeriesId");

                    b.HasOne("DataAccessLayer.Entities.SeriesPart", "SeriesPart")
                        .WithMany("Comments")
                        .HasForeignKey("SeriesPartId");

                    b.HasOne("DataAccessLayer.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentComment");

                    b.Navigation("SeriesPart");

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

            modelBuilder.Entity("DataAccessLayer.Entities.GenresSeries", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Genre", "Genre")
                        .WithMany("SeriesGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.Series", "Series")
                        .WithMany("GenresSeries")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Rating", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Series", "Series")
                        .WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Series");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.SelectionSeries", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Selection", "Selection")
                        .WithMany("SelectionsSeries")
                        .HasForeignKey("SelectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.Series", "Series")
                        .WithMany("SelectionSeries")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Selection");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.SeriesPart", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Series", "Series")
                        .WithMany("SeriesParts")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Series");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.StudiosSeries", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Series", "Series")
                        .WithMany("StudiosSeries")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccessLayer.Entities.Studio", "Studio")
                        .WithMany("StudiosSeries")
                        .HasForeignKey("StudioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Series");

                    b.Navigation("Studio");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Category", b =>
                {
                    b.Navigation("CategoriesSeries");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Comment", b =>
                {
                    b.Navigation("CommentDislikes");

                    b.Navigation("CommentLikes");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Genre", b =>
                {
                    b.Navigation("SeriesGenres");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Selection", b =>
                {
                    b.Navigation("SelectionsSeries");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Series", b =>
                {
                    b.Navigation("CategoriesSeries");

                    b.Navigation("Comments");

                    b.Navigation("GenresSeries");

                    b.Navigation("SelectionSeries");

                    b.Navigation("SeriesParts");

                    b.Navigation("StudiosSeries");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.SeriesPart", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Studio", b =>
                {
                    b.Navigation("StudiosSeries");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.User", b =>
                {
                    b.Navigation("CommentDislikes");

                    b.Navigation("CommentLikes");

                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
