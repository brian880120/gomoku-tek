using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Tek.Gomoku.Service.Models;

namespace Tek.Gomoku.Service.Migrations
{
    [DbContext(typeof(GameContext))]
    [Migration("20170419015027_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Tek.Gomoku.Service.Models.GameMove", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ColumnIndex");

                    b.Property<string>("PlayerName");

                    b.Property<int>("RowIndex");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("ID");

                    b.ToTable("GameMove");
                });
        }
    }
}
