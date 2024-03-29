﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkLogerCA.Models;

#nullable disable

namespace WorkLogerCA.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211211200123_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WorkLogerCA.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("ArrivalDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CertificateExpiryDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DepartureDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Document")
                        .HasColumnType("longtext");

                    b.Property<string>("EquipmentIdentification")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FactoryNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<string>("SentFrom")
                        .HasColumnType("longtext");

                    b.Property<string>("State")
                        .HasColumnType("longtext");

                    b.Property<int>("TransportId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.HasIndex("TransportId");

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("WorkLogerCA.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("CompletedRequest")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ContractorNote")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateTimeSendRequest")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Note")
                        .HasColumnType("longtext");

                    b.Property<int?>("NumberDrillingCrew")
                        .HasColumnType("int");

                    b.Property<string>("PlaceOfWork")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RepeatedRequest")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RequestDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RequestDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RequestNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("RequestState")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SendResult")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Request");
                });

            modelBuilder.Entity("WorkLogerCA.Models.Transport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ArrivalDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DetpartureDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DirectRide")
                        .HasColumnType("longtext");

                    b.Property<string>("DriverFullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Note")
                        .HasColumnType("longtext");

                    b.Property<string>("Passengers")
                        .HasColumnType("longtext");

                    b.Property<string>("ReturnDestination")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ReturnRide")
                        .HasColumnType("longtext");

                    b.Property<int?>("WaybillNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Transport");
                });

            modelBuilder.Entity("WorkLogerCA.Models.Work", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CompletionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DescriptionOfPerformedWork")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Note")
                        .HasColumnType("longtext");

                    b.Property<int>("PerformersOfWork")
                        .HasColumnType("int");

                    b.Property<string>("PlaceOfWork")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TransportId")
                        .HasColumnType("int");

                    b.Property<bool>("WorkCompletiting")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("TransportId");

                    b.ToTable("Work");
                });

            modelBuilder.Entity("WorkLogerCA.Models.Equipment", b =>
                {
                    b.HasOne("WorkLogerCA.Models.Request", "Request")
                        .WithMany("Equipment")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkLogerCA.Models.Transport", "Transport")
                        .WithMany("Equipment")
                        .HasForeignKey("TransportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Request");

                    b.Navigation("Transport");
                });

            modelBuilder.Entity("WorkLogerCA.Models.Work", b =>
                {
                    b.HasOne("WorkLogerCA.Models.Transport", "Transport")
                        .WithMany("ExecutionOfWork")
                        .HasForeignKey("TransportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transport");
                });

            modelBuilder.Entity("WorkLogerCA.Models.Request", b =>
                {
                    b.Navigation("Equipment");
                });

            modelBuilder.Entity("WorkLogerCA.Models.Transport", b =>
                {
                    b.Navigation("Equipment");

                    b.Navigation("ExecutionOfWork");
                });
#pragma warning restore 612, 618
        }
    }
}
