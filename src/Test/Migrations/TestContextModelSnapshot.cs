﻿// <auto-generated />
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Migrations {
    [DbContext(typeof(TestContext))]
    // ReSharper disable once UnusedMember.Global
    internal class TestContextModelSnapshot : ModelSnapshot {
        protected override void BuildModel(ModelBuilder modelBuilder) {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Data.TestData", b => {
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
