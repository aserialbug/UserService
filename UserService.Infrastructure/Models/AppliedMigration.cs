using System;

namespace UserService.Infrastructure.Models;

public class AppliedMigration
{
    public int Id { get; init; }
    public string Description { get; init; }
    public DateTime AppliedAt { get; init; }
}