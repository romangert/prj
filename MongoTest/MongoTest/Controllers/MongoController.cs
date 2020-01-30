using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoTest.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class MongoController : ControllerBase
    {
        [HttpGet]
        public bool Update(int id, string value)
        {
            IMongoCollection<BsonDocument> things = GetDB();

            //UPDATE  
            BsonElement updatePersonFirstNameElement = new BsonElement("PersonFirstName", "Souvik");

            BsonDocument updatePersonDoc = new BsonDocument();
            updatePersonDoc.Add(updatePersonFirstNameElement);
            updatePersonDoc.Add(new BsonElement("PersonAge", 24));

            BsonDocument findPersonDoc = new BsonDocument(new BsonElement("PersonFirstName", "Sankhojjal"));

            var updateDoc = things.FindOneAndReplace(findPersonDoc, updatePersonDoc);

            Console.WriteLine(updateDoc);

            return true;
        }

        [HttpGet]
        public void Create(string firstName, int age)
        {
            //Create 

            //retrieve the current collection first.
            IMongoCollection<BsonDocument> things = GetDB();

            //Create a BsonDocument object where we want to store our data.             
            //I showed how to explicitly create a BsonElement object variable to store key-value pair and then add it to the BsonDocument object.
            BsonElement personFirstNameElement = new BsonElement("PersonFirstName", firstName);
            BsonDocument personDoc = new BsonDocument();

            // I did not create a BsonElement object variable, rather I directly passed the object as parameter. The last statement inserts the data in the collection "things".
            personDoc.Add(personFirstNameElement);
            personDoc.Add(new BsonElement("PersonAge", age));

            things.InsertOne(personDoc);
        }

        // DELETE: api/ApiWithActions/5
        [HttpGet]
        public void Delete(int id)
        {
            //DELETE  
            BsonDocument findPersonDoc = new BsonDocument(new BsonElement("PersonFirstName", "Sourav"));
            IMongoCollection<BsonDocument> things = GetDB();
            things.FindOneAndDelete(findPersonDoc);
        }

        // GET: api/Mongo
        [HttpGet]
        public IEnumerable<string> Get()
        {
            IMongoCollection<BsonDocument> things = GetDB();
            //READ  
            var resultDoc = things.Find(new BsonDocument()).ToList();
            List<string> result = new List<string>();
            foreach (var item in resultDoc)
            {
                result.Add(item.ToString());
            }
               
            return result;
        }

        // GET: api/Mongo/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Mongo
        
        [HttpPost]
        public void Post([FromBody] string value)
        {
            //Create 

            //retrieve the current collection first.
            IMongoCollection<BsonDocument> things = GetDB();

            //Create a BsonDocument object where we want to store our data.             
            //I showed how to explicitly create a BsonElement object variable to store key-value pair and then add it to the BsonDocument object.
            BsonElement personFirstNameElement = new BsonElement("PersonFirstName", "Sankhojjal");
            BsonDocument personDoc = new BsonDocument();

            // I did not create a BsonElement object variable, rather I directly passed the object as parameter. The last statement inserts the data in the collection "things".
            personDoc.Add(personFirstNameElement);
            personDoc.Add(new BsonElement("PersonAge", 23));

            things.InsertOne(personDoc);
        }

        // PUT: api/Mongo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            IMongoCollection<BsonDocument> things = GetDB();

            //UPDATE  
            BsonElement updatePersonFirstNameElement = new BsonElement("PersonFirstName", "Souvik");

            BsonDocument updatePersonDoc = new BsonDocument();
            updatePersonDoc.Add(updatePersonFirstNameElement);
            updatePersonDoc.Add(new BsonElement("PersonAge", 24));

            BsonDocument findPersonDoc = new BsonDocument(new BsonElement("PersonFirstName", "Sankhojjal"));

            var updateDoc = things.FindOneAndReplace(findPersonDoc, updatePersonDoc);

            Console.WriteLine(updateDoc);
        }


        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    //DELETE  
        //    BsonDocument findPersonDoc = new BsonDocument(new BsonElement("PersonFirstName", "Sourav"));
        //    IMongoCollection<BsonDocument> things = GetDB();            
        //    things.FindOneAndDelete(findPersonDoc);
        //}

        IMongoCollection<BsonDocument> GetDB()
        {
            MongoClient dbClient = new MongoClient("mongodb://127.0.0.1:27017");

            //Database List  
            //var dbList = dbClient.ListDatabases().ToList();
            //Console.WriteLine("The list of databases are :");
            //List<string> lstDB = new List<string>();
            //foreach (var item in dbList)
            //{
            //    lstDB.Add(item.ToString());
            //}

            //Get Database and Collection  
            IMongoDatabase db = dbClient.GetDatabase("test");

            //var collList = db.ListCollections().ToList();
            //List<string> lstCol = new List<string>();
            //Console.WriteLine("The list of collections are :");
            //foreach (var item in collList)
            //{
            //    lstCol.Add(item.ToString());
            //}

            //retrieve the current collection first.
            IMongoCollection<BsonDocument> things = db.GetCollection<BsonDocument>("things");

            return things;
        }

        static void FullTest(string[] args)
        {
            try
            {
                MongoClient dbClient = new MongoClient("mongodb://127.0.0.1:27017");

                //Database List  
                var dbList = dbClient.ListDatabases().ToList();

                Console.WriteLine("The list of databases are :");
                foreach (var item in dbList)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine("\n\n");

                //Get Database and Collection  
                IMongoDatabase db = dbClient.GetDatabase("test");
                var collList = db.ListCollections().ToList();
                Console.WriteLine("The list of collections are :");
                foreach (var item in collList)
                {
                    Console.WriteLine(item);
                }

                var things = db.GetCollection<BsonDocument>("things");

                //CREATE  
                BsonElement personFirstNameElement = new BsonElement("PersonFirstName", "Sankhojjal");

                BsonDocument personDoc = new BsonDocument();
                personDoc.Add(personFirstNameElement);
                personDoc.Add(new BsonElement("PersonAge", 23));

                things.InsertOne(personDoc);

                //UPDATE  
                BsonElement updatePersonFirstNameElement = new BsonElement("PersonFirstName", "Souvik");

                BsonDocument updatePersonDoc = new BsonDocument();
                updatePersonDoc.Add(updatePersonFirstNameElement);
                updatePersonDoc.Add(new BsonElement("PersonAge", 24));

                BsonDocument findPersonDoc = new BsonDocument(new BsonElement("PersonFirstName", "Sankhojjal"));

                var updateDoc = things.FindOneAndReplace(findPersonDoc, updatePersonDoc);

                Console.WriteLine(updateDoc);

                //DELETE  
                BsonDocument findAnotherPersonDoc = new BsonDocument(new BsonElement("PersonFirstName", "Sourav"));

                things.FindOneAndDelete(findAnotherPersonDoc);

                //READ  
                var resultDoc = things.Find(new BsonDocument()).ToList();
                foreach (var item in resultDoc)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    
}
}
