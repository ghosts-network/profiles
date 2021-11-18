﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GhostNetwork.Profiles.Friends;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace GhostNetwork.Profile.ApiTests.Friends
{
    [TestFixture]
    public class GetTests
    {
        [Test]
        public async Task SearchFriends_Ok()
        {
            //Setup
            var user = Guid.NewGuid();

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.SearchFriendsAsync(0, 10, user))
                .ReturnsAsync((new List<Guid>(), 10));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.GetAsync($"/Relations/{user}/friends");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Test]
        public async Task SearchFollowers_Ok()
        {
            //Setup
            var user = Guid.NewGuid();

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.SearchFollowersAsync(0, 10, user))
                .ReturnsAsync((new List<Guid>(), 10));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.GetAsync($"/Relations/{user}/followers");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Test]
        public async Task SearchIncomingRequests_Ok()
        {
            //Setup
            var user = Guid.NewGuid();

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.SearchIncomingRequestsAsync(0, 10, user))
                .ReturnsAsync((new List<Guid>(), 10));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.GetAsync($"/Relations/{user}/friends/incoming-requests");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Test]
        public async Task SearchOutgoingRequests_Ok()
        {
            //Setup
            var user = Guid.NewGuid();

            var serviceMock = new Mock<IRelationsService>();

            serviceMock
                .Setup(x => x.SearchIncomingRequestsAsync(0, 10, user))
                .ReturnsAsync((new List<Guid>(), 10));

            var client = TestServerHelper.New(collection =>
            {
                collection.AddScoped(_ => serviceMock.Object);
            });
            
            //Act
            var response = await client.GetAsync($"/Relations/{user}/friends/outgoing-requests");
            
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}