using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net.Http;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Hemsida
{
    internal class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Skriv in titeln för din hemsida");
            string title = Console.ReadLine();

            Console.WriteLine("Vill du ha en stylad HeadTag?\nY/N\n");
            string htmlContent = "";
            string styledHead = Console.ReadLine();
            if (styledHead.ToUpper() == "Y")
            {

                StyledWebSiteGenerator styledWebsite = new StyledWebSiteGenerator(title);
                styledWebsite.AddTags();
                styledWebsite.AddTableOfPersons();
                htmlContent = styledWebsite.GetHtml();

            }
            else
            {
                WebSiteGenerator website = new WebSiteGenerator(title);
                website.AddTags();
                website.AddTableOfPersons();
                htmlContent = website.GetHtml();
            }


            HtmlUtilities.DisplayHtmlInBrowser(title, htmlContent, true);
        }


    }

    public interface IHtmlPage
    {
        public string GetHtml();
    }

    class WebSiteGenerator : IHtmlPage
    {
        private bool _maintag = false;
        public string Title;

        protected List<string> list = new List<string>
        {
            "<!DOCTYPE html>",
            "<html>",
            "<body>",
            "</body>",
            "</html>",
        };

        public WebSiteGenerator(string title)
        {
            Title = title;
        }

        protected virtual void PrintHead()
        {
            list.Insert(1, $"<head>\n{Title}\n</head>");
        }

        public void InsertAtIndex(string input)
        {
            Console.WriteLine("Vart vill du placera taggen?");
            Console.WriteLine("Du får inte placera det innan första <body> taggen och efter sista </body> taggen\n");

            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {list[i]}");
            }

            int val = 0;

            while (true)
            {
                Console.WriteLine($"Ange en position mellan 3 och {list.Count - 2}: ");
                val = Convert.ToInt32(Console.ReadLine()) - 1;

                if (val >= 2 && val <= (list.Count - 2)) 
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Felaktig position, försök igen.");
                }
            }

            list.Insert(val + 1, input);
            Console.WriteLine($"Taggen har lagts till på position {val + 2}.");
        }



        public void MainTag()
        {
            Console.WriteLine("Du har valt att lägga till en Main tag, denna tag kan bara anges en gång");

            if (_maintag)
            {
                Console.WriteLine("Det finns redan en main tag");
            } else
            {
                InsertAtIndex("<main>");
                InsertAtIndex("</main>");
                _maintag = true;
            }


        }

        public void AddTableOfPersons()
        {
            string tableHtml = "<table>";
            string csvFilePath = "people-100.csv";

            bool checkedFirst = false;

            try
            {
                using (var reader = new StreamReader(csvFilePath))
                {

                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!checkedFirst)
                        {
                            checkedFirst = true;
                            string[] headerInfo = line.Split(",");
                            tableHtml += "<tr>\n";

                            foreach (var header in headerInfo)
                            {
                                tableHtml += $"<th>{header}</th>\n";
                            }
                            tableHtml += "</tr>\n";
                            
                            continue;    
                        }

                        Person person = Person.ParsePerson(line);
                        tableHtml += $"<tr>\n<td>{person.Index}</td>\n<td>{person.Id}</td>\n<td>{person.Name}</td>\n<td>{person.LastName}</td>\n<td>{person.Sex}</td>\n<td>{person.Email}</td>\n<td>{person.Phone}</td>\n<td>{person.DateOfBirth}</td>\n<td>{person.JobTitle}</td>\n</tr>\n";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                tableHtml += "</table>";
                InsertAtIndex(tableHtml);
            }

        }

        public void AddTags()
        {
            Console.WriteLine("Välj vilken tagg som du vill lägga till");
            Console.WriteLine(" 1. H1\n 2. P\n 3. Main\n");

            int input = 0;

            while (true)
            {
                input = Convert.ToInt32(Console.ReadLine());

                if (input < 1 || input > 3)
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

        public string GetHtml()
        {
            PrintHead();
            StringBuilder htmlBuilder = new StringBuilder();

            foreach (string text in list)
            {
                htmlBuilder.AppendLine(text);

            }

            return htmlBuilder.ToString();
        }

        static public void WriteToFile(string htmlText)
        {
            using (StreamWriter sw = new StreamWriter("Hemsida.txt"))
            {
                sw.WriteLine(htmlText);
            }
        }

    }
    public class Person
    {
        public int Index { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        public string JobTitle { get; set; }

        public override string ToString()
        {
            return $"{Index} {Id} {Name} {LastName} {Sex} {Email} {Phone} {DateOfBirth} {JobTitle}";
        }

        static public Person ParsePerson(string csvLine)
        {
            string[] personInfo = csvLine.Split(",");
            

            int index = int.TryParse(personInfo[0], out int IndId) ? IndId : 0;

            return new Person
            {
                Index = index,
                Id = personInfo[1],
                Name = personInfo[2],
                LastName = personInfo[3],
                Sex = personInfo[4],
                Email = personInfo[5],
                Phone = personInfo[6],
                DateOfBirth = personInfo[7],
                JobTitle = personInfo[8],
            };
        }
    }

    public static class HtmlUtilities
    {
        /// <summary>
        /// Save html content to a temporary file and open it in a browser
        /// </summary>
        /// <param name="pageTitle">Page-title is used as filename</param>
        /// <param name="htmlContent">Sets the html for the file that is to be display</param>
        /// <param name="openFile">Opens the file in browser. True is default</param>
        /// <param name="webbrowserName">Chrome is default but can be set to any webbrowser. Don't include .exe in the name<
        public static void DisplayHtmlInBrowser(string pageTitle, string htmlContent, bool openFile = true, string webbrowserName = "chrome")
        {
            pageTitle = pageTitle.Replace(" ", "_");
            var tempPath = System.IO.Path.GetTempPath();
            string htmlFile = Path.Combine(tempPath, $"{pageTitle}.html");
            File.WriteAllText(htmlFile, htmlContent);
            if (openFile)
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = webbrowserName;
                process.StartInfo.Arguments = @$"file:///{htmlFile}";
                process.Start();
            }
        }
    }



    class StyledWebSiteGenerator : WebSiteGenerator
    {
        public string? Color { get; set; }

        public StyledWebSiteGenerator(string title, string? color = null)
            : base(title)
        {
            Color = color;
        }

        protected override void PrintHead()
        {
            list.Insert(1, $"<head>\n<style>\nbody {{ margin: 0; padding: 0; }}\ntable {{ width: 100%; border-collapse: collapse; }}\nth, td {{ border: 1px solid black; padding: 8px; text-align: left; }}\nth {{ background-color: #f2f2f2; }}\n</style>\n<title>{Title}</title>\n</head>");
        }
    }


}
