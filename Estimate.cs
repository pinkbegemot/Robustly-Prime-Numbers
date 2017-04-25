namespace RPNtest {
    internal class Estimate {


        int Fibonacci(int n) {
            if (n == 1 || n == 0)
                return n;

            int fn = 0;
            int fn1 = 0;
            int fn2 = 1;
            for (int i = 2; i <= n; i++) {
                fn = fn1 + fn2;
                fn1 = fn2;
                fn2 = fn;
            }
            return fn;
        }

    
    }

}
