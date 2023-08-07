﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Data;

#nullable disable

namespace WebApplication1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230806214049_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.6.23329.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApplication1.Models.Shirt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsForMen")
                        .HasColumnType("bit");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<int?>("Size")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Shirts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BrandName = "Nike",
                            Color = "Blue",
                            IsForMen = true,
                            Price = 30.0,
                            Size = 10
                        },
                        new
                        {
                            Id = 2,
                            BrandName = "Nike",
                            Color = "Black",
                            IsForMen = true,
                            Price = 35.0,
                            Size = 12
                        },
                        new
                        {
                            Id = 3,
                            BrandName = "Adidas",
                            Color = "Pink",
                            IsForMen = false,
                            Price = 28.0,
                            Size = 8
                        },
                        new
                        {
                            Id = 4,
                            BrandName = "Adidas",
                            Color = "Yellow",
                            IsForMen = false,
                            Price = 30.0,
                            Size = 9
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
