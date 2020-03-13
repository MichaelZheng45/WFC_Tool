using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public List<GameObject> objList;
    public List<Color> colorList;
    public sampleMap mapData;

    public Texture2D sampleTex;
    public Texture2D outputTex;

    public int outputX;
    public int outputY;
    public int seed;
    private void Start()
    {
        generate();
    }
    public void generate()
    {
        objList = new List<GameObject>();
        byte[,] sampleData = new byte[sampleTex.width, sampleTex.height];
        colorList = new List<Color>();
        for(int y= 0; y < sampleTex.height; y++)
        {
            for(int x = 0; x < sampleTex.width; x++)
            {
                Color newColor = sampleTex.GetPixel(x, y);
                int index = 0;
                foreach(Color c in colorList)
                {
                    if (c == newColor) break;
                    index++;
                }

                if (index == colorList.Count) colorList.Add(newColor);
                sampleData[x, y] = (byte)index;
            }
        }

        //new class
        TwoDimWaveFunctionCollapse model = new TwoDimWaveFunctionCollapse(sampleData, objList, colorList, 8, outputX, outputY, false, false, 8, 0);

        //class.run
        for(int i = 0; i < 10; i++)
        {
            if(model.Run(seed, 0));
            {
                break;
            }
        }


        //draw
        gameObject.GetComponent<Renderer>().material.mainTexture = model.draw(outputTex);

    }

}
