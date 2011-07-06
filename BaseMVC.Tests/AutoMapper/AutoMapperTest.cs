using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using NUnit.Framework;
using Rhino.Mocks;
using Castle.Windsor;

namespace BaseMVC.Tests.AutoMapper
{
    [TestFixture]
    public class AutoMapperTest
    {
        [Test]
        public void ShouldHaveCorrectMappings()
        {
            BaseMVC.AutoMapper.AutoMapper.Configure(MockRepository.GenerateStub<IWindsorContainer>());
            Mapper.AssertConfigurationIsValid();
        }
    }
}
