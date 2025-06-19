using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrail : MonoBehaviour
{
    //The number of vertices to create per frame
    private const int NUM_VERTICES = 12;

    [SerializeField]
    [Tooltip("The empty game object located at the tip of the blade")]
    private GameObject _tip = null;

    [SerializeField]
    [Tooltip("The empty game object located at the base of the blade")]
    private GameObject _base = null;

    [SerializeField]
    [Tooltip("The mesh object with the mesh filter and mesh renderer")]
    private GameObject _meshParent = null;

    [SerializeField]
    [Tooltip("The number of frame that the trail should be rendered for")]
    private int _trailFrameLength = 3;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private int _frameCount;
    private Vector3 _previousTipPosition;
    private Vector3 _previousBasePosition;

    void Start()
    {
        _mesh = new Mesh();
        _meshParent.GetComponent<MeshFilter>().mesh = _mesh;

        _vertices = new Vector3[_trailFrameLength * NUM_VERTICES];
        _triangles = new int[_vertices.Length];

        //Set starting position for tip and base
        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;
    }

    void LateUpdate()
    {
        //Reset the frame count one we reach the frame length
        if (_frameCount == (_trailFrameLength * NUM_VERTICES))
        {
            _frameCount = 0;
        }
        //Draw first triangle vertices for back and front
        _vertices[_frameCount] = _meshParent.transform.InverseTransformPoint(_base.transform.position);
        Vector3 basePos = _meshParent.transform.InverseTransformPoint(_base.transform.position);
        Vector3 tipPos = _meshParent.transform.InverseTransformPoint(_tip.transform.position);
        Vector3 prevBasePos = _meshParent.transform.InverseTransformPoint(_previousBasePosition);
        Vector3 prevTipPos = _meshParent.transform.InverseTransformPoint(_previousTipPosition);

        _vertices[_frameCount] = basePos;
        _vertices[_frameCount + 1] = tipPos;
        _vertices[_frameCount + 2] = prevTipPos;

        _vertices[_frameCount + 3] = basePos;
        _vertices[_frameCount + 4] = prevTipPos;
        _vertices[_frameCount + 5] = tipPos;

        _vertices[_frameCount + 6] = prevTipPos;
        _vertices[_frameCount + 7] = basePos;
        _vertices[_frameCount + 8] = prevBasePos;

        _vertices[_frameCount + 9] = prevTipPos;
        _vertices[_frameCount + 10] = prevBasePos;
        _vertices[_frameCount + 11] = basePos;

        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;
    }
}
