using API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Transactions;

namespace TestAPI
{
    [TestClass]
    public class DtoTests
    {
        [TestMethod]
        public void AddUserConstructorDTOTest()
        {
            var Username = "TestUser";
            var GroupIds = new List<long>() { 1, 2, 3, 4, 5 };

            var testDto = new API.DTO.AddUserRequest(Username, GroupIds);

            Assert.AreEqual(Username, testDto.Name);
            Assert.AreEqual(GroupIds, testDto.GroupIds);
        }

        [TestMethod]
        public void AddUserValidateDTOTest()
        {
            string Username = "";
            var GroupIds = new List<long>() { 1, 2, 3, 4, 5 };

            var testDto = new API.DTO.AddUserRequest(Username, GroupIds);

            Assert.IsFalse(testDto.IsValid);
            Assert.AreEqual("Name cannot be null or empty", testDto.ValidationMessage);
        }
    }
}
