﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PasswordsManager.DataAccess;

namespace PasswordsManager.Migrations
{
    [DbContext(typeof(PasswordsDbContext))]
    [Migration("20220923162730_More inverse property for Password Model")]
    partial class MoreinversepropertyforPasswordModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13");

            modelBuilder.Entity("PasswordsManager.Model.Parameter", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("PasswordsManager.Model.Password", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Login")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Pass")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Passwords");
                });

            modelBuilder.Entity("PasswordsManager.Model.PasswordTag", b =>
                {
                    b.Property<int>("PasswordId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TagLabel")
                        .HasColumnType("TEXT");

                    b.HasKey("PasswordId", "TagLabel");

                    b.HasIndex("TagLabel");

                    b.ToTable("PasswordTags");
                });

            modelBuilder.Entity("PasswordsManager.Model.PasswordUserAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PasswordId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PasswordId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("PasswordUserAccounts");
                });

            modelBuilder.Entity("PasswordsManager.Model.Tag", b =>
                {
                    b.Property<string>("Label")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Label");

                    b.HasIndex("UserId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("PasswordsManager.Model.UserAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("PasswordsManager.Model.PasswordTag", b =>
                {
                    b.HasOne("PasswordsManager.Model.Password", "Password")
                        .WithMany("PasswordTagsBelong")
                        .HasForeignKey("PasswordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PasswordsManager.Model.Tag", "Tag")
                        .WithMany("PasswordTagsBelong")
                        .HasForeignKey("TagLabel")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PasswordsManager.Model.PasswordUserAccount", b =>
                {
                    b.HasOne("PasswordsManager.Model.Password", "Password")
                        .WithOne("PasswordUserAccountBelong")
                        .HasForeignKey("PasswordsManager.Model.PasswordUserAccount", "PasswordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PasswordsManager.Model.UserAccount", "User")
                        .WithMany("PasswordsBelong")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PasswordsManager.Model.Tag", b =>
                {
                    b.HasOne("PasswordsManager.Model.UserAccount", "User")
                        .WithMany("TagsBelong")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
