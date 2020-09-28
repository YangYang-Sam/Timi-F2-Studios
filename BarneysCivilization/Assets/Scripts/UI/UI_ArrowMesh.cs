using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ArrowMesh : MonoBehaviour
{
    public static UI_ArrowMesh instance;
    private void Awake()
    {
        instance = this;
    }
    // 重力加速度
    [SerializeField]
    private Vector3 G = new Vector3(0.0f, 0.0f, 9.8f);
    //间隔时间
    [SerializeField]
    private float _FixedTime = 0.05f;

    // 速度  保证速度均匀，路径越长时间越长
    [SerializeField]
    private float _Speed = 10;
    public float Speed
    {
        set
        {
            _Speed = value;
        }
        get
        {
            return _Speed;
        }
    }
    //箭头的宽度
    [SerializeField]
    private float _ArrowWidth = 2.0f;

    private MeshFilter _MeshFilter;
    private MeshRenderer _MeeshRenderer;

    public bool Visible;
    // private Vector3 _StartPos;
#if UNITY_EDITOR
    private void OnValidate()
    {
    }
#endif

    public void SetVisibility(bool visible)
    {
        if (visible != Visible)
        {
            Visible = visible;
            _MeeshRenderer.enabled = visible;
        }
    }

    private void Start()
    {
        _MeshFilter = GetComponent<MeshFilter>();
        _MeeshRenderer = GetComponent<MeshRenderer>();
    }
    
    public void UpdatePosition(Vector3 _endPos)
    {
        if (!_MeshFilter)
            _MeshFilter = GetComponent<MeshFilter>();
        List<Vector3> _pos = GetRadianPos(Vector3.zero, _endPos-transform.position);
        CreateMesh(_MeshFilter, _pos);
    }

    #region 创建模型
    void CreateMesh(MeshFilter _meshFilter, List<Vector3> _pos)
    {
        int _num = _pos.Count - 1;
        if (_num < 1)
            return;
        float _halfWidth = _ArrowWidth * 0.5f;
        Vector3 _dir = GetDir(_pos[0], _pos[_num]);

        //Vector3[] _vertices = new Vector3[_num*4+8];
        //Vector2[] _uv = new Vector2[_num * 4 + 8];
        //int[] _triangle = new int[_num * 6 + 12];
        Vector3[] _vertices = new Vector3[_num * 4];
        Vector2[] _uv = new Vector2[_num * 4];
        int[] _triangle = new int[_num * 6];
        for (int i = 0; i < _num; i++)
        {
            //计算顶点位置  
            _vertices[i * 4 + 0] = _pos[i] + _dir * _halfWidth;
            _vertices[i * 4 + 1] = _pos[i + 1] - _dir * _halfWidth;
            _vertices[i * 4 + 2] = _pos[i + 1] + _dir * _halfWidth;
            _vertices[i * 4 + 3] = _pos[i] - _dir * _halfWidth;

            //计算uv位置  
            _uv[i * 4 + 0] = new Vector2(0.0f, 0.0f);
            _uv[i * 4 + 1] = new Vector2(1.0f, 1.0f);
            _uv[i * 4 + 2] = new Vector2(1.0f, 0.0f);
            _uv[i * 4 + 3] = new Vector2(0.0f, 1.0f);
        }

        int _verticeIndex = 0;

        for (int i = 0; i < _num; i++)
        {
            // 第一个三角形  
            _triangle[_verticeIndex++] = i * 4 + 0;
            _triangle[_verticeIndex++] = i * 4 + 1;
            _triangle[_verticeIndex++] = i * 4 + 2;
            // 第二个三角形  
            _triangle[_verticeIndex++] = i * 4 + 1;
            _triangle[_verticeIndex++] = i * 4 + 0;
            _triangle[_verticeIndex++] = i * 4 + 3;
        }
        Mesh _newMesh = new Mesh();
        _newMesh.vertices = _vertices;
        _newMesh.uv = _uv;
        _newMesh.triangles = _triangle;
#if UNITY_EDITOR
        _meshFilter.sharedMesh = _newMesh;
#else
        _meshFilter.mesh = _newMesh;  
#endif
    }
    #endregion

    #region 获取箭头的垂直向量
    Vector3 GetDir(Vector3 _start, Vector3 _end)
    {
        Vector3 _dirValue = (_end - _start).normalized;
        //因为不需要考虑z轴的向量，加一个条件，即可得出唯一垂直向量
        Vector3 _dir = new Vector3(Mathf.Abs(_dirValue.z),0, -1.0f * Mathf.Sign(_dirValue.x * _dirValue.z) * Mathf.Abs(_dirValue.x));
        if (_dirValue.z < 0)
            _dir *= -1.0f;
        return _dir;
    }
    #endregion

    #region 获取两点之间的点
    List<Vector3> GetRadianPos(Vector3 _startPos, Vector3 _endPos)
    {
        List<Vector3> _pos = new List<Vector3>();

        float _LifeTime = 4 /*Vector3.Distance(_startPos, _endPos) / _Speed*/;

        // 朝上移动:v=v0-gt;   v0=v+gt; v0=0;
        Vector3 _startSpeed = (_endPos - _startPos) / _LifeTime + G * (_LifeTime * 0.5f);
        for (float _moveTime = 0.0f; _moveTime <= _LifeTime; _moveTime += _FixedTime)
        {
            if (_moveTime > _LifeTime)
                _moveTime = _LifeTime;
            Vector3 _tmpPos = _startPos + (_startSpeed * _moveTime - 0.5f * G * _moveTime * _moveTime);
            _pos.Add(_tmpPos);
        }

        return _pos;
    }
    #endregion
}
