﻿using GhostNetwork.Profiles.Friends;
using GhostNetwork.Profiles.SecuritySettings;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GhostNetwork.Profiles.Tests
{
    [TestFixture]
    public class AccessResolverTests
    {
        [Test]
        public async Task ResolveAccess_UserId_Equal_ToUserId_Ok()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = userId;


            var section = new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>());

            var securityStorageMock = new Mock<ISecuritySettingStorage>();
            var relationServiceMock = new Mock<IRelationsService>();

            securityStorageMock.Setup(s => s.FindSectionByUserIdAsync(toUserId, sectionName))
                .ReturnsAsync(section);


            var accessResolver = new AccessResolver(securityStorageMock.Object, relationServiceMock.Object);

            // Act
            var result = await accessResolver.ResolveAccessAsync(userId, toUserId, sectionName);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ResolveAccess_Everyone_Error()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var section = new SecuritySettingsSection(Access.Everyone, Enumerable.Empty<Guid>());

            var securityStorageMock = new Mock<ISecuritySettingStorage>();
            var relationServiceMock = new Mock<IRelationsService>();

            securityStorageMock.Setup(s => s.FindSectionByUserIdAsync(toUserId, sectionName))
                .ReturnsAsync(section);

            var accessResolver = new AccessResolver(securityStorageMock.Object, relationServiceMock.Object);

            // Act
            var result = await accessResolver.ResolveAccessAsync(userId, toUserId, sectionName);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ResolveAccess_NoOne_Error()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var section = new SecuritySettingsSection(Access.NoOne, Enumerable.Empty<Guid>());

            var securityStorageMock = new Mock<ISecuritySettingStorage>();
            var relationServiceMock = new Mock<IRelationsService>();

            securityStorageMock.Setup(s => s.FindSectionByUserIdAsync(toUserId, sectionName))
                .ReturnsAsync(section);

            var accessResolver = new AccessResolver(securityStorageMock.Object, relationServiceMock.Object);

            // Act
            var result = await accessResolver.ResolveAccessAsync(userId, toUserId, sectionName);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ResolveAccess_OnlyFriends_Ok()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var section = new SecuritySettingsSection(Access.OnlyFriends, Enumerable.Empty<Guid>());

            var securityStorageMock = new Mock<ISecuritySettingStorage>();
            securityStorageMock.Setup(s => s.FindSectionByUserIdAsync(toUserId, sectionName))
                .ReturnsAsync(section);

            var relationServiceMock = new Mock<IRelationsService>();
            relationServiceMock.Setup(s => s.IsFriendAsync(userId, toUserId))
                .ReturnsAsync(true);

            var accessResolver = new AccessResolver(securityStorageMock.Object, relationServiceMock.Object);

            // Act
            var result = await accessResolver.ResolveAccessAsync(userId, toUserId, sectionName);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ResolveAccess_OnlyFriends_Error()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var section = new SecuritySettingsSection(Access.OnlyFriends, Enumerable.Empty<Guid>());

            var securityStorageMock = new Mock<ISecuritySettingStorage>();
            securityStorageMock.Setup(s => s.FindSectionByUserIdAsync(toUserId, sectionName))
                .ReturnsAsync(section);

            var relationServiceMock = new Mock<IRelationsService>();
            relationServiceMock.Setup(s => s.IsFriendAsync(userId, toUserId))
                .ReturnsAsync(false);

            var accessResolver = new AccessResolver(securityStorageMock.Object, relationServiceMock.Object);

            // Act
            var result = await accessResolver.ResolveAccessAsync(userId, toUserId, sectionName);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ResolveAccess_OnlyCertainUsers_Ok()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var section = new SecuritySettingsSection(Access.OnlyCertainUsers, new List<Guid> { userId });

            var securityStorageMock = new Mock<ISecuritySettingStorage>();

            securityStorageMock.Setup(s => s.FindSectionByUserIdAsync(toUserId, sectionName))
                .ReturnsAsync(section);
            securityStorageMock.Setup(s => s.ContainsInCertainUsersAsync(userId, toUserId, sectionName))
                .ReturnsAsync(true);

            var relationServiceMock = new Mock<IRelationsService>();
            relationServiceMock.Setup(s => s.IsFriendAsync(userId, toUserId))
                .ReturnsAsync(false);

            var accessResolver = new AccessResolver(securityStorageMock.Object, relationServiceMock.Object);

            // Act
            var result = await accessResolver.ResolveAccessAsync(userId, toUserId, sectionName);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ResolveAccess_OnlyCertainUsers_Error()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var section = new SecuritySettingsSection(Access.OnlyCertainUsers, new List<Guid> { userId });

            var securityStorageMock = new Mock<ISecuritySettingStorage>();

            securityStorageMock.Setup(s => s.FindSectionByUserIdAsync(toUserId, sectionName))
                .ReturnsAsync(section);
            securityStorageMock.Setup(s => s.ContainsInCertainUsersAsync(userId, toUserId, sectionName))
                .ReturnsAsync(false);

            var relationServiceMock = new Mock<IRelationsService>();
            relationServiceMock.Setup(s => s.IsFriendAsync(userId, toUserId))
                .ReturnsAsync(false);

            var accessResolver = new AccessResolver(securityStorageMock.Object, relationServiceMock.Object);

            // Act
            var result = await accessResolver.ResolveAccessAsync(userId, toUserId, sectionName);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ResolveAccess_EveryoneExceptCertainUsers_Ok()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var section = new SecuritySettingsSection(Access.EveryoneExceptCertainUsers, Enumerable.Empty<Guid>());

            var securityStorageMock = new Mock<ISecuritySettingStorage>();

            securityStorageMock.Setup(s => s.FindSectionByUserIdAsync(toUserId, sectionName))
                .ReturnsAsync(section);
            securityStorageMock.Setup(s => s.ContainsInCertainUsersAsync(userId, toUserId, sectionName))
                .ReturnsAsync(false);

            var relationServiceMock = new Mock<IRelationsService>();
            relationServiceMock.Setup(s => s.IsFriendAsync(userId, toUserId))
                .ReturnsAsync(false);

            var accessResolver = new AccessResolver(securityStorageMock.Object, relationServiceMock.Object);

            // Act
            var result = await accessResolver.ResolveAccessAsync(userId, toUserId, sectionName);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ResolveAccess_EveryoneExceptCertainUsers_Error()
        {
            // Setup
            const string sectionName = "friends";

            var userId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var section = new SecuritySettingsSection(Access.EveryoneExceptCertainUsers, new List<Guid> { userId });

            var securityStorageMock = new Mock<ISecuritySettingStorage>();

            securityStorageMock.Setup(s => s.FindSectionByUserIdAsync(toUserId, sectionName))
                .ReturnsAsync(section);
            securityStorageMock.Setup(s => s.ContainsInCertainUsersAsync(userId, toUserId, sectionName))
                .ReturnsAsync(true);

            var relationServiceMock = new Mock<IRelationsService>();
            relationServiceMock.Setup(s => s.IsFriendAsync(userId, toUserId))
                .ReturnsAsync(false);

            var accessResolver = new AccessResolver(securityStorageMock.Object, relationServiceMock.Object);

            // Act
            var result = await accessResolver.ResolveAccessAsync(userId, toUserId, sectionName);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
