using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using RESTSharpTest;
using System;
using System.Collections.Generic;
using System.Net;

namespace RESTSharpTest
{
    public class Contact
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
    }
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            //Initialize the base URL to execute requests made by the instance
            client = new RestClient("http://localhost:4000");
        }
        private IRestResponse GetContactList()
        {
            //Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("/contacts/list", Method.GET);

            // Execute the request
            RestSharp.IRestResponse response = client.Execute(request);
            return response;
        }

        // Ability to read the entries from json server.
        [TestMethod]
        public void ReadEntriesFromJsonServer()
        {
            IRestResponse response = GetContactList();

            // Check if the status code of response equals the default code for the method requested
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Convert the response object to list of employees
            List<Contact> employeeList = JsonConvert.DeserializeObject<List<Contact>>(response.Content);
            Assert.AreEqual(4, employeeList.Count);

            // Print all contacts from json server
            Console.Write("\nID".PadRight(4) + "FirstName".PadRight(12) + "LastName".PadRight(12) + "Address".PadRight(20));
            Console.Write("City".PadRight(18) + "State".PadRight(12) + "Zip".PadRight(10) + "Phone No.".PadRight(15) + "Email".PadRight(12));
            Console.Write("\n");
            foreach (Contact contact in employeeList)
            {
                Console.Write(contact.ID.ToString().PadRight(4) + contact.FirstName.PadRight(12) + contact.LastName.PadRight(12));
                Console.Write(contact.Address.PadRight(20) + contact.City.PadRight(18) + contact.State.PadRight(12));
                Console.Write(contact.Zip.PadRight(10) + contact.PhoneNo.PadRight(15) + contact.Email.PadRight(12));
                Console.Write("\n");
            }
        }
    }
}