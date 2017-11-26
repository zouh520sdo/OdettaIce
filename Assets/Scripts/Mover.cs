using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour {

    public enum Player
    {
        ice,
        odetta,
        enemy,
        unit
    }

    public Player player;

    public float speed = 1.0f;
    public float maxSpeed;
    public float minSpeed;
    public bool canMove;
    public int enemyDir;

    protected CharactorBase charactor;
    protected Rigidbody2D rigid2d;
    protected JumpDetection jumpDetection;

    void Awake()
    {
        OnAwake();
    }

    // Use this for initialization
    void Start() {
        OnStart();
    }

    // Update is called once per frame
    void Update() {
        OnUpdate();
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
    }

    protected virtual void OnAwake()
    {
        enemyDir = 0;
        if (player == Player.unit)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
        rigid2d = GetComponent<Rigidbody2D>();
        foreach (Transform child in transform)
        {
            if (child.tag == "Feet")
            {
                jumpDetection = child.GetComponent<JumpDetection>();
            }
        }

        // 
        if (tag == "Ice" || tag == "Odetta")
        {
            charactor = GetComponent<CharactorBase>();
        }
    }
    protected virtual void OnUpdate()
    {

    }
    protected virtual void OnStart()
    {

    }
    protected virtual void OnFixedUpdate()
    {
        if (player == Player.enemy)
        {
            EnemyMove(enemyDir);
        }
        else
        {
            MyMove();
        }
    }

    protected virtual void MyMove()
    {
        float x = 0;
        if (canMove)
        {
            if (player == Player.odetta)
            {
                if (jumpDetection && jumpDetection.isOnGround && Input.GetKeyDown(KeyCode.W))
                {
                    rigid2d.AddForce(Vector2.up * 2000f);
                }
                x = Input.GetAxis("p1horizontal");
            }
            else if (player == Player.ice)
            {
                if (jumpDetection && jumpDetection.isOnGround && Input.GetKeyDown(KeyCode.UpArrow))
                {
                    rigid2d.AddForce(Vector2.up * 1000f);
                }
                x = Input.GetAxis("p2horizontal");
            }
            if (charactor)
            {
                if (x > 0)
                {
                    charactor.dir = 1;
                    Vector3 scale = transform.localScale;
                    scale.x = Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
                else if (x < 0)
                {
                    charactor.dir = -1;
                    Vector3 scale = transform.localScale;
                    scale.x = -Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
            }
        }
        Vector2 v = rigid2d.velocity;
        v.x = speed * x * Time.deltaTime;
        rigid2d.velocity = v;
    }

    public void EnemyMove(int dir)
    {
        if (canMove)
        {
            if (player == Player.enemy)
            {
                Vector2 v = rigid2d.velocity;
                v.x = speed * dir * Time.deltaTime;
                rigid2d.velocity = v;
            }
        }
        else
        {
            if (player == Player.enemy)
            {
                Vector2 v = rigid2d.velocity;
                v.x = 0;
                rigid2d.velocity = v;
            }
        }
    }
}
