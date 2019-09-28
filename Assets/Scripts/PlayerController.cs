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
            new Vector3(-step, 0f, 0f),
            new Vector3(0f, 0f, step),
            new Vector3(step, 0f, 0f),
            new Vector3(0f, 0f, -step)
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
        nodes.Add(_tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(_tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(_tr.GetChild(2).GetComponent<Rigidbody>());

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
        switch(direction)
        {
            case Direction.RIGHT:
                nodes[1].position = nodes[0].position - new Vector3(nodeDist, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(nodeDist * 2f, 0f, 0f);
                break;
            case Direction.LEFT:
                nodes[1].position = nodes[0].position + new Vector3(nodeDist, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(nodeDist * 2f, 0f, 0f);
                break;
            case Direction.UP:
                nodes[1].position = nodes[0].position - new Vector3(0f, 0f, nodeDist);
                nodes[2].position = nodes[0].position - new Vector3(0f, 0f, nodeDist * 2);
                break;
            case Direction.DOWN:
                nodes[1].position = nodes[0].position + new Vector3(0f, 0f, nodeDist);
                nodes[2].position = nodes[0].position + new Vector3(0f, 0f, nodeDist * 2);
                break;
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
        if (other.tag == "Wall")
        {
            gameObject.SetActive(false);
        }
    }

    public void SetInputDirection(Direction dir)
    {
       if(dir == Direction.UP && direction == Direction.DOWN ||
          dir == Direction.DOWN && direction == Direction.UP ||
          dir == Direction.RIGHT && direction == Direction.LEFT ||
          dir == Direction.LEFT && direction == Direction.RIGHT )
        {
            return;
        }
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