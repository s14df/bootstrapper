﻿using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Bootstrapper.UnityExtension;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bootstrapper.Tests.Extensions.Containers.Unity
{
    [TestClass]
    public class StartupTasksRegistrationTests
    {
        [TestMethod]
        public void ShouldCreateAStartupTasksRegistration()
        {
            //Act
            var result = new StartupTaskRegistration();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StartupTaskRegistration));
        }

        [TestMethod]
        public void ShouldRegisterStartupTaskFromAssembly()
        {
            //Arrange
            var container = new UnityContainer();
            var containerExtension = new Mock<IBootstrapperContainerExtension>();
            var collector = new Mock<IAssemblyCollector>();
            collector.Setup(c => c.Assemblies).Returns(new List<Assembly> { Assembly.GetExecutingAssembly() });
            collector.Setup(c => c.AssemblyNames).Returns(new List<string>());
            containerExtension.Setup(c => c.LookForStartupTasks).Returns(collector.Object);
            Bootstrapper.With.Container(containerExtension.Object);

            //Act
            new StartupTaskRegistration().Register(container);
            var result = container.ResolveAll<IStartupTask>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IStartupTask>));
            Assert.IsTrue(result.Any(m => m.GetType() == typeof(TestStartupTask)));
        }


        [TestMethod]
        public void ShouldRegisterStartupTaskFromAssemblyName()
        {
            //Arrange
            var container = new UnityContainer();
            var containerExtension = new Mock<IBootstrapperContainerExtension>();
            var collector = new Mock<IAssemblyCollector>();
            collector.Setup(c => c.Assemblies).Returns(new List<Assembly>());
            collector.Setup(c => c.AssemblyNames).Returns(new List<string> { Assembly.GetExecutingAssembly().FullName });
            containerExtension.Setup(c => c.LookForStartupTasks).Returns(collector.Object);
            Bootstrapper.With.Container(containerExtension.Object);

            //Act
            new StartupTaskRegistration().Register(container);
            var result = container.ResolveAll<IStartupTask>();
            Bootstrapper.ClearExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<IStartupTask>));
            Assert.IsTrue(result.Any(m => m.GetType() == typeof(TestStartupTask)));
        }
    }
}
