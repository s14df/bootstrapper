﻿using System;
using System.Collections.Generic;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Bootstrap.Tests.Other;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions
{
    [TestClass]
    public class BootstrapperExtensionsTests
    {
        [TestMethod]
        public void ShouldCreateABoostrapperExtensions()
        {
            //Act
            var result = new BootstrapperExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BootstrapperExtensions));
        }

        [TestMethod]
        public void ShouldReturnTheSameInstance()
        {
            //Arrange
            var extensions = new BootstrapperExtensions();

            //Act
            var result = extensions.And;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(BootstrapperExtensions));
            Assert.AreSame(extensions,result);
        }

        [TestMethod]
        public void ShouldHaveNoExtensions()
        {
            //Arrange
            var extensions = new BootstrapperExtensions();
            extensions.Extension(A.Fake<IBootstrapperExtension>());

            //Act
            extensions.ClearExtensions();
            var result = extensions.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(IList<IBootstrapperExtension>));
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldAddAnExtension()
        {
            //Arrange
            var extension = A.Fake<IBootstrapperExtension>();
            var extensions = new BootstrapperExtensions();

            //Act
            extensions.Extension(extension);
            var result = extensions.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(IList<IBootstrapperExtension>));
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(extension, result[0]);
        }

        [TestMethod]
        public void ShouldSetTheContainer()
        {
            //Arrange
            var containerExtension = A.Fake<IBootstrapperContainerExtension>();
            var extensions = new BootstrapperExtensions();

            //Act
            extensions.Extension(containerExtension);
            var result = Bootstrapper.ContainerExtension;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IBootstrapperContainerExtension));
            Assert.AreSame(containerExtension, result);
        }

        [TestMethod]
        public void ShouldReturnAReadOnlyListOfBoostraperExtensions()
        {
            //Arrange
            var extension = A.Fake<IBootstrapperExtension>();
            var newExtension = A.Fake<IBootstrapperExtension>();
            var extensions = new BootstrapperExtensions();
            extensions.Extension(extension);

            //Act
            var result = extensions.GetExtensions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<IBootstrapperExtension>));
            Assert.AreEqual(1, result.Count);
            ExceptionAssert.Throws<NotSupportedException>(() => result.Add(newExtension));
        }

        [TestMethod]
        public void ShouldInvokeTheStartMethodOftheBootStrapper()
        {
            //Arrange
            var extension = A.Fake<IBootstrapperExtension>();
            Bootstrapper.With.Extension(extension);

            //Act
            Bootstrapper.With.Start();
            Bootstrapper.ClearExtensions();

            //Assert
            A.CallTo(() => extension.Run()).MustHaveHappened();
        }

    }
}
