using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RPNtest {
    internal class Program {
        private const string Rpnfile = "RPN.txt";
        private const string Primesfile = "Primes.txt";
        private const string Rpncountfile = "RPNcount.txt";
        private static int _limit = 21474;
        private static int _rcount;
        private static int _pos;


        //private static void Main() {

        //    while (true){
        //        try{

        //            if (!File.Exists(Rpnfile)){
        //                //We only need to run this once, if there is no textfile
        //                GenerateRPNs();
        //            }
        //            else{
        //                try{
        //                    var reader = new StreamReader(Rpncountfile);
        //                    _rcount = Convert.ToInt32(reader.ReadLine());
        //                }
        //                catch (Exception){
        //                    Console.WriteLine("Can't read RPN count file.");
        //                }
        //            }

        //            Console.WriteLine("Enter a number between 1 and " + _rcount + " to find a Robust Prime Number");

        //            if (!int.TryParse(Console.ReadLine(), out _pos))
        //                Console.WriteLine("The entered value is NOT a number. Try again.");


        //            else{

        //                var reader = new StreamReader(Rpnfile);
        //                Console.WriteLine(GetSuperPrime(_pos, reader));
        //                //Console.WriteLine(" RPNs count: " + _rcount);
        //                Console.WriteLine("Press Enter to try again or Escape to quit");
        //                if (Console.ReadKey().Key == (ConsoleKey.Escape))
        //                    Environment.Exit(0);

        //            }
        //        }
        //        catch (Exception fx){

        //            Console.WriteLine(fx.Message);

        //            Console.ReadLine();


        //        }

        //    }
        //}

        private static void GeneratePrimes() {
            while(true){
            //We only need to run this once
            Console.WriteLine("Enter a number to generate prime numbers up to and press Enter.");
            string input = Console.ReadLine();
            int value;
            if (int.TryParse(input, out value))
                _limit = value;

            else{
                ConsoleKeyInfo cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.Escape)
                    break;
                if (cki.Key != ConsoleKey.Enter)
                    Console.WriteLine("Please enter a VALID number");
                Console.ReadLine();
            }
            GeneratePrimes(Primesfile);
            }
        }



        public static BitArray SieveOfEratosthenes(int limit) {
            var bits = new BitArray(limit + 1, true);
            bits[0] = false;
            bits[1] = false;
            for (int i = 0; i*i <= limit; i++){
                if (bits[i]){
                    for (int j = i*i; j <= limit; j += i){
                        bits[j] = false;
                    }
                }
            }
            return bits;
        }

        public static List<int> GeneratePrimesSieveOfEratosthenes(int n) {
            int limit = ApproximateNthPrime(n);
            BitArray bits = SieveOfEratosthenes(limit);
            var primes = new List<int>();
            for (int i = 0, found = 0; i < limit && found < n; i++){
                if (bits[i]){
                    primes.Add(i);
                    found++;
                }
            }
            return primes;
        }

        public static int ApproximateNthPrime(int nn) {
            double n = nn;
            double p;
            if (nn >= 7022){
                p = n*Math.Log(n) + n*(Math.Log(Math.Log(n)) - 0.9385);
            }
            else if (nn >= 6){
                p = n*Math.Log(n) + n*Math.Log(Math.Log(n));
            }
            else if (nn > 0){
                p = new[] {2, 3, 5, 7, 11}[nn - 1];
            }
            else{
                p = 0;
            }
            return (int) p;
        }


        private static void GeneratePrimes(string filename) {
            Console.WriteLine("Generating prime numbers. Wait..");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int pcount = 0;
            List<int> sievePrimes = GeneratePrimesSieveOfEratosthenes(_limit);
            using (var tw = new StreamWriter(filename, true)){
                foreach (int mp in sievePrimes){
                    tw.WriteLine(mp);
                    pcount++;
                    tw.Close();
                }
                }
                
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;


            Console.WriteLine("Primes generated, execution time: " + elapsedMs + " ms, count =" + pcount);

        }


        public static string GetSuperPrime(int start, StreamReader reader) {
            int count = 0;
            string line;
            while ((line = reader.ReadLine()) != null && count++ < start){

                if (count == start)
                    return line;
            }
            return "Not found at this position";
        }

        public static IEnumerable<short> GetPrimes(string start, StreamReader reader) {
            int count = 0;
            string line;
            int pos = Convert.ToInt32(start);
            while ((line = reader.ReadLine()) != null && count++ < pos){
                yield return short.Parse(line);
            }
        }






        private static void GenerateRPNs() {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (!File.Exists(Primesfile)){
                //We only need to run this once 
                GeneratePrimes();
            }

            Console.WriteLine("Generating all RPNs...");
            string line;
            var reader = new StreamReader(Primesfile);
            var rpns = new List<string>();
            while ((line = reader.ReadLine()) != null){
                string temp = line;

                foreach (char prime in line){

                    int match = 0;

                    while (temp.Length > 0 && !line.Contains("0")){

                        if (PrimeTool.IsPrime(Convert.ToInt32(temp)))
                            match++;
                        temp = temp.Remove(0, 1);
                        try{
                            if (match == line.Length){
                                rpns.Add(line);
                            }

                        }
                        catch (Exception){
                            Console.WriteLine("Oops! Exception!");
                          
                          
                        }


                    }
                }

            } // end while reading

            _rcount = rpns.Count;
            File.WriteAllLines(Rpnfile, rpns);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("RPNs generated, excution time: " + elapsedMs + "ms");
            Console.WriteLine("RPNs count " + _rcount);
            var writer = new StreamWriter(Rpncountfile);
            using (writer){
                writer.Write(_rcount);
            }

        }


        public static class PrimeTool {
            public static bool IsPrime(int candidate) {
                // Test whether the parameter is a prime number.
                if ((candidate & 1) == 0){
                    if (candidate == 2){
                        return true;
                    }
                    return false;
                }
                // Note://
                // I exclude 1 at the end.
                for (int i = 3; (i*i) <= candidate; i += 2){
                    if ((candidate%i) == 0){
                        return false;
                    }
                }
                return candidate != 1;
            }
        }


    }
}















