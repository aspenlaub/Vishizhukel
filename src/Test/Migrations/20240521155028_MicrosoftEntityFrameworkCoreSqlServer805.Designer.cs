﻿// <auto-generated />
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Migrations
{
    [DbContext(typeof(TestContext))]
    [Migration("20240521155028_MicrosoftEntityFrameworkCoreSqlServer805")]
    partial class MicrosoftEntityFrameworkCoreSqlServer805
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data.TestData", b =>
                {
                    b.Property<string>("Guid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Guid");

                    b.ToTable("TestDatas");
                });
#pragma warning restore 612, 618
        }
    }
}