﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SilveR.Models;

namespace SilveR.Migrations
{
    [DbContext(typeof(SilveRContext))]
    partial class SilveRContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity("SilveR.Models.Analysis", b =>
                {
                    b.Property<int>("AnalysisID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AnalysisGuid")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int?>("DatasetID");

                    b.Property<string>("DatasetName")
                        .HasMaxLength(50);

                    b.Property<DateTime>("DateAnalysed")
                        .HasColumnType("datetime");

                    b.Property<string>("HtmlOutput");

                    b.Property<string>("RProcessOutput");

                    b.Property<int>("ScriptID");

                    b.Property<string>("Tag")
                        .HasMaxLength(200)
                        .IsUnicode(false);

                    b.HasKey("AnalysisID");

                    b.HasIndex("DatasetID");

                    b.HasIndex("ScriptID");

                    b.ToTable("Analyses");
                });

            modelBuilder.Entity("SilveR.Models.Argument", b =>
                {
                    b.Property<int>("ArgumentID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnalysisID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Value")
                        .HasMaxLength(100);

                    b.HasKey("ArgumentID");

                    b.HasIndex("AnalysisID");

                    b.ToTable("Arguments");
                });

            modelBuilder.Entity("SilveR.Models.Dataset", b =>
                {
                    b.Property<int>("DatasetID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DatasetName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("TheData")
                        .IsRequired();

                    b.Property<int>("VersionNo");

                    b.HasKey("DatasetID");

                    b.ToTable("Datasets");
                });

            modelBuilder.Entity("SilveR.Models.Script", b =>
                {
                    b.Property<int>("ScriptID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("RequiresDataset");

                    b.Property<string>("ScriptDisplayName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("ScriptFileName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ScriptID");

                    b.ToTable("Scripts");
                });

            modelBuilder.Entity("SilveR.Models.UserOption", b =>
                {
                    b.Property<int>("UserOptionID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AssessCovariateInteractions");

                    b.Property<string>("BWFill")
                        .IsRequired();

                    b.Property<string>("BWLine")
                        .IsRequired();

                    b.Property<string>("CategoryBarFill")
                        .IsRequired();

                    b.Property<string>("ColourFill")
                        .IsRequired();

                    b.Property<string>("ColourLine")
                        .IsRequired();

                    b.Property<bool>("CovariateRegressionCoefficients");

                    b.Property<bool>("DisplayLSMeansLines");

                    b.Property<bool>("DisplayPointLabels");

                    b.Property<bool>("DisplaySEMLines");

                    b.Property<decimal>("ErrorBarWidth");

                    b.Property<string>("FontStyle")
                        .IsRequired();

                    b.Property<bool>("GeometryDisplay");

                    b.Property<decimal>("GraphicsBWHigh");

                    b.Property<decimal>("GraphicsBWLow");

                    b.Property<string>("GraphicsFont")
                        .IsRequired();

                    b.Property<decimal>("GraphicsHeightJitter");

                    b.Property<string>("GraphicsTextColour")
                        .IsRequired();

                    b.Property<decimal>("GraphicsWidthJitter");

                    b.Property<int>("GraphicsXAngle");

                    b.Property<decimal>("GraphicsXHorizontalJust");

                    b.Property<int>("GraphicsYAngle");

                    b.Property<decimal>("GraphicsYVerticalJust");

                    b.Property<int>("JpegHeight");

                    b.Property<int>("JpegWidth");

                    b.Property<string>("LegendPosition")
                        .IsRequired();

                    b.Property<string>("LegendTextColour")
                        .IsRequired();

                    b.Property<int>("LegendTextSize");

                    b.Property<int>("LineSize");

                    b.Property<string>("LineTypeDashed")
                        .IsRequired();

                    b.Property<string>("LineTypeSolid")
                        .IsRequired();

                    b.Property<bool>("OutputData");

                    b.Property<bool>("OutputPlotsInBW");

                    b.Property<string>("PaletteSet")
                        .IsRequired();

                    b.Property<int>("PointShape");

                    b.Property<int>("PointSize");

                    b.Property<int>("TitleSize");

                    b.Property<int>("XAxisTitleFontSize");

                    b.Property<int>("XLabelsFontSize");

                    b.Property<int>("YAxisTitleFontSize");

                    b.Property<int>("YLabelsFontSize");

                    b.HasKey("UserOptionID");

                    b.ToTable("UserOptions");
                });

            modelBuilder.Entity("SilveR.Models.Analysis", b =>
                {
                    b.HasOne("SilveR.Models.Dataset", "Dataset")
                        .WithMany("Analysis")
                        .HasForeignKey("DatasetID")
                        .HasConstraintName("FK_Analyses_Datasets");

                    b.HasOne("SilveR.Models.Script", "Script")
                        .WithMany("Analysis")
                        .HasForeignKey("ScriptID")
                        .HasConstraintName("FK_Analyses_Scripts");
                });

            modelBuilder.Entity("SilveR.Models.Argument", b =>
                {
                    b.HasOne("SilveR.Models.Analysis", "Analysis")
                        .WithMany("Arguments")
                        .HasForeignKey("AnalysisID")
                        .HasConstraintName("FK_Arguments_Analyses")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
