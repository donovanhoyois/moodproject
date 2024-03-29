﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoodProject.Api;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoodProject.Api.Migrations
{
    [DbContext(typeof(MoodProjectContext))]
    [Migration("20231226113951_MedicationDayUsage_utcOffset")]
    partial class MedicationDayUsage_utcOffset
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MoodProject.Core.Models.ChatRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SymptomId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SymptomId");

                    b.ToTable("ChatRooms");
                });

            modelBuilder.Entity("MoodProject.Core.Models.ChatRoomComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ChatRoomPostId")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ModerationStatus")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ChatRoomPostId");

                    b.ToTable("ChatRoomComments");
                });

            modelBuilder.Entity("MoodProject.Core.Models.ChatRoomPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ChatRoomId")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ModerationStatus")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChatRoomId");

                    b.ToTable("ChatRoomPosts");
                });

            modelBuilder.Entity("MoodProject.Core.Models.CustomQuizzQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FactorType")
                        .HasColumnType("integer");

                    b.Property<int>("SymptomTypeId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SymptomTypeId");

                    b.ToTable("QuizzQuestions");
                });

            modelBuilder.Entity("MoodProject.Core.Models.FactorValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("SymptomId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<float>("Value")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("SymptomId");

                    b.ToTable("FactorValues");
                });

            modelBuilder.Entity("MoodProject.Core.Models.Medication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("AreNotificationsEnabled")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("boolean");

                    b.Property<int>("MonthUsage")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Usage")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Medications");
                });

            modelBuilder.Entity("MoodProject.Core.Models.MedicationDayUsage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MedicationId")
                        .HasColumnType("integer");

                    b.Property<TimeOnly>("TimeOfTheDay")
                        .HasColumnType("time without time zone");

                    b.Property<int>("UtcOffset")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MedicationId");

                    b.ToTable("MedicationDayUsages");
                });

            modelBuilder.Entity("MoodProject.Core.Models.Notifications.NotificationSubscription", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Auth")
                        .HasColumnType("text");

                    b.Property<string>("P256dh")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("NotificationSubscriptions");
                });

            modelBuilder.Entity("MoodProject.Core.Models.QuizzAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CustomQuizzQuestionId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("CustomQuizzQuestionId");

                    b.ToTable("QuizzAnswers");
                });

            modelBuilder.Entity("MoodProject.Core.Models.Symptom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("TypeId")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isDisabled")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Symptoms");
                });

            modelBuilder.Entity("MoodProject.Core.Models.SymptomType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SymptomTypes");
                });

            modelBuilder.Entity("MoodProject.Core.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthProviderUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("HasAcceptedGdpr")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MoodProject.Core.Models.ChatRoom", b =>
                {
                    b.HasOne("MoodProject.Core.Models.Symptom", "Symptom")
                        .WithMany()
                        .HasForeignKey("SymptomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Symptom");
                });

            modelBuilder.Entity("MoodProject.Core.Models.ChatRoomComment", b =>
                {
                    b.HasOne("MoodProject.Core.Models.ChatRoomPost", null)
                        .WithMany("Comments")
                        .HasForeignKey("ChatRoomPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MoodProject.Core.Models.ChatRoomPost", b =>
                {
                    b.HasOne("MoodProject.Core.Models.ChatRoom", null)
                        .WithMany("Posts")
                        .HasForeignKey("ChatRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MoodProject.Core.Models.CustomQuizzQuestion", b =>
                {
                    b.HasOne("MoodProject.Core.Models.SymptomType", "SymptomType")
                        .WithMany()
                        .HasForeignKey("SymptomTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SymptomType");
                });

            modelBuilder.Entity("MoodProject.Core.Models.FactorValue", b =>
                {
                    b.HasOne("MoodProject.Core.Models.Symptom", null)
                        .WithMany("ValuesHistory")
                        .HasForeignKey("SymptomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MoodProject.Core.Models.MedicationDayUsage", b =>
                {
                    b.HasOne("MoodProject.Core.Models.Medication", null)
                        .WithMany("DayUsages")
                        .HasForeignKey("MedicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MoodProject.Core.Models.QuizzAnswer", b =>
                {
                    b.HasOne("MoodProject.Core.Models.CustomQuizzQuestion", null)
                        .WithMany("AnswerPossibilities")
                        .HasForeignKey("CustomQuizzQuestionId");
                });

            modelBuilder.Entity("MoodProject.Core.Models.Symptom", b =>
                {
                    b.HasOne("MoodProject.Core.Models.SymptomType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("MoodProject.Core.Models.ChatRoom", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("MoodProject.Core.Models.ChatRoomPost", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("MoodProject.Core.Models.CustomQuizzQuestion", b =>
                {
                    b.Navigation("AnswerPossibilities");
                });

            modelBuilder.Entity("MoodProject.Core.Models.Medication", b =>
                {
                    b.Navigation("DayUsages");
                });

            modelBuilder.Entity("MoodProject.Core.Models.Symptom", b =>
                {
                    b.Navigation("ValuesHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
