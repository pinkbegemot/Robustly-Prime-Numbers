using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RPNtest {
    /// <summary>
    /// The class generates primary numbers up to the specified limit, generates RPNs and finds a distinct RPN in the list.
    /// Author: Tatiana K. Jørgensen
    /// Created: 10.04.2017
    /// </summary>
    internal class RPNgenerator {
        private const string Rpnfile = "RPN.txt";
        private const string Primesfile = "Primes.txt";
        private const string Rpncountfile = "RPNcount.txt";
        private static int _limit;
        private static int _rcount;
        private static int _pos;
        private static List<int> Lprimes;


        private static void Main() {

            while (true) {
                Console.WriteLine();
                Console.WriteLine("Enter a limit up to 5000000 to generate RPNs and press Enter.");
                string input = Console.ReadLine();
                int value;
                if (int.TryParse(input, out value))
                    _limit = value;

                else {
                    ConsoleKeyInfo cki = Console.ReadKey();
                    if (cki.Key == ConsoleKey.Escape)
                        break;
                    if (cki.Key != ConsoleKey.Enter)
                        Console.WriteLine("Please enter a VALID number");
                    else
                        Console.WriteLine("The input " + input + " is not a VALID number");
                    Console.ReadLine(); ;

                }
                try {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    Console.WriteLine("Generating. ..It might take a while...");
                    GenerateRpmSieveOfEratosthenes(ApproximateNthPrime(_limit));
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    Console.WriteLine("RPNs generated, execution time: " + elapsedMs + "ms");
                    Console.WriteLine();
                    Console.WriteLine("What RPN do you want to check? Enter a number between 1 and " + _rcount);
                    _pos = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Found RPN: " + GetRpn(_pos));
                    Console.ReadLine();
                } catch (Exception ex) {

                    Console.WriteLine("Error:  " + ex.Message);

                }

            }
        }
        /// <summary>
        /// Method counts digits in any given number without converting the input parameter to String.
        /// </summary>
        /// <param name="x"></param>
        /// <returns>integer</returns>

        private static int CountDigits(int x) {
            int max_digits = 100;
            for (int i = 1; i < max_digits; i++) {
                if (x < Math.Pow(10, i)) {
                    return i;
                }
            }
            return 0;
        }
        /// <summary>
        /// Method generates prime numbers up to the specified limit and filters the Robust Prime Numbers.
        /// </summary>
        /// <param name="n">Upper limit</param>
        /// <returns>List<int>List of Integers<</returns>

        public static List<int> GenerateRpmSieveOfEratosthenes(int n) {
            int limit = ApproximateNthPrime(n);
            BitArray bits = SieveOfEratosthenes(limit);
            var primes = new List<int>();

            for (int i = 0, found = 0; i < limit && found < n; i++) {
                if (bits[i]) {
                    if (i < 8) {
                        primes.Add(i);
                        Console.WriteLine(i);
                    } else {

                        int length = CountDigits(i);
                        int temp = i;

                        for (int j = length, match = 1; j > 1; j--) {
                            temp /= 10;
                            int mod = SetModulo(length);
                            int mo = temp % 10;
                            if (!(mo == 0) && PrimeTool.IsPrime(i % mod)) {
                                length--;
                                match++;
                            }
                            if ((CountDigits(i) == 1 || match == CountDigits(i))) {
                                primes.Add(i);
                                Console.WriteLine(i);
                            }
                            found++;

                        }

                    }
                }
            }
            _rcount = primes.Count;
            Console.WriteLine("Total Count: " + _rcount);
            Lprimes = primes;
            return primes;
        }

        /// <summary>
        /// Returns the RPN in the specified position
        /// </summary>
        /// <param name="ndx"></param>
        /// <returns>Int</returns>
        static int GetRpn(int ndx) {
            return Lprimes[ndx - 1];
        }

        /// <summary>
        /// A helper method calculating the modulo pattern /length according to the RPN candidate number length.
        /// </summary>
        /// <param name="length"></param>
        /// <returns>String, example "100" or "1000"</returns>
        static int SetModulo(int l) {

            String value = "1";
            for (int i = 1; i < l; i++)
                value += "0";
            return Convert.ToInt32(value);

        }

        /// <summary>
        /// One of the many implementations of the classic Sieve of Eratosthenes.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns>BitArray</returns>


        public static BitArray SieveOfEratosthenes(int limit) {
            var bits = new BitArray(limit + 1, true);
            bits[0] = false;
            bits[1] = false;
            for (int i = 0; i * i <= limit; i++) {
                if (bits[i]) {
                    for (int j = i * i; j <= limit; j += i) {
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
            for (int i = 0, found = 0; i < limit && found < n; i++) {
                if (bits[i]) {
                    primes.Add(i);
                    found++;
                }
            }
            return primes;
        }

        /// <summary>
        /// Returns an approximate prime number
        /// </summary>
        /// <param name="nn"></param>
        /// <returns>int</returns>

        public static int ApproximateNthPrime(int nn) {
            double n = nn;
            double p;
            if (nn >= 7022) {
                p = n * Math.Log(n) + n * (Math.Log(Math.Log(n)) - 0.9385);
            } else if (nn >= 6) {
                p = n * Math.Log(n) + n * Math.Log(Math.Log(n));
            } else if (nn > 0) {
                p = new[] { 2, 3, 5, 7, 11 }[nn - 1];
            } else {
                p = 0;
            }
            return (int)p;
        }

        /// <summary>
        /// Method can be used for test purposes - it writes the prime Nos to a file.
        /// </summary>
        /// <param name="filename"></param>
        private static void GeneratePrimes(string filename) {
            Console.WriteLine("Generating prime numbers. Wait..");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int pcount = 0;
            List<int> sievePrimes = GeneratePrimesSieveOfEratosthenes(_limit);
            using (var tw = new StreamWriter(filename, true)) {
                foreach (int mp in sievePrimes) {
                    tw.WriteLine(mp);
                    pcount++;
                    tw.Close();
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;


            Console.WriteLine("Primes generated, execution time: " + elapsedMs + " ms, count =" + pcount);

        }


        /// <summary>
        /// DEBUG, Not used in Release. Writes RPNs to a file
        /// </summary>


        private static void GenerateRPNs() {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Console.WriteLine("Generating all RPNs...");
            string line;
            var reader = new StreamReader(Primesfile);
            var rpns = new List<string>();
            while ((line = reader.ReadLine()) != null) {
                string temp = line;

                foreach (char c in line) {

                    int match = 0;

                    while (temp.Length > 0 && !line.Contains("0")) {

                        if (PrimeTool.IsPrime(Convert.ToInt32(temp)))
                            match++;
                        temp = temp.Remove(0, 1);
                        try {
                            if (match == line.Length) {
                                rpns.Add(line);
                            }

                        } catch (Exception) {
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
            using (writer) {
                writer.Write(_rcount);
            }

        }

        /// <summary>
        ///  Internal class for checking the prime numbers.
        /// </summary>
        public static class PrimeTool {
            /// <summary>
            ///  Method tests, whether the parameter is a prime number.
            /// </summary>
            /// <param name="candidate"></param>
            /// <returns>Boolean</returns>
            public static bool IsPrime(int candidate) {

                if ((candidate & 1) == 0) {
                    if (candidate == 2) {
                        return true;
                    }
                    return false;
                }
                // We exclude 1 at the end.
                for (int i = 3; (i * i) <= candidate; i += 2) {
                    if ((candidate % i) == 0) {
                        return false;
                    }
                }
                return candidate != 1;
            }
        }


    }
}






