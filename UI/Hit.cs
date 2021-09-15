using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hit : MonoBehaviour
{
	public TextMesh red;
	public TextMesh black;
	float startTime;
	float currentTime;
	public float lifespan; //文字显示时间

	public float fadeTime;  //退隐时间
	public float fadeSpeed; //退隐速度

	public float maxSize; //最大尺寸
	public float speed; // 自定义速度
	public string text;

	public Transform cam;
	// Use this for initialization
	void Start()
	{
		startTime = Time.fixedTime;//获取记录文字显示的开始时间
	}

    private void OnEnable()
    {
		cam = Camera.main.transform;
	}

    // Update is called once per frame
    void Update()
	{
		

		currentTime = Time.fixedTime - startTime;//当前游戏运行的时间
		if (currentTime > lifespan)//如果运行时间超过文字生命周期则销毁文字
		{
			Destroy(gameObject);
		}

        //fade out
        if (currentTime > fadeTime)//当时间超过渐隐设置的时间，开始渐隐
        {
            float fade = (fadeTime - (currentTime - fadeTime)) * (1 / fadeSpeed);
            Color tempColor = red.color;
            tempColor.a = fade;//渐隐透明度
            red.color = tempColor;//设计文字显示黄色，当然你可以根据需要设置颜色，new color也可以

            tempColor = black.color;
            tempColor.a = fade;
            black.color = tempColor;//背景阴影的显示
        }

        //scale and lift accordingly
        if (transform.localScale.x < maxSize)
		{
			transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (0.01f + currentTime);
		}//随着时间文字大小的变化实现
		transform.position += Vector3.up * Time.deltaTime * speed;//沿着y轴垂直上升
		//transform.LookAt(Camera.main.transform.position);
		//保持文字始终以最初的rect显示在摄像机视野
		red.text = text;
		black.text = text;

		gameObject.transform.forward = cam.forward;

	}
}
