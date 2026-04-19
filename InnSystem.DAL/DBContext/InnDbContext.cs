using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
// se agrega la referencia a modelos
using InnSystem.Model;

namespace InnSystem.DAL.DBConext;

public partial class InnDbContext : DbContext
{
    public InnDbContext()
    {
    }

    public InnDbContext(DbContextOptions<InnDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActionLog> ActionLogs { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingStatus> BookingStatuses { get; set; }

    public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomImage> RoomImages { get; set; }

    public virtual DbSet<RoomStatus> RoomStatuses { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("btree_gist")
            .HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<ActionLog>(entity =>
        {
            entity.HasKey(e => e.IdLog).HasName("action_logs_pkey");

            entity.ToTable("action_logs");

            entity.Property(e => e.IdLog)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_log");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .HasColumnName("action");
            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("action_date");
            entity.Property(e => e.AffectedTable)
                .HasMaxLength(50)
                .HasColumnName("affected_table");
            entity.Property(e => e.Details)
                .HasColumnType("jsonb")
                .HasColumnName("details");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.RecordId)
                .HasMaxLength(100)
                .HasColumnName("record_id");
            entity.Property(e => e.SourceIp).HasColumnName("source_ip");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ActionLogs)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("action_logs_id_user_fkey");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.IdBooking).HasName("bookings_pkey");

            entity.ToTable("bookings");

            entity.Property(e => e.IdBooking)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_booking");
            entity.Property(e => e.CancelReason).HasColumnName("cancel_reason");
            entity.Property(e => e.CheckIn).HasColumnName("check_in");
            entity.Property(e => e.CheckOut).HasColumnName("check_out");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.GuestsCount).HasColumnName("guests_count");
            entity.Property(e => e.IdRoom).HasColumnName("id_room");
            entity.Property(e => e.IdStatus)
                .HasDefaultValue(1)
                .HasColumnName("id_status");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.TotalCost)
                .HasPrecision(10, 2)
                .HasColumnName("total_cost");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdRoomNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdRoom)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("bookings_id_room_fkey");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("bookings_status_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("bookings_id_user_fkey");
        });

        modelBuilder.Entity<BookingStatus>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("booking_statuses_pkey");

            entity.ToTable("booking_statuses");

            entity.HasIndex(e => e.Name, "booking_statuses_name_key").IsUnique();

            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<InvoiceType>(entity =>
        {
            entity.HasKey(e => e.IdType).HasName("invoice_types_pkey");

            entity.ToTable("invoice_types");

            entity.HasIndex(e => e.Name, "invoice_types_name_key").IsUnique();

            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.IdModule).HasName("modules_pkey");

            entity.ToTable("modules");

            entity.Property(e => e.IdModule).HasColumnName("id_module");
            entity.Property(e => e.FrontendPath)
                .HasMaxLength(150)
                .HasColumnName("frontend_path");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .HasColumnName("icon");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.IdPayment).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.Property(e => e.IdPayment)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_payment");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.ExternalRef).HasColumnName("external_ref");
            entity.Property(e => e.IdBooking).HasColumnName("id_booking");
            entity.Property(e => e.IdMethod).HasColumnName("id_method");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("payment_date");

            entity.HasOne(d => d.IdBookingNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdBooking)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("payments_id_booking_fkey");

            entity.HasOne(d => d.IdMethodNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdMethod)
                .HasConstraintName("payments_id_method_fkey");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("payments_id_status_fkey");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("payments_id_type_fkey");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.IdMethod).HasName("payment_methods_pkey");

            entity.ToTable("payment_methods");

            entity.HasIndex(e => e.Name, "payment_methods_name_key").IsUnique();

            entity.Property(e => e.IdMethod).HasColumnName("id_method");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("payment_statuses_pkey");

            entity.ToTable("payment_statuses");

            entity.HasIndex(e => e.Name, "payment_statuses_name_key").IsUnique();

            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "roles_role_name_key").IsUnique();

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");

            entity.HasMany(d => d.IdModules).WithMany(p => p.IdRoles)
                .UsingEntity<Dictionary<string, object>>(
                    "RoleModule",
                    r => r.HasOne<Module>().WithMany()
                        .HasForeignKey("IdModule")
                        .HasConstraintName("role_modules_id_module_fkey"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("IdRole")
                        .HasConstraintName("role_modules_id_role_fkey"),
                    j =>
                    {
                        j.HasKey("IdRole", "IdModule").HasName("role_modules_pkey");
                        j.ToTable("role_modules");
                        j.IndexerProperty<int>("IdRole").HasColumnName("id_role");
                        j.IndexerProperty<int>("IdModule").HasColumnName("id_module");
                    });
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.IdRoom).HasName("rooms_pkey");

            entity.ToTable("rooms");

            entity.HasIndex(e => e.RoomNumber, "rooms_room_number_key").IsUnique();

            entity.Property(e => e.IdRoom).HasColumnName("id_room");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdRoomType).HasColumnName("id_room_type");
            entity.Property(e => e.IdStatus)
                .HasDefaultValue(1)
                .HasColumnName("id_status");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(20)
                .HasColumnName("room_number");

            entity.HasOne(d => d.IdRoomTypeNavigation).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.IdRoomType)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("rooms_id_room_type_fkey");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("rooms_status_fkey");

            entity.HasMany(d => d.IdServices).WithMany(p => p.IdRooms)
                .UsingEntity<Dictionary<string, object>>(
                    "RoomService",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("IdService")
                        .HasConstraintName("room_services_id_service_fkey"),
                    l => l.HasOne<Room>().WithMany()
                        .HasForeignKey("IdRoom")
                        .HasConstraintName("room_services_id_room_fkey"),
                    j =>
                    {
                        j.HasKey("IdRoom", "IdService").HasName("room_services_pkey");
                        j.ToTable("room_services");
                        j.IndexerProperty<int>("IdRoom").HasColumnName("id_room");
                        j.IndexerProperty<int>("IdService").HasColumnName("id_service");
                    });
        });

        modelBuilder.Entity<RoomImage>(entity =>
        {
            entity.HasKey(e => e.IdImage).HasName("room_images_pkey");

            entity.ToTable("room_images");

            entity.Property(e => e.IdImage).HasColumnName("id_image");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdRoom).HasColumnName("id_room");
            entity.Property(e => e.Url).HasColumnName("url");

            entity.HasOne(d => d.IdRoomNavigation).WithMany(p => p.RoomImages)
                .HasForeignKey(d => d.IdRoom)
                .HasConstraintName("room_images_id_room_fkey");
        });

        modelBuilder.Entity<RoomStatus>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("room_statuses_pkey");

            entity.ToTable("room_statuses");

            entity.HasIndex(e => e.Name, "room_statuses_name_key").IsUnique();

            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.IdRoomType).HasName("room_types_pkey");

            entity.ToTable("room_types");

            entity.HasIndex(e => e.Name, "room_types_name_key").IsUnique();

            entity.Property(e => e.IdRoomType).HasColumnName("id_room_type");
            entity.Property(e => e.BasePrice)
                .HasPrecision(10, 2)
                .HasColumnName("base_price");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.GuestCapacity).HasColumnName("guest_capacity");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.HasKey(e => e.IdSeason).HasName("seasons_pkey");

            entity.ToTable("seasons");

            entity.Property(e => e.IdSeason).HasColumnName("id_season");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.PriceMultiplier)
                .HasPrecision(4, 2)
                .HasDefaultValue(1.00m)
                .HasColumnName("price_multiplier");
            entity.Property(e => e.SeasonName)
                .HasMaxLength(100)
                .HasColumnName("season_name");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("services_pkey");

            entity.ToTable("services");

            entity.HasIndex(e => e.Name, "services_name_key").IsUnique();

            entity.Property(e => e.IdService).HasColumnName("id_service");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.IdUser)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_user");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_id_role_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
