﻿using System;
using Moq;
using NUnit.Framework;
using NzbDrone.Common.Disk;
using NzbDrone.Common.EnvironmentInfo;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.HealthCheck.Checks;
using NzbDrone.Core.Test.Framework;

namespace NzbDrone.Core.Test.HealthCheck.Checks
{
    [TestFixture]
    public class UpdateCheckFixture : CoreTest<UpdateCheck>
    {
        [Test]
        public void should_return_error_when_app_folder_is_write_protected()
        {
            WindowsOnly();

            Mocker.GetMock<IAppFolderInfo>()
                  .Setup(s => s.StartUpFolder)
                  .Returns(@"C:\NzbDrone");

            Mocker.GetMock<NzbDrone.Common.Disk.IDiskProvider>()
                  .Setup(c => c.FolderWritable(Moq.It.IsAny<string>()))
                  .Returns(false);

            Subject.Check().ShouldBeError();
        }

        [Test]
        public void should_return_error_when_app_folder_is_write_protected_and_update_automatically_is_enabled()
        {
            MonoOnly();

            Mocker.GetMock<IConfigFileProvider>()
                  .Setup(s => s.UpdateAutomatically)
                  .Returns(true);

            Mocker.GetMock<IAppFolderInfo>()
                  .Setup(s => s.StartUpFolder)
                  .Returns(@"/opt/nzbdrone");

            Mocker.GetMock<NzbDrone.Common.Disk.IDiskProvider>()
                  .Setup(c => c.FolderWritable(Moq.It.IsAny<string>()))
                  .Returns(false);

            Subject.Check().ShouldBeError();
        }
    }
}
