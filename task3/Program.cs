using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace task3
{
    class Program
    {
        static string Converthmac(byte[] h)
        {
            return string.Join("", h.Select(e =>String.Format("{0:x2}", e)));
        }
        static string game(int rand, int user, int lenth)
        {
            int midleValue = lenth / 2;

            if (rand == user)
            {
                return "draw";
            }
            else if (user >= lenth)
            {
                if (user - rand > 0 && user - rand <= midleValue)
                    return "user";
                return "computer";
            }
            else if (user < lenth)
            {
                if (rand - user > 0 && rand - user <= midleValue)
                    return "computer";
                return "user";
            }


                return "";
            }

        static void Main(string[] args)
        {
            if (args.Length < 3) {
                throw new Exception("Error!! Minimal count of arguments 3");
            } else if (args.Distinct().Count() != args.Length){
                throw new Exception("Error! Arguments must be uniq");
            } else if (args.Length % 2 == 0) {
                throw new Exception("Error!! The numbers of arguments must be odd");
            }

            //key
            byte[] key = new byte[16];
            var rnum = RandomNumberGenerator.Create();
            rnum.GetBytes(key);

            //computer choise
            Random random = new Random();
            var randchoice = random.Next(0, args.Length - 1);
            byte[] choice = Encoding.Default.GetBytes(randchoice.ToString());

            //HMAC
            var hmac = new HMACSHA256(key);
            var hash = hmac.ComputeHash(choice);
            Console.WriteLine($"{Converthmac(hash)}");

            //menu
            string userchoise;
            int usernum;

            while (true) {
                Console.WriteLine("Choose your step:");
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {args[i]}");
                }
                Console.WriteLine("0 - Exit");
                Console.WriteLine("Your choise: ");
                userchoise = Console.ReadLine();

                try
                {
                    usernum = Convert.ToInt32(userchoise) - 1;
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong write");
                    continue;
                }

                if (usernum == -1)
                    return;
                if (usernum >= 0 && usernum < args.Length)
                    break;
            }

            Console.WriteLine($"Your choise: {args[usernum]}");
            Console.WriteLine($"Computer choise: {args[randchoice]}");

            string finish = game(randchoice, usernum, args.Length);

            switch (finish)
            {
                case "user":
                    Console.WriteLine("User win");
                    break;
                case "computer":
                    Console.WriteLine("Computer win");
                    break;
                case "draw":
                    Console.WriteLine("Draw!");
                    break;
                default:
                    break;
            }

            Console.WriteLine($"{Converthmac(hash)}");
        }
    }
}
