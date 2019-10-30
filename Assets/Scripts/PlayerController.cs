using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour
{
    private float nodeDist = 1.6f;
    [HideInInspector]
    public Direction direction;

    [HideInInspector]
    public float step = 1.6f;

    [HideInInspector]
    public float movementFrequency = 0.2f;

    public float counter;
    private bool move;

    [SerializeField]
    private GameObject tail;

    public List<Vector3> deltaPos;

    private List<Rigidbody> nodes;
    private int initialNodes = 3;

    private Rigidbody mainBody;
    private Rigidbody headBody;
    private Transform _tr;

    bool createNewTail;

    void Awake()
    {
        _tr = transform;
        mainBody = GetComponent<Rigidbody>();

        InitSnakeNodes();
        InitPlayer();

        deltaPos = new List<Vector3>()
        {
            Vector3.left * step,
            Vector3.forward * step,
            Vector3.right * step,
            Vector3.back * step
        };
    }

    void Update()
    {
        CheckMovementFreq();
    }

    void FixedUpdate()
    {
        if (move)
        {
            move = false;
            Move();
        }
    }

    void InitSnakeNodes()
    {
        nodes = new List<Rigidbody>();
        for(int i = 0; i < initialNodes; i++)
        {
            nodes.Add(_tr.GetChild(i).GetComponent<Rigidbody>());
        }

        headBody = nodes[0];
    }

    void SetDirectionRandom()
    {
        int dirRandom = Random.Range(0, (int)Direction.COUNT);
        direction = (Direction)dirRandom;
    }

    void InitPlayer()
    {
        SetDirectionRandom();
        float offset = 1f;
        for (int i = 1; i < initialNodes; i++)
        {
            if(direction == Direction.RIGHT)
                nodes[i].position = nodes[0].position + Vector3.left * nodeDist * offset;
            else if(direction == Direction.LEFT)
                nodes[i].position = nodes[0].position - Vector3.left * nodeDist * offset;
            else if (direction == Direction.UP)
                nodes[i].position = nodes[0].position + Vector3.back * nodeDist * offset;
            else if (direction == Direction.DOWN)
                nodes[i].position = nodes[0].position - Vector3.back * nodeDist * offset;
            offset *= 2f;
        }
    }

    void Move()
    {
        Vector3 dPos = deltaPos[(int)direction];
        Vector3 parentPos = headBody.position;
        Vector3 prevPos;

        mainBody.position += dPos;
        headBody.position += dPos;

        for (int i = 1; i < nodes.Count; i++)
        {
            prevPos = nodes[i].position;
            nodes[i].position = parentPos;
            parentPos = prevPos;
        }

        if (createNewTail)
        {
            Vector3 pos = nodes[nodes.Count - 1].transform.position;
            nodes.Add(
               Instantiate(tail.GetComponent<Rigidbody>(),
               new Vector3(pos.x, pos.y, pos.z),
               Quaternion.identity,
               transform) as Rigidbody);
            createNewTail = false;
        }
    }

    void CheckMovementFreq()
    {
        counter += Time.deltaTime;

        if(counter >= movementFrequency)
        {
            counter = 0f;
            move = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
        {
            other.gameObject.SetActive(false);
            createNewTail = true;
        }
        else if (other.tag == "Wall" || other.tag == "Snake")
        {
            gameObject.SetActive(false);
        }
    }

    public void SetInputDirection(Direction dir)
    {
        if(System.Math.Abs(dir - direction) == 2)
            return;
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 90, 0));
        headBody.MoveRotation(headBody.rotation * deltaRotation);

        direction = dir;
        ForceMove();
    }

    void ForceMove()
    {
        counter = 0;
        move = false;
        Move();
    }
}