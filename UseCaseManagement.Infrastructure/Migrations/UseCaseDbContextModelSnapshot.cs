﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UseCaseManagement.Infrastructure;

#nullable disable

namespace UseCaseManagement.Infrastructure.Migrations
{
    [DbContext(typeof(UseCaseDbContext))]
    partial class UseCaseDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LogSourceUseCase", b =>
                {
                    b.Property<Guid>("LogSourcesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UseCasesId")
                        .HasColumnType("uuid");

                    b.HasKey("LogSourcesId", "UseCasesId");

                    b.HasIndex("UseCasesId");

                    b.ToTable("UseCaseLogSources", (string)null);
                });

            modelBuilder.Entity("UseCaseManagement.Domain.LogSource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("LogSource");
                });

            modelBuilder.Entity("UseCaseManagement.Domain.LogSourceFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FileSize")
                        .HasColumnType("integer");

                    b.Property<Guid>("LogSourceId")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LogSourceId");

                    b.ToTable("LogSourceFile");
                });

            modelBuilder.Entity("UseCaseManagement.Domain.UseCase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("MitreAttacks")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Tag")
                        .HasColumnType("text");

                    b.Property<List<Guid>>("Tenants")
                        .IsRequired()
                        .HasColumnType("uuid[]");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("UseCase");
                });

            modelBuilder.Entity("UseCaseManagement.Domain.UseCaseFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FileSize")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UseCaseId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UseCaseId");

                    b.ToTable("UseCaseFile");
                });

            modelBuilder.Entity("UseCaseManagement.Domain.Vendor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Vendor");
                });

            modelBuilder.Entity("UseCaseVendor", b =>
                {
                    b.Property<Guid>("UseCasesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("VendorsId")
                        .HasColumnType("uuid");

                    b.HasKey("UseCasesId", "VendorsId");

                    b.HasIndex("VendorsId");

                    b.ToTable("VendorUseCase", (string)null);
                });

            modelBuilder.Entity("LogSourceUseCase", b =>
                {
                    b.HasOne("UseCaseManagement.Domain.LogSource", null)
                        .WithMany()
                        .HasForeignKey("LogSourcesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UseCaseManagement.Domain.UseCase", null)
                        .WithMany()
                        .HasForeignKey("UseCasesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UseCaseManagement.Domain.LogSourceFile", b =>
                {
                    b.HasOne("UseCaseManagement.Domain.LogSource", "LogSource")
                        .WithMany("Files")
                        .HasForeignKey("LogSourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LogSource");
                });

            modelBuilder.Entity("UseCaseManagement.Domain.UseCaseFile", b =>
                {
                    b.HasOne("UseCaseManagement.Domain.UseCase", "UseCase")
                        .WithMany("Files")
                        .HasForeignKey("UseCaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UseCase");
                });

            modelBuilder.Entity("UseCaseVendor", b =>
                {
                    b.HasOne("UseCaseManagement.Domain.UseCase", null)
                        .WithMany()
                        .HasForeignKey("UseCasesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UseCaseManagement.Domain.Vendor", null)
                        .WithMany()
                        .HasForeignKey("VendorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UseCaseManagement.Domain.LogSource", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("UseCaseManagement.Domain.UseCase", b =>
                {
                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}
