using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    //    //持续时间
    //    public float duration = 2f;
    //    //创建新残影间隔
    //    public float interval = 0.1f;

    //    //网格数据
    //    SkinnedMeshRenderer[] meshRender;


    //    void Start()
    //    {
    //        //获取身上所有的Mesh
    //        meshRender = this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

    //    }

    //    private float lastTime = 0;

    //    private Vector3 lastPos = Vector3.zero;

    //    void Update()
    //    {
    //        //人物有位移才创建残影
    //        if (lastPos == this.transform.position)
    //        {
    //            return;
    //        }
    //        lastPos = this.transform.position;
    //        if (Time.time - lastTime < interval)
    //        {//残影间隔时间
    //            return;
    //        }
    //        lastTime = Time.time;

    //        if (meshRender == null)
    //            return;
    //        for (int i = 0; i < meshRender.Length; i++)
    //        {
    //            Mesh mesh = new Mesh();
    //            meshRender[i].BakeMesh(mesh);

    //            GameObject go = new GameObject();
    //            go.hideFlags = HideFlags.HideAndDontSave;

    //            Shadow item = go.AddComponent<Shadow>();//控制残影消失
    //            item.duration = duration;
    //            item.deleteTime = Time.time + duration;

    //            MeshFilter filter = go.AddComponent<MeshFilter>();
    //            filter.mesh = mesh;

    //            MeshRenderer meshRen = go.AddComponent<MeshRenderer>();

    //            meshRen.material = meshRender[i].material;

    //            go.transform.localScale = meshRender[i].transform.localScale;
    //            go.transform.position = meshRender[i].transform.position;
    //            go.transform.rotation = meshRender[i].transform.rotation;

    //            item.meshRender = meshRen;
    //        }
    //    }
//    //开启残影
//    public bool _OpenAfterImage;

//    //残影颜色
//    public Color _AfterImageColor = Color.black;
//    //残影的生存时间
//    public float _SurvivalTime = 1;
//    //生成残影的间隔时间
//    public float _IntervalTime = 0.2f;
//    private float _Time = 0;
//    //残影初始透明度
//    [Range(0.1f, 1.0f)]
//    public float _InitialAlpha = 1.0f;

//    private List<AfterImage> _AfterImageList;
//    private SkinnedMeshRenderer _SkinnedMeshRenderer;

//    void Awake()
//    {
//        _AfterImageList = new List<AfterImage>();
//        _SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
//    }
//    void Update()
//    {
//        if (_AfterImageList != null)
//        {
//            if (_SkinnedMeshRenderer == null)
//            {
//                //_OpenAfterImage = false;
//                return;
//            }

//            _Time += Time.deltaTime;
//            //生成残影
//            CreateAfterImage();
//            //刷新残影
//            UpdateAfterImage();
//        }
//    }
//    /// <summary>
//    /// 生成残影
//    /// </summary>
//    void CreateAfterImage()
//    {
//        //生成残影
//        if (_Time >= _IntervalTime)
//        {
//            _Time = 0;

//            Mesh mesh = new Mesh();
//            _SkinnedMeshRenderer.BakeMesh(mesh);

//            Material material = new Material(_SkinnedMeshRenderer.material);
//            SetMaterialRenderingMode(material, RenderingMode.Fade);

//            _AfterImageList.Add(new AfterImage(
//                mesh,
//                material,
//                transform.localToWorldMatrix,
//                _InitialAlpha,
//                Time.realtimeSinceStartup,
//                _SurvivalTime));
//        }
//    }
//    /// <summary>
//    /// 刷新残影
//    /// </summary>
//    void UpdateAfterImage()
//    {
//        //刷新残影，根据生存时间销毁已过时的残影
//        for (int i = 0; i < _AfterImageList.Count; i++)
//        {
//            float _PassingTime = Time.realtimeSinceStartup - _AfterImageList[i]._StartTime;

//            if (_PassingTime > _AfterImageList[i]._Duration)
//            {
//                if (_AfterImageList[i] != null)
//                {
//                    Destroy(_AfterImageList[i]);
//                }
//                _AfterImageList.Remove(_AfterImageList[i]);
//                continue;
//            }

//            if (_AfterImageList[i]._Material.HasProperty("_Color"))
//            {
//                _AfterImageList[i]._Alpha *= (1 - _PassingTime / _AfterImageList[i]._Duration);
//                _AfterImageColor.a = _AfterImageList[i]._Alpha;
//                _AfterImageList[i]._Material.SetColor("_Color", _AfterImageColor);
//            }

//            Graphics.DrawMesh(_AfterImageList[i]._Mesh, _AfterImageList[i]._Matrix, _AfterImageList[i]._Material, gameObject.layer);
//        }
//    }
//    /// <summary>
//    /// 设置纹理渲染模式
//    /// </summary>
//    void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
//    {
//        switch (renderingMode)
//        {
//            case RenderingMode.Opaque:
//                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
//                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
//                material.SetInt("_ZWrite", 1);
//                material.DisableKeyword("_ALPHATEST_ON");
//                material.DisableKeyword("_ALPHABLEND_ON");
//                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
//                material.renderQueue = -1;
//                break;
//            case RenderingMode.Cutout:
//                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
//                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
//                material.SetInt("_ZWrite", 1);
//                material.EnableKeyword("_ALPHATEST_ON");
//                material.DisableKeyword("_ALPHABLEND_ON");
//                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
//                material.renderQueue = 2450;
//                break;
//            case RenderingMode.Fade:
//                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
//                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
//                material.SetInt("_ZWrite", 0);
//                material.DisableKeyword("_ALPHATEST_ON");
//                material.EnableKeyword("_ALPHABLEND_ON");
//                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
//                material.renderQueue = 3000;
//                break;
//            case RenderingMode.Transparent:
//                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
//                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
//                material.SetInt("_ZWrite", 0);
//                material.DisableKeyword("_ALPHATEST_ON");
//                material.DisableKeyword("_ALPHABLEND_ON");
//                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
//                material.renderQueue = 3000;
//                break;
//        }
//    }

//    public void DelayCancelShadow()
//    {
//        StartCoroutine(DelayCancelShadowCoroutine());
//    }
//    public void DelayCancelShadow(float _delayTime)
//    {
//        StartCoroutine(DelayCancelShadowCoroutine(_delayTime));
//    }
//    IEnumerator DelayCancelShadowCoroutine()
//    {
//        yield return new WaitForSeconds(_SurvivalTime);
//        _OpenAfterImage = true;
//    }
//    IEnumerator DelayCancelShadowCoroutine(float _delayTime)
//    {
//        yield return new WaitForSeconds(_delayTime);
//        _OpenAfterImage = true;
//    }
//}
//public enum RenderingMode
//{
//    Opaque,
//    Cutout,
//    Fade,
//    Transparent,
//}
//class AfterImage : Object
//{
//    //残影网格
//    public Mesh _Mesh;
//    //残影纹理
//    public Material _Material;
//    //残影位置
//    public Matrix4x4 _Matrix;
//    //残影透明度
//    public float _Alpha;
//    //残影启动时间
//    public float _StartTime;
//    //残影保留时间
//    public float _Duration;

//    public AfterImage(Mesh mesh, Material material, Matrix4x4 matrix4x4, float alpha, float startTime, float duration)
//    {
//        _Mesh = mesh;
//        _Material = material;
//        _Matrix = matrix4x4;
//        _Alpha = alpha;
//        _StartTime = startTime;
//        _Duration = duration;
//    }



}
