using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ProceduralTexture : MonoBehaviour {

    public Texture2D LightGrid;
    public Vector2 ImageDimensions;
    public Vector2 WindowDimensions;

    public int MaterialID = 0;

	void Start ()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material light_mat = mr.materials[MaterialID];
        LightUp();
        light_mat.SetTexture("_MainTex", LightGrid);
	}

    void LightUp()
    {
        for (int y = 1; y < ImageDimensions.y; y = y + 8)
        {
            for (int x = 1; x < ImageDimensions.x; x = x + 8)
            {
                if (LightGrid.GetPixel(x, y) == Color.white)
                {
                    float gradient = Random.Range(0.0f, 1.0f);
                    Color newColor = Color.Lerp(Color.black, Color.white, gradient);
                    for(int i = 0; i < WindowDimensions.y; i++)
                    {
                        for (int j = 0; j < WindowDimensions.x; j++)
                        {
                            LightGrid.SetPixel(x + j, y + i, newColor);
                        }
                    }
                }
            }
        }
        LightGrid.Apply();
    }
	
}
