using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//base class for wave function collapse
public abstract class WaveFunctionCollapse
{
    protected bool[][] wave;
    protected int[][][] propagator;
    int[][][] compatible;
    protected int[] observed;

    (int, int)[] stack;
    int stackSize;

    //random generator

    protected int FMX, FMY, T;
    protected bool periodic;

    protected double[] weights;
    double[] weightLogWeights;

    int[] sumsOfOnes;
    double sumOfWeights, sumOfWeightLogWeights, startingEntropy;
    double[] sumsOfWeights, sumsOfWeightLogWeights, entropies;

    //stuff done

    protected WaveFunctionCollapse(int width, int height)
    {
        FMX = width;
        FMY = height;
    }

    void Init()
    {
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
                double noise = 1E-6 * UnityEngine.Random.value;
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
        int r = distribution.Random(UnityEngine.Random.value);

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

    protected void Propagate()
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
        UnityEngine.Random.seed = seed;

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

    protected void Ban(int i, int t)
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

    protected virtual void Clear()
    {
        for (int i = 0; i < wave.Length; i++)
        {
            for (int t = 0; t < T; t++)
            {
                wave[i][t] = true;
                for (int d = 0; d < 4; d++) compatible[i][t][d] = propagator[opposite[d]][t].Length;
            }

            sumsOfOnes[i] = weights.Length;
            sumsOfWeights[i] = sumOfWeights;
            sumsOfWeightLogWeights[i] = sumOfWeightLogWeights;
            entropies[i] = startingEntropy;
        }
    }

    protected abstract bool OnBoundary(int x, int y);

    protected static int[] DX = { -1, 0, 1, 0 };
    protected static int[] DY = { 0, 1, 0, -1 };
    static int[] opposite = { 2, 3, 0, 1 };
}

public class TwoDimWaveFunctionCollapse : WaveFunctionCollapse
{
    int N;
    
    byte[][] patterns;

    byte[,] sample;
    List<GameObject> tiles;
    int ground;

    public TwoDimWaveFunctionCollapse(byte[,] nSample, List<GameObject> nTiles, int N, int width, int height, bool periodicInput, bool periodicOutput, int symmetry, int nGround) : base(width, height)
    {
        this.N = N;
        periodic = periodicOutput;

        //
        sample = nSample;
        tiles = nTiles;
        int SMX = sample.GetLength(0);
        int SMY = sample.GetLength(1);
        int count = tiles.Count;
        long W = count.ToPower(N * N);

        byte[] pattern(Func<int, int, byte> f)
        {
            byte[] result = new byte[N * N];
            for(int y = 0; y < N; y++)
            {
                for(int x = 0; x < N; x++)
                {
                    result[x + y * N] = f(x, y);
                }
            }
            return result;
        };

        byte[] patternFromSample(int x, int y) => pattern((dx, dy) => sample[(x + dx) % SMX, (y + dy) % SMY]);
        byte[] rotate(byte[] p) => pattern((x, y) => p[N - 1 - y + x * N]);
        byte[] reflect(byte[] p) => pattern((x, y) => p[N - 1 - x + y * N]);

        long index(byte[] p)
        {
            long result = 0, power = 1;
            for(int i = 0; i < p.Length; i++)
            {
                result += p[p.Length - 1 - i] * power;
                power *= count;
            }
            return result;
        }

        byte[] patternFromIndex(long ind)
        {
            long residue = ind, power = W;
            byte[] result = new byte[N * N];

            for(int i = 0; i < result.Length; i++)
            {
                power /= count;
                int C = 0;
                
                while(residue >= power)
                {
                    residue -= power;
                    C++;
                }

                result[i] = (byte)C;
            }

            return result;
        }

        Dictionary<long, int> dicWeights = new Dictionary<long, int>();
        List<long> ordering = new List<long>();

        for(int y = 0; y < (periodicInput ? SMY: SMY - N + 1); y++)
        {
            for(int x = 0; x < (periodicInput ? SMX : SMX - N + 1); x++)
            {
                byte[][] ps = new byte[8][];

                ps[0] = patternFromSample(x, y);
                ps[1] = reflect(ps[0]);
                ps[2] = rotate(ps[0]);
                ps[3] = reflect(ps[2]);
                ps[4] = rotate(ps[2]);
                ps[5] = reflect(ps[4]);
                ps[6] = rotate(ps[4]);
                ps[7] = reflect(ps[6]);

                for(int k = 0; k < symmetry; k++)
                {
                    long ind = index(ps[k]);
                    if (dicWeights.ContainsKey(ind))
                    {
                        dicWeights[ind]++;
                    }
                    else
                    {
                        dicWeights.Add(ind, 1);
                        ordering.Add(ind);
                    }
                }
            }
        }

        T = dicWeights.Count;
        this.ground = (nGround + T) % T;
        patterns = new byte[T][];

        base.weights = new double[T];

        int counter = 0;
        foreach(long w in ordering)
        {
            patterns[counter] = patternFromIndex(w);
            base.weights[counter] = weights[w];
            counter++;
        }

        //bool agrees
    }



    protected override bool OnBoundary(int x, int y) => !periodic && (x + N > FMX || y + N > FMY || x < 0 || y < 0);

    protected override void Clear()
    {
        base.Clear();

        if (ground != 0)
        {
            for (int x = 0; x < FMX; x++)
            {
                for (int t = 0; t < T; t++) if (t != ground) Ban(x + (FMY - 1) * FMX, t);
                for (int y = 0; y < FMY - 1; y++) Ban(x + y * FMX, ground);
            }

            Propagate();
        }
    }
}
