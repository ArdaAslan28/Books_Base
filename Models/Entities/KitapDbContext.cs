﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace books.Models.Entities;

public partial class KitapDbContext : DbContext
{
    public KitapDbContext()
    {
    }

    public KitapDbContext(DbContextOptions<KitapDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Diller> Dillers { get; set; }

    public virtual DbSet<Iletisim> Iletisims { get; set; }

    public virtual DbSet<Kitaplar> Kitaplars { get; set; }

    public virtual DbSet<Turler> Turlers { get; set; }

    public virtual DbSet<Turlertokitaplar> Turlertokitaplars { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Yayinevleri> Yayinevleris { get; set; }

    public virtual DbSet<Yazarlar> Yazarlars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;database=kitap_db;default command timeout=120;sslmode=none", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.36-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin5_turkish_ci")
            .HasCharSet("latin5");

        modelBuilder.Entity<Diller>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("diller");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.DilAdi)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("dilAdi");
        });

        modelBuilder.Entity<Iletisim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("iletisim");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Eposta)
                .HasMaxLength(100)
                .HasColumnName("eposta");
            entity.Property(e => e.Goruldu).HasColumnName("goruldu");
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("ip");
            entity.Property(e => e.Konu)
                .HasMaxLength(150)
                .HasColumnName("konu");
            entity.Property(e => e.Mesaj)
                .HasMaxLength(600)
                .HasColumnName("mesaj");
            entity.Property(e => e.TarihSaat)
                .HasColumnType("datetime")
                .HasColumnName("tarihSaat");
        });

        modelBuilder.Entity<Kitaplar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("kitaplar")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Adi)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasColumnName("adi")
                .UseCollation("latin5_turkish_ci")
                .HasCharSet("latin5");
            entity.Property(e => e.DilId)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("dilID");
            entity.Property(e => e.Ozet)
                .HasMaxLength(5000)
                .HasColumnName("ozet")
                .UseCollation("latin5_turkish_ci")
                .HasCharSet("latin5");
            entity.Property(e => e.Resim)
                .HasMaxLength(50)
                .HasColumnName("resim");
            entity.Property(e => e.SayfaSayisi)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("sayfaSayisi");
            entity.Property(e => e.YayinTarihi).HasColumnName("yayinTarihi");
            entity.Property(e => e.YayineviId)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("yayineviID");
            entity.Property(e => e.YazarId)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("yazarID");
        });

        modelBuilder.Entity<Turler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("turler")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Sira)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)");
            entity.Property(e => e.TurAdi)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("turAdi");
        });

        modelBuilder.Entity<Turlertokitaplar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("turlertokitaplar")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.KitapId)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("kitapID");
            entity.Property(e => e.TurId)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("turID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Yayinevleri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("yayinevleri");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Adres)
                .HasMaxLength(150)
                .HasColumnName("adres");
            entity.Property(e => e.Sira)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(4)")
                .HasColumnName("sira");
            entity.Property(e => e.Tel)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("tel");
            entity.Property(e => e.YayineviAdi)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasColumnName("yayineviAdi");
        });

        modelBuilder.Entity<Yazarlar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("yazarlar");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Adi)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("adi");
            entity.Property(e => e.Cinsiyeti).HasColumnName("cinsiyeti");
            entity.Property(e => e.DogumTarihi).HasColumnName("dogumTarihi");
            entity.Property(e => e.DogumYeri)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("dogumYeri");
            entity.Property(e => e.Soyadi)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("soyadi");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
