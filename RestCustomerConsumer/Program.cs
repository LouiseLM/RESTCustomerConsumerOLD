using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestCustomerConsumer
{
    class Program
    {
        /// <summary>
        /// Pointing to Customer Service og metode
        /// </summary>
        private static string CustomersUri = "https://localhost:44334/api/customer/";
        // private static string CustomersUri = "https://restcustomerlm.azurewebsites.net/api/customer/";
        static void Main(string[] args)
        {
            IList<Customer> allC = GetCustomersAsync().Result;
            //Console.WriteLine(string.Join("\n", allC.ToString()));
            for (int i = 0; i < allC.Count; i++)
            Console.WriteLine(allC[i].ToString());

            Console.WriteLine("-------------------");

            Console.WriteLine("GET: Give ID for specific customer to be printed");
            Console.WriteLine("ID: ");

            String idGet = Console.ReadLine();
            int id = Int32.Parse(idGet);
            Customer c = GetCustomerInfoAsync(id).Result;
            Console.WriteLine(c.ToString());

            Console.WriteLine("-------------------");

            Console.WriteLine("POST: Give data for new customer to be inserted");
            Console.WriteLine("First Name: ");
            String first = Console.ReadLine();
            Console.WriteLine("Last Name: ");
            String last = Console.ReadLine();
            Console.WriteLine("Year: ");
            String yearStr = Console.ReadLine();
            int year = Int32.Parse(yearStr);

            Customer newCustomer = new Customer(first, last, year);
            Customer customer = PostNewCustomerAsync(newCustomer).Result;
            Console.WriteLine("Customer inserted");
            Console.WriteLine(customer.ToString());

            Console.WriteLine("-------------------");

            Console.WriteLine("PUT: Give ID for customer to be updated");
            String idU = Console.ReadLine();
            int id2 = int.Parse(idU);
            Customer cu = GetCustomerInfoAsync(id2).Result;
            Console.WriteLine("This customer will be updated: " + cu.ToString());

            Console.WriteLine("PUT: Give new data for customer to be updated");
            Console.WriteLine("First Name: ");
            cu.FirstName = Console.ReadLine();
            Console.WriteLine("Last Name: ");
            cu.LastName = Console.ReadLine();
            Console.WriteLine("Year: ");
            String yearStrU = Console.ReadLine();
            cu.YearOfReg = Int32.Parse(yearStrU);

            Customer cu2 = UpdateCustomerAsync(cu, id2).Result;
            Console.WriteLine("Customer updated");
            Console.WriteLine(cu2.ToString());

            Console.WriteLine("-------------------");

            Console.WriteLine("DELETE: Give ID for customer to be deleted");
            Console.WriteLine("ID: ");

            String idDel = Console.ReadLine();
            int id1 = int.Parse(idDel);
            Customer x = DeleteOneCustomerAsync(id1).Result;
            Customer x1 = GetCustomerInfoAsync(id1).Result;
            if (x1 == null) Console.WriteLine("Customer id: " + id1 + " not found. Deletion successful");

            Console.WriteLine("-------------------");

            Console.ReadKey();
        }

        public static async Task<IList<Customer>> GetCustomersAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(CustomersUri);
                IList<Customer> cList = JsonConvert.DeserializeObject<IList<Customer>>(content);
                return cList;
            }
        }
        
        public static async Task<Customer> GetCustomerInfoAsync(int id)
        {
            string CusomerIdUri = CustomersUri + "/" + id;
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(CusomerIdUri);
                Customer c1 = JsonConvert.DeserializeObject<Customer>(content);
                return c1;
            }
        }

        public static async Task<Customer> PostNewCustomerAsync(Customer newCust)
        {
            using (HttpClient client = new HttpClient())
            {
                var jsonString = JsonConvert.SerializeObject(newCust);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(CustomersUri, content);
                
                string message = await response.Content.ReadAsStringAsync();
                Customer newCustPrint = JsonConvert.DeserializeObject<Customer>(message);
                return newCustPrint;
            }
        }

        public static async Task<Customer> DeleteOneCustomerAsync(int id)
        {
            string requestUri = CustomersUri + id;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(requestUri);
                //Console.WriteLine("Statuscode" + response.StatusCode);
                string print = await response.Content.ReadAsStringAsync();
                Customer deletedCustomer = JsonConvert.DeserializeObject<Customer>(print);
                return deletedCustomer;
            }
        }

        public static async Task<Customer> UpdateCustomerAsync(Customer uCust, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string requestUri = CustomersUri + id;
                var jsonString = JsonConvert.SerializeObject(uCust);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(requestUri, content);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("Customer not found. Try another id");
                }
                string str = await response.Content.ReadAsStringAsync();
                Customer updCustomer = JsonConvert.DeserializeObject<Customer>(str);
                return updCustomer;
            }
        }
    }
}
