using Microsoft.EntityFrameworkCore;
using DdzServer.Models;

namespace DdzServer.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Room> Rooms { get; set; }
        // public DbSet<GameRecord> GameRecords { get; set; } // 可扩展战绩表

        // 可根据需要配置表结构和关系
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 配置主键、索引、关系等
        }
    }
} 