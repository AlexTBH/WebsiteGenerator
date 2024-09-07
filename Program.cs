using System.ComponentModel.Design;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Hemsida
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WebSiteGenerator test = new WebSiteGenerator();
            test.DisplayWebsite();
            test.AddTags();
            test.AddTags();
            test.AddTags();
            test.DisplayWebsite();

        }

        class WebSiteGenerator()
        {
            private bool Maintag = false;

            List<string> list = new List<string>
            {
                "<!DOCTYPE html>",
                "<html>",
                "<body>",
                "</body>",
                "</html>",
            };


            // Använd en list istället för att behålla värden
            public void InsertAtIndex(string input)
            {
                if (list.Count > 5)
                {

                    Console.WriteLine("Vart vill du placera taggen?");
                    Console.WriteLine("Du får inte placera det innan första body taggen och efter sista body taggen\n");


                    for (int i = 0; i < list.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {list[i]}");
                    }

                    int val = 0;

                    while (true) {

                        Console.WriteLine($"ange en position mellan 3 till {list.Count - 2}");
                        val = Convert.ToInt32(Console.ReadLine());

                        if (val >= 2 && val <= (list.Count - 2))
                        {
                            break;
                        } 
                        else 
                        {
                            Console.WriteLine("Du har inte angett en position där din text kan placeras\nAnge en ny input");
                        }
                    }

                    list.Insert(val, input);
                } 
                else
                {
                    list.Insert(3, input);
                }

            }

            public void MainTag()
            {
                Console.WriteLine("Du har valt att lägga till en Main tag, denna tag kan bara anges en gång");
                
                if (Maintag == true)
                {
                    Console.WriteLine("Det finns redan en main tag");
                } else
                {
                    InsertAtIndex("<main>");
                    InsertAtIndex("</main>");
                }

                Maintag = true;
            }

            public void AddTags()
            {
                Console.WriteLine("Välj vilken tagg som du vill lägga till");
                Console.WriteLine(" 1. H1\n 2. P\n 3. Main\n");

                int input = 0;

                while (true)
                {
                    input = Convert.ToInt32(Console.ReadLine());

                    if (input < 0 && input > 3)
                    {
                        Console.WriteLine("Ange ett giltigt input mellan 1-3");
                    }
                    else
                    {
                        break;
                    }
                }

                switch (input)
                {
                    case 1:
                        HeadTags();
                        break;
                    case 2:
                        PTags();
                        break;
                    case 3:
                        MainTag();
                        break;
                    default:
                        Console.WriteLine("Ingen giltig inmatning");
                        break;
                }
            }

            public void PTags()
            {
                Console.WriteLine("Du har valt att lägga till en P tagg, ange vad som ska stå på den taggen");
                string input = Console.ReadLine();

                string text = "<p>" + input + "</p>";

                InsertAtIndex(text);
            }


            public void HeadTags()
            {
                Console.WriteLine("Du har valt att lägga till en H tagg, ange vad som ska stå på den taggen.");
                string input = Console.ReadLine();

                string text = "<h1>" + input + "</h1>";

                InsertAtIndex(text);
            }

            public void DisplayWebsite()
            {
                foreach (string text in list)
                {
                    Console.WriteLine(text);
                }
            }

        }
    }
}
