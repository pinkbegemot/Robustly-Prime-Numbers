using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNtest {

    /// <summary>
    /// This method encodes the Sieve of Eratosthenes, excluding
    /// all even numbers greater than 2 etc.
    /// </summary>
    internal class Program {
        static String input;
        private static void Calculate(string[] args) {
            do{
               
                string[] numbers = PrimesHash.GetAllPN();
                Console.WriteLine("Enter a number");
                input = Console.ReadLine();
                int a = Convert.ToInt32(input);

                int first = PrimesHash.GetPrime(a);

                Console.WriteLine(" PM " + first);
                string temp = input;
                //2147483648
                int match = 0;

                for (int i = 1; i < 214745; i++){
                    if (PrimeTool.IsPrime(i)){
                     
                        while (temp.Length > 1 && a > 0){
                            temp = temp.Remove(0, 1);
                            a = Convert.ToInt32(temp);
                            var ti = Convert.ToInt32(temp);
                            if (PrimeTool.IsPrime(ti)){
                                Console.WriteLine("Next PM " + ti);
                                match++;
                            }


                            if (match == input.Length - 1){
                                Console.WriteLine(input + " is an RPM ");
                                input = null;
                            }
                            //Console.Write(PrimesHash.GetPrime(ti));
                        }

                    }

                }
                
                Console.WriteLine("Enter a number");
              
            } while (String.IsNullOrEmpty(input));
            Console.Read();
        }
    }


    public static class PrimeTool {
        public static bool IsPrime(int candidate) {
            // Test whether the parameter is a prime number.
            if ((candidate & 1) == 0) {
                if (candidate == 2) {
                    return true;
                } else {
                    return false;
                }
            }
            // Note:
            // ... This version was changed to test the square.
            // ... Original version tested against the square root.
            // ... Also we exclude 1 at the end.
            for (int i = 3; (i * i) <= candidate; i += 2) {
                if ((candidate % i) == 0) {
                    return false;
                }
            }
            return candidate != 1;
        }
    }
}
