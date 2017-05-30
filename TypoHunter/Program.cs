using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TypoHunter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.Write("The program requires 3 arguments: a file containing domains to check against a second file and a threshold for a match");
                Console.Write("For example: > TypoHunter newdomains.txt watchlist.txt 75");
                Console.Write("The files must contain a single domain name on each line. The threshold is a percentage of how close the domains must match. Only domain names above this threshold is shown.");
            }
            else
            {
                string newdomains = args[0];
                string watchlist = args[1];
                double threshold;
                if (Double.TryParse(args[2], out threshold))
                {
                    threshold = Double.Parse(args[2]);
                }
                else
                {
                    threshold = 80.0;
                }
                Compare(newdomains, watchlist, threshold);
                Console.ReadLine();
            }
        }
        static void Compare(string newdomains, string watchlist, double threshold)
        {
            foreach (string linea in File.ReadLines(@newdomains))
            {
                string bd = linea;
                foreach (string lineb in File.ReadLines(@watchlist))
                {
                    string baddomain = bd;
                    string gd = lineb;
                    baddomain = StripDomain(baddomain);
                    string gooddomain = StripDomain(lineb);
                    int lev = Levenshtein(gooddomain, baddomain);
                    if (gooddomain.Length > 0)
                    {
                        double ratio = (100 * (gooddomain.Length - lev) / gooddomain.Length);
                        if (ratio > threshold)
                        {
                            Console.Write("Domain: " + bd + "\t matches: " + gd + "\t by: " + ratio.ToString() + "\n");
                        }
                    }
                }
            }
        }
        static string StripDomain(string domain)
        {
            Char delimeter = '.';
            String[] splitdomain = domain.Split(delimeter);
            int len = splitdomain.Length;
            if (len > 1)
            {
                string name = splitdomain[len - 2];
                return name;
            }
            else if (len == 1)
            {
                string name = splitdomain[0];
                return name;
            }
            else
            {
                return "Domain name: " + domain + " is invalid";
            }
        }
        public static int Levenshtein(string s, string t)
        {
            // From DotNetPerls: https://www.dotnetperls.com/levenshtein
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {
                return n;
            }
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }
            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }
    }
}
