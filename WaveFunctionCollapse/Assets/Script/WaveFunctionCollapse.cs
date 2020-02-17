using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//base class for wave function collapse
public class WaveFunctionCollapse
{
    bool[][] wave;
    int[][][] propagator;
    int[][][] compatible;
    int[] observed;

    (int, int)[] stack;
    int stackSize;

    //random generator

    int FMX, FMY, T;
    bool periodic;

    double[] weights;
    double[] weightLogWeights;

    int[] sumsOfOnes;
    double sumOfWeights, sumOfWeightLogWeights, startingEntropy;
    double[] sumsOfWeights, sumsOfWeightLogWeights, entropies;

    //stuff done

    public WaveFunctionCollapse(int width, int height)
    {
        FMX = width;
        FMY = height;
    }

    void Init()
    {
        random = new Random();
        wave = new bool[FMX * FMY][];
        compatible = new int[wave.Length][][];
        for(int i = 0; i < wave.Length; i++)
        {
            wave[i] = new bool[T];
            compatible[i] = new int[T][];
            for(int t = 0; t < T; t++)
            {
                compatible[i][t] = new int[4];
            }
        }

        weightLogWeights = new double[T];
        sumOfWeights = 0;
        sumOfWeightLogWeights = 0;

        for(int t = 0; t < T; t++)
        {
            weightLogWeights[t] = weights[t] * Mathf.Log((float)weights[t]);
            sumOfWeights += weights[t];
            sumOfWeightLogWeights += weightLogWeights[t];
        }
        startingEntropy = Mathf.Log((float)sumOfWeights) - sumOfWeightLogWeights / sumOfWeights;

        sumsOfOnes = new int[FMX * FMY];
        sumsOfWeights = new double[FMX * FMY];
        sumsOfWeightLogWeights = new double[FMX * FMY];
        entropies = new double[FMX * FMY];

        stack = new (int, int)[wave.Length * T];
        stackSize = 0;
    }

    bool? Observe()
    {
        double min = 1E+3;
        int argmin = -1;

        for(int i = 0; i < wave.Length; i++)
        {
            if (OnBoundary(i % FMX, i / FMX)) continue;

            int amount = sumsOfOnes[i];
            if (amount == 0) 
                return false;

            double entropy = entropies[i];
            if(amount > 1 && entropy <= min)
            {
                double noise = 1E-6 * Random.value;
                if(entropy + noise < min)
                {
                    min = entropy + noise;
                    argmin = i;
                }
            }
        }

        if(argmin == -1)
        {
            observed = new int[FMX * FMY];
            for(int i = 0; i < wave.Length; i++)
            {
                for(int t = 0; t < T; t++)
                {
                    if(wave[i][t])
                    {
                        observed[i] = t;
                        break;
                    }
                }
            }

            return true;
        }

        double[] distribution = new double[T];
        for(int t = 0; t < T; t++)
        {
            distribution[t] = wave[argmin][t] ? weights[t] : 0;
        }
        int r = distribution.Random(Random.value);

        bool[] w = wave[argmin];
        for(int t = 0; t < T; t++)
        {
            if(w[t] != (t == r))
            {
                Ban(argmin, t);
            }
        }

        return null;
    }

    void Propagate()
    {
        while(stackSize > 0)
        {
            var e1 = stack[stackSize - 1];
            stackSize--;

            int i1 = e1.Item1;
            int x1 = i1 % FMX, y1 = i1 / FMX;

            for(int d = 0; d < 4; d++)
            {
                int dx = DX[d], dy = DY[d];

                int x2 = x1 + dx, y2 = y1 + dy;

                if (OnBoundary(x2, y2))
                    continue;

                if(x2 < 0)
                {
                    x2 += FMX;
                }
                else if(x2 >= FMX)
                {
                    x2 -= FMX;
                }

                if(y2 < 0)
                {
                    y2 += FMX;
                }
                else if (y2 >= FMY)
                {
                    y2 -= FMY;
                }

                int i2 = x2 + y2 * FMX;
                int[] p = propagator[d][e1.Item2];
                int[][] compat = compatible[i2];
                
                for(int l = 0; l < p.Length; l++)
                {
                    int t2 = p[l];
                    int[] comp = compat[t2];

                    comp[d]--;
                    if(comp[d] == 0)
                    {
                        Ban(i2, t2);
                    }
                }
            }
        }
    }

    public bool Run(int seed, int limit)
    {
        if(wave == null)
        {
            Init();
        }

        Clear();
        Random.seed = seed;

        for(int l = 0; l < limit || limit == 0; l++)
        {
            bool? result = Observe();
            if(result != null)
            {
                return (bool)result;
            }
            Propagate();
        }

        return true;
    }

    void Ban(int i, int t)
    {
        wave[i][t] = false;

        int[] comp = compatible[i][t];
        for(int d = 0; d < 4; d++)
        {
            comp[d] = 0;
        }
        stack[stackSize] = (i, t);
        stackSize++;

        sumsOfOnes[i] -= 1;
        sumsOfWeights[i] -= weights[t];
        sumsOfWeightLogWeights[i] -= weightLogWeights[t];

        double sum = sumsOfWeights[i];
        entropies[i] = Mathf.Log((float)sum) - sumsOfWeightLogWeights[i] / sum;
    }

    void Clear()
    {
        for(int i = 0; i < wave.Length; i++)
        {
            for(int t = 0; t < T; t++)
            {
                wave[i][t] = true;
                for(int d = 0; d < 4; d++)
                {
                    compatible[i][t][d] = propagator[opposite[d]][t].Length;
                }
            }

            sumsOfOnes[i] = weights.Length;
            sumsOfWeights[i] = sumOfWeights;
            sumsOfWeightLogWeights[i] = sumOfWeightLogWeights;
            entropies[i] = startingEntropy;
        }
    }

    bool OnBoundary(int x, int y) => !periodic && (x < 0 || y < 0 || x >= FMX || y >= FMY);

    protected static int[] DX = { -1, 0, 1, 0 };
    protected static int[] DY = { 0, 1, 0, -1 };
    static int[] opposite = { 2, 3, 0, 1 };
}


