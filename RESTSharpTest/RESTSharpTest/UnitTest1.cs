using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Nodes;

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
        // Ability to add multiple contacts to the address book JSON server 
        [TestMethod]
        public void OnCallingPostAPI_ForAContactListWithMultipleContacts_ReturnContactObject()
        {
            // Arrange
            List<Contact> contactList = new List<Contact>();
            contactList.Add(new Contact { FirstName = "Stephen", LastName = "Strange", PhoneNo = "6300964579", Address = "Bleecker Street", City = "Manhattan", State = "NewYork", Zip = "10431", Email = "drStrange@yahoo.com" });
            contactList.Add(new Contact { FirstName = "Thor", LastName = "Odinson", PhoneNo = "6777456345", Address = "RoyalPalace", City = "Asgard", State = "Asgard", Zip = "22400", Email = "thor@rediffmail.com" });
            contactList.Add(new Contact { FirstName = "Clint", LastName = "Barton", PhoneNo = "7654564345", Address = "Broadway Street", City = "NewYork City", State = "NewYork", Zip = "14422", Email = "hawkeye@rediffmail.com" });

            //Iterate the loop for each contact
            foreach (var contact in contactList)
            {
                //Initialize the request for POST to add new contact
                RestRequest request = new RestRequest("/contacts/list", Method.POST);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("firstname", contact.FirstName);
                jsonObj.Add("lastname", contact.LastName);
                jsonObj.Add("phoneNo", contact.PhoneNo);
                jsonObj.Add("address", contact.Address);
                jsonObj.Add("city", contact.City);
                jsonObj.Add("state", contact.State);
                jsonObj.Add("zip", contact.Zip);
                jsonObj.Add("email", contact.Email);

                //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Contact addedContact = JsonConvert.DeserializeObject<Contact>(response.Content);
                Assert.AreEqual(addedContact.FirstName, contact.FirstName);
                Assert.AreEqual(addedContact.LastName, contact.LastName);
                Assert.AreEqual(addedContact.PhoneNo, contact.PhoneNo);
                Console.WriteLine(response.Content);
            }
        }
        // Ability to update the phoneNo, zipCode into the json server
        [TestMethod]
        public void OnCallingPutAPI_ReturnContactObjects()
        {
            // Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/contacts/6", Method.PUT);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("firstname", "Thor");
            jsonObj.Add("lastname", "Odinson");
            jsonObj.Add("phoneNo", "7858070934");
            jsonObj.Add("address", "RoyalPalace");
            jsonObj.Add("city", "Asgard");
            jsonObj.Add("state", "Asgard");
            jsonObj.Add("zip", "535678");
            jsonObj.Add("email", "thor@rediffmail.com");
            //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
            request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Contact contact = JsonConvert.DeserializeObject<Contact>(response.Content);
            Assert.AreEqual("Thor", contact.FirstName);
            Assert.AreEqual("Odinson", contact.LastName);
            Assert.AreEqual("7858070934", contact.PhoneNo);
            Assert.AreEqual("535678", contact.Zip);
            Console.WriteLine(response.Content);
        }
        // Ability to delete the contact details with given id
        [TestMethod]
        public void OnCallingDeleteAPI_ReturnSuccessStatus()
        {
            //Arrange
            //Initialize the request for PUT to add new employee
            RestRequest request = new RestRequest("/contacts/5", Method.DELETE);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}