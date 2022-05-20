using Newtonsoft.Json;

namespace HTTPClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HTTPCalls call = new HTTPCalls();

            string option;

            Console.WriteLine("Dati http call-ul:");
            Console.WriteLine("1 - get categories");
            Console.WriteLine("2 - get specific category");
            Console.WriteLine("3 - post one category");
            Console.WriteLine("4 - delete one category");
            Console.WriteLine("5 - put category title");
            Console.WriteLine("6 - post products in category");
            Console.WriteLine("7 - get products from category");
            Console.WriteLine("quit - exit\n");

            while (true)
            {
                Thread.Sleep(2000);
                Console.WriteLine("\n---------------------------------------------------");
                Console.Write("Choice: ");
                option = Console.ReadLine();

                if (option == "1")
                {
                    call.GetCategories();
                }
                if (option == "2")
                {
                    Console.WriteLine("1 - id");
                    Console.WriteLine("2 - name");
                    Console.Write("Introduce the choice (id/name): ");

                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        int categoryId = GetId();
                        call.GetCategory(categoryId);
                    }
                    if (choice == "2")
                    {
                        string categoryName = GetName();
                        call.GetCategory(categoryName);
                    }
                }
                if (option == "3")
                {
                    Console.Write("Introduce title of category: ");
                    string title = Console.ReadLine();
                    call.PostCategory(title);
                }
                if (option == "4")
                {
                    int categoryId = GetId();
                    call.DeleteCategory(categoryId);
                }
                if (option == "5")
                {
                    int id = GetId();
                    string name = GetName();
                    call.PutCategoryTitle(id, name);
                }
                if (option == "6")
                {
                    int categoryid = GetId();
                    string title = GetName();
                    Console.Write("Introduce the price: ");
                    double price = Convert.ToDouble(Console.ReadLine());
                    call.PostProducts(title, price, categoryid);
                }
                if (option == "7")
                {
                    int id = GetId();
                    call.GetProductsFromCategory(id);
                }
                if (option == "quit")
                {
                    break;
                }
            }
        }

        //Helper method
        public static int GetId()
        {
            Console.Write("Introduce the id: ");

            string id = Console.ReadLine();

            return int.Parse(id);
        }

        //Helper method
        public static string GetName()
        {
            Console.Write("Introduce the name: ");

            return Console.ReadLine();
        }
    }
}

