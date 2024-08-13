using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

public partial class DbestateContext : DbContext
{
    public DbestateContext()
    {
    }

    public DbestateContext(DbContextOptions<DbestateContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BuildingType> BuildingTypes { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<OperationType> OperationTypes { get; set; }

    public virtual DbSet<OwnerType> OwnerTypes { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<PropertyType> PropertyTypes { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<RegionUnit01> RegionUnit01s { get; set; }

    public virtual DbSet<RepairRate> RepairRates { get; set; }

    public virtual DbSet<RoomCount> RoomCounts { get; set; }

    public virtual DbSet<Target> Targets { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MATINNOTEBOOK;Initial Catalog=dbestate;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BuildingType>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("building_type");

            entity.HasIndex(e => e.IdBuildingType, "IX_building_type");

            entity.Property(e => e.BuildingTypeName)
                .HasMaxLength(50)
                .HasColumnName("building_type_name");
            entity.Property(e => e.IdBuildingType).HasColumnName("id_building_type");
            entity.Property(e => e.Keyword)
                .HasMaxLength(500)
                .HasColumnName("keyword");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.IdCurrency);

            entity.ToTable("currency");

            entity.Property(e => e.IdCurrency)
                .ValueGeneratedNever()
                .HasColumnName("id_currency");
            entity.Property(e => e.Currency1)
                .HasMaxLength(50)
                .HasColumnName("currency");
            entity.Property(e => e.Keyword)
                .HasMaxLength(500)
                .HasColumnName("keyword");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.IdDocument);

            entity.ToTable("document");

            entity.Property(e => e.IdDocument)
                .ValueGeneratedNever()
                .HasColumnName("id_document");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(50)
                .HasColumnName("document_name");
            entity.Property(e => e.Keyword)
                .HasMaxLength(500)
                .HasColumnName("keyword");
            entity.Property(e => e.Keyword01)
                .HasMaxLength(50)
                .HasColumnName("keyword_01");
            entity.Property(e => e.Keyword02)
                .HasMaxLength(50)
                .HasColumnName("keyword_02");
            entity.Property(e => e.Keyword03)
                .HasMaxLength(50)
                .HasColumnName("keyword_03");
        });

        modelBuilder.Entity<OperationType>(entity =>
        {
            entity.HasKey(e => e.IdOperationType);

            entity.ToTable("operation_type");

            entity.Property(e => e.IdOperationType)
                .ValueGeneratedNever()
                .HasColumnName("id_operation_type");
            entity.Property(e => e.Keyword)
                .HasMaxLength(500)
                .HasColumnName("keyword");
            entity.Property(e => e.OperationTypeName)
                .HasMaxLength(50)
                .HasColumnName("operation_type_name");
        });

        modelBuilder.Entity<OwnerType>(entity =>
        {
            entity.HasKey(e => e.IdOwnerType);

            entity.ToTable("owner_type");

            entity.Property(e => e.IdOwnerType)
                .ValueGeneratedNever()
                .HasColumnName("id_owner_type");
            entity.Property(e => e.Keyword)
                .HasMaxLength(500)
                .HasColumnName("keyword");
            entity.Property(e => e.OwnerTypeName)
                .HasMaxLength(50)
                .HasColumnName("owner_type_name");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("property");

            entity.Property(e => e.Address)
                .HasColumnType("ntext")
                .HasColumnName("address");
            entity.Property(e => e.ApprovmentMessage)
                .HasMaxLength(255)
                .HasColumnName("approvment_message");
            entity.Property(e => e.ApprovmentStatus).HasColumnName("approvment_status");
            entity.Property(e => e.Area).HasColumnName("area");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.CpName)
                .HasMaxLength(50)
                .HasColumnName("cp_name");
            entity.Property(e => e.CpPhoneNumber01)
                .HasMaxLength(50)
                .HasColumnName("cp_phone_number_01");
            entity.Property(e => e.CpPhoneNumber02)
                .HasMaxLength(50)
                .HasColumnName("cp_phone_number_02");
            entity.Property(e => e.CpPhoneNumber03)
                .HasMaxLength(50)
                .HasColumnName("cp_phone_number_03");
            entity.Property(e => e.Data)
                .HasColumnType("ntext")
                .HasColumnName("data");
            entity.Property(e => e.EX)
                .HasMaxLength(50)
                .HasColumnName("eX");
            entity.Property(e => e.EY)
                .HasMaxLength(50)
                .HasColumnName("eY");
            entity.Property(e => e.FkIdBuildingType).HasColumnName("fk_id_building_type");
            entity.Property(e => e.FkIdCity).HasColumnName("fk_id_city");
            entity.Property(e => e.FkIdCurrency).HasColumnName("fk_id_currency");
            entity.Property(e => e.FkIdDocument).HasColumnName("fk_id_document");
            entity.Property(e => e.FkIdLink).HasColumnName("fk_id_link");
            entity.Property(e => e.FkIdMetro).HasColumnName("fk_id_metro");
            entity.Property(e => e.FkIdOperationType).HasColumnName("fk_id_operation_type");
            entity.Property(e => e.FkIdOwnerType).HasColumnName("fk_id_owner_type");
            entity.Property(e => e.FkIdPropertyType).HasColumnName("fk_id_property_type");
            entity.Property(e => e.FkIdRepair).HasColumnName("fk_id_repair");
            entity.Property(e => e.FkIdRoom).HasColumnName("fk_id_room");
            entity.Property(e => e.FkIdSource).HasColumnName("fk_id_source");
            entity.Property(e => e.FkIdTarget).HasColumnName("fk_id_target");
            entity.Property(e => e.Floor).HasColumnName("floor");
            entity.Property(e => e.FloorOf).HasColumnName("floor_of");
            entity.Property(e => e.GeneralArea).HasColumnName("general_area");
            entity.Property(e => e.IdProperty).HasColumnName("id_property");
            entity.Property(e => e.Images)
                .HasColumnType("ntext")
                .HasColumnName("images");
            entity.Property(e => e.InsertDate)
                .HasPrecision(3)
                .HasColumnName("insert_date");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");
            entity.Property(e => e.UploadMessage)
                .HasMaxLength(250)
                .HasColumnName("upload_message");
            entity.Property(e => e.UploadStatus).HasColumnName("upload_status");
        });

        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.HasKey(e => e.IdPropertyType);

