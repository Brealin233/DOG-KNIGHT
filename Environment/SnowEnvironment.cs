using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowEnvironment : MonoBehaviour
{
    public RenderTexture rt;
    public Texture drawImg;
    public Texture defaultImg;

    public Camera mainCam;
    public GameObject player;

    void Start()
    {
        mainCam = Camera.main.GetComponent<Camera>();

        GetComponent<Renderer>().material.mainTexture = rt;

        DrawDefault();
    }

    public void DrawDefault()
    {
        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, rt.width, rt.height, 0);

        Rect rect = new Rect(0, 0, rt.width, rt.height);
        //绘制贴图
        Graphics.DrawTexture(rect, defaultImg);

        GL.PopMatrix();
        RenderTexture.active = null;
    }

    public void Draw(int x,int y)
    {
        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, rt.width, rt.height, 0);

        x -= (int)(drawImg.width * 0.5f);
        y -= (int)(drawImg.height * 0.5f);

        Rect rect = new Rect(x, y, drawImg.width, drawImg.height);//设置像素图像
        //绘制贴图
        Graphics.DrawTexture(rect, drawImg);

        GL.PopMatrix();
        RenderTexture.active = null;
    }

    void Update()
    {
        //if (Input.GetMouseButton(0))
        //{
            //Debug.Log("按下");
            Ray ray = mainCam.ScreenPointToRay(player.transform.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                int x = (int)(hit.textureCoord.x * rt.width); //转化为像素,乘上rt
                int y = (int)(rt.height - hit.textureCoord.y * rt.height); //前面减数是解决绘制方向
                Draw(x, y);
            }
        //}
    }
}
