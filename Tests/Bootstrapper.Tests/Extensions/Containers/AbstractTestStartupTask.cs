﻿using System;

namespace Bootstrapper.Tests.Extensions.Containers
{
    public abstract class AbstractTestStartupTask: IStartupTask
    {
        public void Run() {}
        public void Reset() {}
    }
}