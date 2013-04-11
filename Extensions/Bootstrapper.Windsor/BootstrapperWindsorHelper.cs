﻿using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using Castle.Core.Internal;
using Castle.MicroKernel;

namespace Bootstrap.Windsor
{
    public static class BootstrapperWindsorHelper
    {
        public static WindsorOptions Windsor(this BootstrapperExtensions extensions, params IFacility[] facilities)
        {
            var extension = new WindsorExtension(new RegistrationHelper(), new BootstrapperContainerExtensionOptions());
            facilities.ForEach(extension.AddFacility);
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}
