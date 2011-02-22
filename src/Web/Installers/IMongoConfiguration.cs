using System;

namespace Web.Installers
{
    public interface IMongoConfiguration
    {
        Uri ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}