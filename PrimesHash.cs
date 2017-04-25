using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNtest {
    public static class PrimesHash {
        private static int[] primes;

        static PrimesHash() {

            //
            // Initialize array of  the first primes in the range.
            //
            primes = new int[] {
                3, 7, 11, 17, 23, 29, 37,
                47, 59, 71, 89, 107, 131,
                163, 197, 239, 293, 353,
                431, 521, 631, 761, 919,
                1103, 1327, 1597, 1931,
                2333
            };
        }
        ////2147483647
        //public static int[] GetAllPN() {
        //    if (primefactors.Count < 1)
        //        for (int j = 1; j < 2147; j += 2){
        //            if (PrimeTool.IsPrime(j)){
        //                primefactors.Add(j));
                       
        //            }
        //        }
        //    primes = new int[]{}
        //    return primefactors.ToArray();

        //}

        private static List<string> primefactors = new List<string>();


        //public static int GetPrime(int min) {
        //    //
        //    // Get the first hashtable prime number
        //    // ... that is equal to or greater than the parameter.
        //    //
        //    return
        //        primefactors.G
        //    //    for (int i = 0; i < primes.Length; i++)
        //    //    {
            //        int num2 = primes[i];
            //        if (num2 >= min)
            //        {

            //            return num2;
            //        }
            //    }
            //    for (int j = min | 1; j < 2147483647; j += 2)
            //    {
            //        if (PrimeTool.IsPrime(j))
            //        {
            //            primefactors.Add(j.ToString());
            //            return j;
            //        }
            //    }
            //    return min;
            //}
        }

    }
}
