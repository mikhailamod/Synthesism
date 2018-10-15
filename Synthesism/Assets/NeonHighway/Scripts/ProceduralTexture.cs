using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ProceduralTexture : MonoBehaviour {

    public Texture2D LightGrid;
    private Texture2D instanceTexture;
    public Vector2 ImageDimensions;
    public Vector2 WindowDimensions;

    public int MaterialID = 0;
    public Vector2 tiling;

    public Color[] colors;

	void Start ()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material light_mat = mr.materials[MaterialID];
        light_mat.mainTextureScale = tiling;
        instanceTexture = new Texture2D((int)ImageDimensions.x,(int)ImageDimensions.y);
        Graphics.CopyTexture(LightGrid, instanceTexture);
        light_mat.SetTexture("_MainTex",instanceTexture);
        LightUp();
    }

    void LightUp()
    {
        for (int y = 1; y < ImageDimensions.y; y = y + 8)
        {
            for (int x = 1; x < ImageDimensions.x; x = x + 8)
            {
                if (LightGrid.GetPixel(x, y) == Color.white)
                {
                    int randColorID = Random.Range(0, colors.Length);
                    float gradient = Random.Range(0.0f, 1.0f);
                    Color newColor = Color.Lerp(Color.black, colors[randColorID], gradient);
                    for(int i = 0; i < WindowDimensions.y; i++)
                    {
                        for (int j = 0; j < WindowDimensions.x; j++)
                        {
                            instanceTexture.SetPixel(x + j, y + i, newColor);
                        }
                    }
                }
            }
        }
        instanceTexture.Apply();
    }
	
}
