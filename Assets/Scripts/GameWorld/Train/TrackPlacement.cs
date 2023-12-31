using GameWorld.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum TrackType { TRACK_LEFT = -1, TRACK_STRAIGHT = 0, TRACK_RIGHT = 1 }

public class TrackPlacement : MonoBehaviour
{
    public float TRACK_HEIGHT= 1.1f;

    public Pool<Transform> m_TracksPool = new Pool<Transform>();
    

    //[SerializeField] int m_MaxTrackPlacement = 3;
    [SerializeField] private LayerMask m_CollectibleLayer;
    [SerializeField] private LayerMask m_TrainLayer;
    [SerializeField] private LayerMask m_SpawnCheckHitLayer;
    [SerializeField] private Pool<ParticleSystem> m_PlacementPfxPool;

    private int m_StartingTrackIndex;
    private Train m_Train;

    public int StartingTrackIndex => m_StartingTrackIndex;

    private void Awake()
    {
        m_Train = GetComponent<Train>();

        m_TracksPool.Initialize(new GameObject("Track Parent").transform);
        m_PlacementPfxPool.Initialize(new GameObject("Track Placement Pfx Parent").transform);

        // Spawn Initial Track
        PreTrackInitializeSpawn(-1);
        PreTrackInitializeSpawn(0);
        PreTrackInitializeSpawn(1);
        PreTrackInitializeSpawn(2);
        PreTrackInitializeSpawn(3);

        m_StartingTrackIndex = 1;
    }

    private void PreTrackInitializeSpawn(float zPos)
    {
        Transform track = m_TracksPool.GetNextObject();
        track.position = new Vector3(0, TRACK_HEIGHT, zPos);
        Track trackScript = track.GetComponent<Track>();
        trackScript.DisplayTrack(TrackType.TRACK_STRAIGHT);
        trackScript.SetTrackTravelled();
        track.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SpawnTrainTrack(TrackType.TRACK_LEFT);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            SpawnTrainTrack(TrackType.TRACK_STRAIGHT);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnTrainTrack(TrackType.TRACK_RIGHT);
        }
    }

    public void SpawnTrainTrack(TrackType trackType)
    {
        Transform oldTrack = m_TracksPool.GetCurrentObject();
        Vector3 spawnPos = oldTrack.position + oldTrack.forward;
        Transform newTrack = m_TracksPool.GetNextObject();

        // Check if tile is empty
        Collider[] hitCollider = Physics.OverlapSphere(spawnPos, 0.45f, m_SpawnCheckHitLayer);
         
        if (hitCollider.Length > 0)
        {
            
            return;
        }

        newTrack.position = spawnPos;
        newTrack.rotation = Quaternion.identity;


        float rotationY = 0;

        if (trackType == TrackType.TRACK_LEFT)
        {
            //newTrack.GetComponent<MeshRenderer>().material = m_MatLeft;
            rotationY = oldTrack.eulerAngles.y - 90;
        }
        else if (trackType == TrackType.TRACK_STRAIGHT)
        {
            rotationY = oldTrack.eulerAngles.y;
            //newTrack.GetComponent<MeshRenderer>().material = m_MatForward;

        }
        else if (trackType == TrackType.TRACK_RIGHT)
        {
            rotationY = oldTrack.eulerAngles.y + 90;
            //newTrack.GetComponent<MeshRenderer>().material = m_MatRight;

        }

        if (rotationY == 360) rotationY = 0;

        newTrack.Rotate(new Vector3(0, rotationY, 0));

        // Activate Track
        Track trackScript = newTrack.GetComponent<Track>();
        trackScript.DisplayTrack(trackType);
        newTrack.gameObject.SetActive(true);
        ParticleSystem pfx = m_PlacementPfxPool.GetNextObject();
        pfx.transform.position = newTrack.position;
        pfx.Play();
    }

}