            entity.ToTable("property_type");

            entity.Property(e => e.IdPropertyType)
                .ValueGeneratedNever()
                .HasColumnName("id_property_type");
            entity.Property(e => e.PropertyTypeName)
                .HasMaxLength(50)
                .HasColumnName("property_type_name");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("region");

            entity.HasIndex(e => e.IdRegion, "IX_id_region").IsUnique();

            entity.HasIndex(e => e.RegionCode, "IX_region_code");

            entity.Property(e => e.IdRegion).HasColumnName("id_region");
            entity.Property(e => e.Keyword01)
                .HasMaxLength(50)
                .HasColumnName("keyword_01");
            entity.Property(e => e.Keyword02)
                .HasMaxLength(50)
                .HasColumnName("keyword_02");
            entity.Property(e => e.Keyword03)
                .HasMaxLength(50)
                .HasColumnName("keyword_03");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(25)
                .HasColumnName("region_code");
            entity.Property(e => e.RegionName)
                .HasMaxLength(255)
                .HasColumnName("region_name");
        });

        modelBuilder.Entity<RegionUnit01>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("region_unit_01");

            entity.HasIndex(e => e.IdRegion, "IX_region_unit_01_id_region").IsUnique();

            entity.HasIndex(e => e.RegionCode, "IX_region_unit_01_region_code");

            entity.Property(e => e.IdRegion).HasColumnName("id_region");
            entity.Property(e => e.Keyword01)
                .HasMaxLength(50)
                .HasColumnName("keyword_01");
            entity.Property(e => e.Keyword02)
                .HasMaxLength(50)
                .HasColumnName("keyword_02");
            entity.Property(e => e.Keyword03)
                .HasMaxLength(50)
                .HasColumnName("keyword_03");
            entity.Property(e => e.RegionCode)
                .HasMaxLength(25)
                .HasColumnName("region_code");
            entity.Property(e => e.RegionName)
                .HasMaxLength(255)
                .HasColumnName("region_name");
        });

        modelBuilder.Entity<RepairRate>(entity =>
        {
            entity.HasKey(e => e.IdRepairRate);

            entity.ToTable("repair_rate");

            entity.Property(e => e.IdRepairRate)
                .ValueGeneratedNever()
                .HasColumnName("id_repair_rate");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(0)
                .HasColumnName("is_active");
            entity.Property(e => e.RepairRateName)
                .HasMaxLength(50)
                .HasColumnName("repair_rate_name");
        });

        modelBuilder.Entity<RoomCount>(entity =>
        {
            entity.HasKey(e => e.IdRoomCount);

            entity.ToTable("room_count");

            entity.Property(e => e.IdRoomCount)
                .ValueGeneratedNever()
                .HasColumnName("id_room_count");
            entity.Property(e => e.Keyword)
                .HasMaxLength(50)
                .HasColumnName("keyword");
            entity.Property(e => e.RoomCountName)
                .HasMaxLength(50)
                .HasColumnName("room_count_name");
        });

        modelBuilder.Entity<Target>(entity =>
        {
            entity.HasKey(e => e.IdTarget);

            entity.ToTable("target");

            entity.Property(e => e.IdTarget).HasColumnName("id_target");
            entity.Property(e => e.FkIdRegion).HasColumnName("fk_id_region");
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("insert_date");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(0)
                .HasColumnName("is_deleted");
            entity.Property(e => e.TargetName)
                .HasMaxLength(255)
                .HasColumnName("target_name");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tokens_web_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("web_user");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
