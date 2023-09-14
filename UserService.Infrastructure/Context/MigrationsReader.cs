using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Context;

public class MigrationsReader
{
    private const string MigrationsFile = "Migrations.xml";
    
    private readonly ILogger<MigrationsReader> _logger;

    public MigrationsReader(ILogger<MigrationsReader> logger)
    {
        _logger = logger;
    }


    public Task<MigrationDefinition[]?> ReadMigrationsDefinitions()
    {
        MigrationDefinition[]? migrationDefinitions = default;
        XmlSerializer serializer = new XmlSerializer(typeof(MigrationDefinition[]));
        try
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), MigrationsFile);
            using var migrationsFile = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var result = serializer.Deserialize(migrationsFile);
            if (result != null)
                migrationDefinitions = (MigrationDefinition[])result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading {File}: {Error}", MigrationsFile, e.Message);
        }

        return Task.FromResult(migrationDefinitions);
    }
}