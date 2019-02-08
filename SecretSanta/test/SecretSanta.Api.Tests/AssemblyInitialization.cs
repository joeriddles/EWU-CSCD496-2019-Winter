using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    public class AssemblyInitialization
    {
        [AssemblyInitialize]
        public static void ConfigureAutoMapper(TestContext context)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new MappingProfile()));
        }
    }
}
